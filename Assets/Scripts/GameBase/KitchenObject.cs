using System;
using SyncNetwork;
using Unity.Netcode;
using UnityEngine;

namespace GameBase
{
    using Interfaces;
    using RecipeSO;

    public class KitchenObject : NetworkBehaviour
    {
        [SerializeField] private KitchenObjectSo kitchenObjectSo;

        private IKitchenObjectParent kitchenObjectParent;
        private FollowTransform followTransform;

        protected virtual void Awake()
        {
            this.followTransform = this.GetComponent<FollowTransform>();
        }

        public KitchenObjectSo GetKitchenObjectSo() { return this.kitchenObjectSo; }

        public void SetKitchenObjectParent(IKitchenObjectParent e)
        {
            this.SetKitchenObjectParentServerRpc(e.GetNetworkObject());
        }

        [ServerRpc(RequireOwnership = false)]
        private void SetKitchenObjectParentServerRpc(NetworkObjectReference kitchenObjectParentReference)
        {
            this.SetKitchenObjectParentClientRpc(kitchenObjectParentReference);
        }

        [ClientRpc]
        private void SetKitchenObjectParentClientRpc(NetworkObjectReference kitchenObjectParentReference)
        {
            kitchenObjectParentReference.TryGet(out NetworkObject kitchenObjectParentNetworkObject);
            var kitchenObjectParentNetwork = kitchenObjectParentNetworkObject.GetComponent<IKitchenObjectParent>();
            
            if (this.kitchenObjectParent != null)
            {
                this.kitchenObjectParent.ClearKitchenObject();
            }

            this.kitchenObjectParent = kitchenObjectParentNetwork;

            if (kitchenObjectParentNetwork.HasKitchenObject())
            {
                Debug.LogError("IKitchenObjectParent already has a KitchenObject");
            }

            kitchenObjectParentNetwork.SetKitchenObject(this);

            this.followTransform.SetTargetTransform(this.kitchenObjectParent.GetKitchenObjectFollowTransform());
        }

        public IKitchenObjectParent GetKitchenObjectParent() { return this.kitchenObjectParent; }

        public void DestroySelf()
        {
            this.ClearKitchenObjectOnParent();

            Destroy(this.gameObject);
        }

        public void ClearKitchenObjectOnParent()
        {
            this.kitchenObjectParent.ClearKitchenObject();
        }

        public bool TryGetPlate(out PlateKitchenObject plateKitchenObject)
        {
            if (this is PlateKitchenObject)
            {
                plateKitchenObject = this as PlateKitchenObject;
                return true;
            }

            plateKitchenObject = null;
            return false;
        }

        public static void SpawnKitchenObject(KitchenObjectSo kitchenObjectSo, IKitchenObjectParent kitchenObjectParent)
        {
            KitchenGameMultiplayer.Instance.SpawnKitchenObject(kitchenObjectSo, kitchenObjectParent);
        }

        public static void DestroyKitchenObject(KitchenObject kitchenObject)
        {
            KitchenGameMultiplayer.Instance.DestroyKitchenObject(kitchenObject);
        }
    }
}