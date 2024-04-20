using System;
using GameBase;

namespace Counter
{
    public class DeliveryCounter : BaseCounter
    {
        public static DeliveryCounter Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

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