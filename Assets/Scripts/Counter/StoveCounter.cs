using System;
using GameBase;
using RecipeSO;
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

        private State           state;
        private float           fryingTimer;
        private float           burningTimer;
        private FryingRecipeSo  fryingRecipeSo;
        private BurningRecipeSo burningRecipeSo;

        private void Start()
        {
            this.state = State.Idle;
        }

        private void Update()
        {
            if (this.HasKitchenObject())
            {
                switch (this.state)
                {
                    case State.Idle:
                        break;
                    case State.Frying:
                        this.fryingTimer += Time.deltaTime;
                        
                        this.OnProgressChange?.Invoke(this, new IHasProgress.OnProgressChangeEventArgs()
                        {
                            ProgressNormalized = this.fryingTimer / this.fryingRecipeSo.fryingTimerMax
                        });
                        
                        if (this.fryingTimer > this.fryingRecipeSo.fryingTimerMax)
                        {
                            // Fried
                            this.GetKitchenObject().DestroySelf();

                            KitchenObject.SpawnKitchenObject(this.fryingRecipeSo.output, this);

                            this.state           = State.Fried;
                            this.burningTimer    = 0f;
                            this.burningRecipeSo = this.GetBurningRecipeSoWithInput(this.GetKitchenObject().GetKitchenObjectSo());
                        
                            this.OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                            {
                                StateChanged = this.state
                            });
                        }
                        break;
                    case State.Fried:
                        this.burningTimer += Time.deltaTime;
                        
                        this.OnProgressChange?.Invoke(this, new IHasProgress.OnProgressChangeEventArgs()
                        {
                            ProgressNormalized = this.burningTimer / this.burningRecipeSo.burningTimerMax
                        });
                        
                        if (this.burningTimer > this.burningRecipeSo.burningTimerMax)
                        {
                            // Fried
                            this.GetKitchenObject().DestroySelf();

                            KitchenObject.SpawnKitchenObject(this.burningRecipeSo.output, this);

                            this.state = State.Burned;
                        
                            this.OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                            {
                                StateChanged = this.state
                            });
                            
                            this.OnProgressChange?.Invoke(this, new IHasProgress.OnProgressChangeEventArgs()
                            {
                                ProgressNormalized = 0f
                            });
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
                        player.GetKitchenObject().SetKitchenObjectParent(this);
                    
                        this.fryingRecipeSo = this.GetFryingRecipeSoWithInput(this.GetKitchenObject().GetKitchenObjectSo());

                        this.state       = State.Frying;
                        this.fryingTimer = 0f;
                    
                        this.OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            StateChanged = this.state
                        });
                        
                        this.OnProgressChange?.Invoke(this, new IHasProgress.OnProgressChangeEventArgs()
                        {
                            ProgressNormalized = this.fryingTimer / this.fryingRecipeSo.fryingTimerMax
                        });
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
                            
                            this.state = State.Idle;
                
                            this.OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                            {
                                StateChanged = this.state
                            });
                    
                            this.OnProgressChange?.Invoke(this, new IHasProgress.OnProgressChangeEventArgs()
                            {
                                ProgressNormalized = 0f
                            });
                        }
                    }
                }
                else
                {
                    //Not carrying
                    this.GetKitchenObject().SetKitchenObjectParent(player);

                    this.state = State.Idle;
                
                    this.OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                    {
                        StateChanged = this.state
                    });
                    
                    this.OnProgressChange?.Invoke(this, new IHasProgress.OnProgressChangeEventArgs()
                    {
                        ProgressNormalized = 0f
                    });
                }
            }
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

        public bool IsFried() => this.state == State.Fried;
    }
}
