
using UnityEngine;

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
            }
            else
            {
                //Not carrying
                this.GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }
}