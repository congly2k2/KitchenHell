using System;
using UnityEngine;

public class CuttingCounter : BaseCounter
{
    public event EventHandler<OnProgressChangeEventArgs> OnProgressChange;
    public class OnProgressChangeEventArgs : EventArgs
    {
        public float ProgressNormalized;
    }

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
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSo()))
                {
                    // Player is carrying sth that can cut
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    this.cuttingProgress = 0;
                    
                    var cuttingRecipeSo = GetCuttingRecipeSoWithInput(this.GetKitchenObject().GetKitchenObjectSo());
                    
                    OnProgressChange?.Invoke(this, new OnProgressChangeEventArgs
                    {
                        ProgressNormalized = (float)this.cuttingProgress / cuttingRecipeSo.cuttingProgressMax
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
            }
            else
            {
                //Not carrying
                this.GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

    public override void InteractAlternate(Player player)
    {
        if (this.HasKitchenObject() && HasRecipeWithInput(this.GetKitchenObject().GetKitchenObjectSo()))
        {
            // There is a Kitchen object & it can be cut
            this.cuttingProgress++;
            
            OnCut?.Invoke(this, EventArgs.Empty);

            var cuttingRecipeSo       = GetCuttingRecipeSoWithInput(this.GetKitchenObject().GetKitchenObjectSo());
            
            OnProgressChange?.Invoke(this, new OnProgressChangeEventArgs
            {
                ProgressNormalized = (float)this.cuttingProgress / cuttingRecipeSo.cuttingProgressMax
            });
            
            if (this.cuttingProgress >= cuttingRecipeSo.cuttingProgressMax)
            {
                var outputKitchenObjectSo = GetOutputForInput(this.GetKitchenObject().GetKitchenObjectSo());
                this.GetKitchenObject().DestroySelf();

                KitchenObject.SpawnKitchenObject(outputKitchenObjectSo, this);
            }
        }
    }

    private bool HasRecipeWithInput(KitchenObjectSo inputKitchenObjectSo)
    {
        var cuttingRecipeSo = GetCuttingRecipeSoWithInput(inputKitchenObjectSo);
        return cuttingRecipeSo != null;
    }

    private KitchenObjectSo GetOutputForInput(KitchenObjectSo inputKitchenObjectSo)
    {
        var cuttingRecipeSo = GetCuttingRecipeSoWithInput(inputKitchenObjectSo);
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
