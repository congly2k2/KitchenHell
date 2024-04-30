using System;
using GameBase;
using Unity.Netcode;

namespace Counter
{
    public class TrashCounter : BaseCounter
    {
        public static event EventHandler OnAnyObjectTrashed;
        
        public new static void ResetStaticData()
        {
            TrashCounter.OnAnyObjectTrashed = null;
        }
        public override void Interact(Player player)
        {
            if (!player.HasKitchenObject()) return;

            KitchenObject.DestroyKitchenObject(player.GetKitchenObject());
            
            this.InteractLogicServerRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        private void InteractLogicServerRpc()
        {
            this.InteractLogicClientRpc();
        }

        [ClientRpc]
        private void InteractLogicClientRpc()
        {
            OnAnyObjectTrashed?.Invoke(this, EventArgs.Empty);
        }
    }
}