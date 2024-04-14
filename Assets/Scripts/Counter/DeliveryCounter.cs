namespace Counter
{
    using GameBase;

    public class DeliveryCounter : BaseCounter
    {
        public override void Interact(Player player)
        {
            if (player.HasKitchenObject())
            {
                if (player.GetKitchenObject().TryGetPlate(out var plateKitchenObject))
                {
                    // Only accepts Plates
                    
                    DeliveryManager.Instance.DeliverRecipe(plateKitchenObject);
                    
                    player.GetKitchenObject().DestroySelf();
                }
            }
        }
    }
}