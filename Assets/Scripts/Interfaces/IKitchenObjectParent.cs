using GameBase;
using Unity.Netcode;
using UnityEngine;

namespace Interfaces
{
    public interface IKitchenObjectParent
    {
        public Transform GetKitchenObjectFollowTransform();


        public void SetKitchenObject(KitchenObject e);


        public KitchenObject GetKitchenObject();


        public void ClearKitchenObject();


        public bool HasKitchenObject();

        public NetworkObject GetNetworkObject();

    }
}
