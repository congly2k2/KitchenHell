using Interfaces;
using RecipeSO;
using System;
using GameBase;
using Unity.Netcode;
using UnityEngine;

namespace Counter
{
    public class CuttingCounter : BaseCounter, IHasProgress
    {
        public static event EventHandler                                         OnAnyCut;

        public new static void ResetStaticData()
        {
            CuttingCounter.OnAnyCut = null;
        }
        public event        EventHandler<IHasProgress.OnProgressChangeEventArgs> OnProgressChange;

        public event EventHandler OnCut;
    
        [SerializeField] private CuttingRecipeSo[] cutKitchenObjectSoArray;

        private int cuttingProgress;
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
                        // Player is carrying sth that can cut
                        var localKitchenObject = player.GetKitchenObject();
                        localKitchenObject.SetKitchenObjectParent(this);
                        
                        this.InteractLogicPlaceObjectOnCounterServerRpc();
                    }
                }
                else
                {
                    // Player has nothing
                }
            }
            else
            {
                // There is a KitchenObject here
                if (player.HasKitchenObject())
                {
                    // Player is carrying something
                    if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                    {
                        // Player is holding a plate
                        if (plateKitchenObject.TryAddIngredient(this.GetKitchenObject().GetKitchenObjectSo()))
                        {
                            this.GetKitchenObject().DestroySelf();
                        }
                    }
                }
                else
                {
                    // Player is not carrying anything
                    this.GetKitchenObject().SetKitchenObjectParent(player);
                }
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void InteractLogicPlaceObjectOnCounterServerRpc()
        {
            this.InteractLogicPlaceObjectOnCounterClientRpc();
        }
        
        [ClientRpc]
        private void InteractLogicPlaceObjectOnCounterClientRpc()
        {
            this.cuttingProgress = 0;
                    
            this.OnProgressChange?.Invoke(this, new IHasProgress.OnProgressChangeEventArgs
            {
                ProgressNormalized = 0f
            });
        }

        public override void InteractAlternate(Player player)
        {
            if (this.HasKitchenObject() && this.HasRecipeWithInput(this.GetKitchenObject().GetKitchenObjectSo()))
            {
                // There is a Kitchen object & it can be cut
                this.CutObjectServerRpc();
                this.TestCuttingProgressDoneServerRpc();
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void CutObjectServerRpc()
        {
            this.CutObjectClientRpc();
        }
        
        [ClientRpc]
        private void CutObjectClientRpc()
        {
            this.cuttingProgress++;
            
            this.OnCut?.Invoke(this, EventArgs.Empty);
            OnAnyCut?.Invoke(this, EventArgs.Empty);

            var cuttingRecipeSo = this.GetCuttingRecipeSoWithInput(this.GetKitchenObject().GetKitchenObjectSo());
            
            this.OnProgressChange?.Invoke(this, new IHasProgress.OnProgressChangeEventArgs
            {
                ProgressNormalized = (float)this.cuttingProgress / cuttingRecipeSo.cuttingProgressMax
            });
            
            
        }

        [ServerRpc(RequireOwnership = false)]
        private void TestCuttingProgressDoneServerRpc()
        {
            var cuttingRecipeSo = this.GetCuttingRecipeSoWithInput(this.GetKitchenObject().GetKitchenObjectSo());
            if (this.cuttingProgress >= cuttingRecipeSo.cuttingProgressMax)
            {
                var outputKitchenObjectSo = this.GetOutputForInput(this.GetKitchenObject().GetKitchenObjectSo());
                
                KitchenObject.DestroyKitchenObject(this.GetKitchenObject());

                KitchenObject.SpawnKitchenObject(outputKitchenObjectSo, this);
            }
        }

        private bool HasRecipeWithInput(KitchenObjectSo inputKitchenObjectSo)
        {
            var cuttingRecipeSo = this.GetCuttingRecipeSoWithInput(inputKitchenObjectSo);
            return cuttingRecipeSo != null;
        }

        private KitchenObjectSo GetOutputForInput(KitchenObjectSo inputKitchenObjectSo)
        {
            var cuttingRecipeSo = this.GetCuttingRecipeSoWithInput(inputKitchenObjectSo);
            if (cuttingRecipeSo != null)
            {
                return cuttingRecipeSo.output;
            }
            else
            {
                return null;
            }
        }

        private CuttingRecipeSo GetCuttingRecipeSoWithInput(KitchenObjectSo inputKitchenObjectSo)
        {
            foreach (var cuttingRecipeSo in this.cutKitchenObjectSoArray)
            {
                if (cuttingRecipeSo.input == inputKitchenObjectSo)
                {
                    return cuttingRecipeSo;
                }
            }
            return null;
        }
    }
}
