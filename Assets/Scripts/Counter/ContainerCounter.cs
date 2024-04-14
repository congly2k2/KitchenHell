using System;
using GameBase;
using UnityEngine;

namespace Counter
{
    using RecipeSO;

    public class ContainerCounter : BaseCounter
    {
        public event EventHandler OnPlayerGrabbedObject;
    
        [SerializeField] private KitchenObjectSo kitchenObjectSo;

        public override void Interact(Player player)
        {
            if (!player.HasKitchenObject())
            {
                // Player is not carrying anything
                KitchenObject.SpawnKitchenObject(this.kitchenObjectSo, player);
                this.OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
            }
        
        }
    }
}