using UnityEngine;

namespace Counter
{
    using GameBase;
    using Interfaces;
    using RecipeSO;

    public class ClearCounter : BaseCounter, IKitchenObjectParent
    {
        [SerializeField] private KitchenObjectSo kitchenObjectSo;

        public override void Interact(Player player)
        {
            if (!this.HasKitchenObject())
            {
                // There is no kitchenObject here !!!
                if (player.HasKitchenObject())
                {
                    // Player is carrying something
                    player.GetKitchenObject().SetKitchenObjectParent(this);
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
                            KitchenObject.DestroyKitchenObject(this.GetKitchenObject());
                        }
                    }
                    else
                    {
                        // Player is not carrying Plate but something else
                        if (this.GetKitchenObject().TryGetPlate(out plateKitchenObject))
                        {
                            // Counter is holding a plate
                            if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSo()))
                            {
                                KitchenObject.DestroyKitchenObject(player.GetKitchenObject());
                            }

                            
                        }
                    }
                }
                else
                {
                    //Not carrying
                    this.GetKitchenObject().SetKitchenObjectParent(player);
                }
            }
        }
    }
}