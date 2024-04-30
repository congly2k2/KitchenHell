using System;
using GameBase;
using Interfaces;
using RecipeSO;
using Unity.Netcode;
using UnityEngine;

namespace SyncNetwork
{
    public class KitchenGameMultiplayer : NetworkBehaviour
    {
        public static KitchenGameMultiplayer Instance { get; private set; }

        [SerializeField] private KitchenObjectListSo kitchenObjectListSo;

        private void Awake()
        {
            Instance = this;
        }
        
        public void SpawnKitchenObject(KitchenObjectSo kitchenObjectSo, IKitchenObjectParent kitchenObjectParent)
        {
            this.SpawnKitchenObjectServerRpc(this.GetKitchenObjectSoIndex(kitchenObjectSo), kitchenObjectParent.GetNetworkObject());
        }

        [ServerRpc(RequireOwnership = false)]
        private void SpawnKitchenObjectServerRpc(int kitchenObjectSoIndex, NetworkObjectReference kitchenObjectParentReference)
        {
            var kitchenObjectSo = this.GetKitchenObjectSoFromIndex(kitchenObjectSoIndex);
            var kitchenObjectTransform = Instantiate(kitchenObjectSo.prefab);
            
            var kitchenNetworkObject = kitchenObjectTransform.GetComponent<NetworkObject>();
            kitchenNetworkObject.Spawn(true);
            
            var kitchenObject          = kitchenObjectTransform.GetComponent<KitchenObject>();

            kitchenObjectParentReference.TryGet(out NetworkObject kitchenObjectParentNetworkObject);
            var kitchenObjectParent = kitchenObjectParentNetworkObject.GetComponent<IKitchenObjectParent>();
            
            kitchenObject.SetKitchenObjectParent(kitchenObjectParent);
        }

        private int GetKitchenObjectSoIndex(KitchenObjectSo kitchenObjectSo) =>
            this.kitchenObjectListSo.kitchenObjectSoList.IndexOf(kitchenObjectSo);
        
        private KitchenObjectSo GetKitchenObjectSoFromIndex(int kitchenObjectSoIndex) =>
            this.kitchenObjectListSo.kitchenObjectSoList[kitchenObjectSoIndex];
    }
}