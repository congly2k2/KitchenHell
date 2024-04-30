using System;
using GameBase;
using RecipeSO;
using SyncNetwork;
using Unity.Netcode;
using UnityEngine;

namespace Counter
{
    using Interfaces;

    public class StoveCounter : BaseCounter, IHasProgress
    {
        public event EventHandler<IHasProgress.OnProgressChangeEventArgs> OnProgressChange;
        public event EventHandler<OnStateChangedEventArgs>                OnStateChanged;
        public class OnStateChangedEventArgs : EventArgs
        {
            public State StateChanged;
        }
        public enum State
        {
            Idle,
            Frying,
            Fried,
            Burned
        }
    
        [SerializeField] private FryingRecipeSo[]  fryingRecipeSoArray;
        [SerializeField] private BurningRecipeSo[] burningRecipeSoArray;

        private NetworkVariable<State>           state = new NetworkVariable<State>(State.Idle);
        private NetworkVariable<float>           fryingTimer = new NetworkVariable<float>(0f);
        private NetworkVariable<float>           burningTimer = new NetworkVariable<float>(0f);
        private FryingRecipeSo  fryingRecipeSo;
        private BurningRecipeSo burningRecipeSo;

        public override void OnNetworkSpawn()
        {
            this.fryingTimer.OnValueChanged += this.FryingTimer_OnValueChanged;
            this.burningTimer.OnValueChanged += this.BurningTimer_OnValueChanged;
            this.state.OnValueChanged += this.State_OnValueChanged;
        }

        private void BurningTimer_OnValueChanged(float previousvalue, float newvalue)
        {
            var burningTimerMax = this.burningRecipeSo != null ? this.burningRecipeSo.burningTimerMax : 1f;
            this.OnProgressChange?.Invoke(this, new IHasProgress.OnProgressChangeEventArgs()
            {
                ProgressNormalized = this.burningTimer.Value / burningTimerMax
            });
        }

        private void FryingTimer_OnValueChanged(float previousValue, float newValue)
        {
            var fryingTimerMax = this.fryingRecipeSo != null ? this.fryingRecipeSo.fryingTimerMax : 1f;
            this.OnProgressChange?.Invoke(this, new IHasProgress.OnProgressChangeEventArgs()
            {
                ProgressNormalized = this.fryingTimer.Value / fryingTimerMax
            });
        }

        private void State_OnValueChanged(State prevState, State newState)
        {
            this.OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
            {
                StateChanged = this.state.Value
            });

            if (this.state.Value is State.Burned or State.Idle)
            {
                this.OnProgressChange?.Invoke(this, new IHasProgress.OnProgressChangeEventArgs()
                {
                    ProgressNormalized = 0f
                });
            }
        }

        private void Update()
        {
            if (!this.IsServer) return;
            if (this.HasKitchenObject())
            {
                switch (this.state.Value)
                {
                    case State.Idle:
                        break;
                    case State.Frying:
                        this.fryingTimer.Value += Time.deltaTime;
                        
                        if (this.fryingTimer.Value > this.fryingRecipeSo.fryingTimerMax)
                        {
                            // Fried
                            KitchenObject.DestroyKitchenObject(this.GetKitchenObject());

                            KitchenObject.SpawnKitchenObject(this.fryingRecipeSo.output, this);

                            this.state.Value           = State.Fried;
                            this.burningTimer.Value    = 0f;

                            this.SetBurningRecipeSoClientRpc(KitchenGameMultiplayer.Instance.GetKitchenObjectSoIndex(this.GetKitchenObject().GetKitchenObjectSo()));
                        }
                        break;
                    case State.Fried:
                        this.burningTimer.Value += Time.deltaTime;
                        
                        if (this.burningTimer.Value > this.burningRecipeSo.burningTimerMax)
                        {
                            // Fried
                            KitchenObject.DestroyKitchenObject(this.GetKitchenObject());

                            KitchenObject.SpawnKitchenObject(this.burningRecipeSo.output, this);

                            this.state.Value = State.Burned;
                            
                        }
                        break;
                    case State.Burned:
                        break;
                }
            }
        }

        public override void Interact(Player player)
        {
            if (!this.HasKitchenObject())
            {
                // There is no kitchenObject here !!!
                if (player.HasKitchenObject())
                {
                    // Player is carrying something
                    if (this.HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSo()))
                    {
                        // Player is carrying sth that can be fried
                        var localKitchenObject = player.GetKitchenObject();
                        localKitchenObject.SetKitchenObjectParent(this);
                        
                        this.InteractLogicPlaceObjectOnCounterServerRpc(
                            KitchenGameMultiplayer.Instance.GetKitchenObjectSoIndex(localKitchenObject.GetKitchenObjectSo())
                            );
                    }
                }
                else
                {
                    // Player has nothing
                }
            }
            else
            {
                // There is a KO !
                if (player.HasKitchenObject())
                {
                    // Player is carrying
                    if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                    {
                        // Player is holding a plate
                        if (plateKitchenObject.TryAddIngredient(this.GetKitchenObject().GetKitchenObjectSo()))
                        {
                            this.GetKitchenObject().DestroySelf();
                            
                            this.state.Value = State.Idle;
                        }
                    }
                }
                else
                {
                    //Not carrying
                    this.GetKitchenObject().SetKitchenObjectParent(player);

                    this.SetStatIdleServerRpc();
                }
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void SetStatIdleServerRpc()
        {
            this.state.Value = State.Idle;
        }

        [ServerRpc(RequireOwnership = false)]
        private void InteractLogicPlaceObjectOnCounterServerRpc(int kitchenObjectSoIndex)
        {
            this.fryingTimer.Value = 0f;
            this.state.Value       = State.Frying;
            
            this.SetFryingRecipeSoClientRpc(kitchenObjectSoIndex);
        }
        
        [ClientRpc]
        private void SetFryingRecipeSoClientRpc(int kitchenObjectSoIndex)
        {
            var localKitchenObject = KitchenGameMultiplayer.Instance.GetKitchenObjectSoFromIndex(kitchenObjectSoIndex);
            this.fryingRecipeSo = this.GetFryingRecipeSoWithInput(localKitchenObject);
        }
        
        [ClientRpc]
        private void SetBurningRecipeSoClientRpc(int kitchenObjectSoIndex)
        {
            var localKitchenObject = KitchenGameMultiplayer.Instance.GetKitchenObjectSoFromIndex(kitchenObjectSoIndex);
            this.burningRecipeSo = this.GetBurningRecipeSoWithInput(localKitchenObject);
        }
        
        private bool HasRecipeWithInput(KitchenObjectSo inputKitchenObjectSo)
        {
            var fryingCs = this.GetFryingRecipeSoWithInput(inputKitchenObjectSo);
            return fryingCs != null;
        }

        private KitchenObjectSo GetOutputForInput(KitchenObjectSo inputKitchenObjectSo)
        {
            var fryingCs = this.GetFryingRecipeSoWithInput(inputKitchenObjectSo);
            if (fryingCs != null)
            {
                return fryingCs.output;
            }
            else
            {
                return null;
            }
        }

        private FryingRecipeSo GetFryingRecipeSoWithInput(KitchenObjectSo inputKitchenObjectSo)
        {
            foreach (var fryingCs in this.fryingRecipeSoArray)
            {
                if (fryingCs.input == inputKitchenObjectSo)
                {
                    return fryingCs;
                }
            }
            return null;
        }
    
        private BurningRecipeSo GetBurningRecipeSoWithInput(KitchenObjectSo inputKitchenObjectSo)
        {
            foreach (var burnCs in this.burningRecipeSoArray)
            {
                if (burnCs.input == inputKitchenObjectSo)
                {
                    return burnCs;
                }
            }
            return null;
        }

        public bool IsFried() => this.state.Value == State.Fried;
    }
}
