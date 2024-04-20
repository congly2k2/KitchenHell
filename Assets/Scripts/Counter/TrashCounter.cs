using System;
using GameBase;

namespace Counter
{
    public class TrashCounter : BaseCounter
    {
        public static event EventHandler OnAnyObjectTrashed;
        public override void Interact(Player player)
        {
            if (!player.HasKitchenObject()) return;

            player.GetKitchenObject().DestroySelf();

            OnAnyObjectTrashed?.Invoke(this, EventArgs.Empty);
        }
    }
}