using SyncNetwork;
using Unity.Netcode;

namespace GameBase
{
    using System;
    using System.Collections.Generic;
    using RecipeSO;
    using UnityEngine;

    public class PlateKitchenObject : KitchenObject
    {
        public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
        public class  OnIngredientAddedEventArgs : EventArgs
        {
            public KitchenObjectSo KitchenObjectSo;
            
        }
        [SerializeField] private List<KitchenObjectSo> validKitchenObjectSoList;
        
        private List<KitchenObjectSo> kitchenObjectSoList;

        protected override void Awake()
        {
            base.Awake();
            this.kitchenObjectSoList = new List<KitchenObjectSo>();
        }

        public bool TryAddIngredient(KitchenObjectSo kitchenObjectSo)
        {
            if (!this.validKitchenObjectSoList.Contains(kitchenObjectSo))
            {
                // Not a valid ingredient
                return false;
            }
            if (this.kitchenObjectSoList.Contains(kitchenObjectSo))
            {
                // Already has this type
                return false;
            }
            else
            {
                this.AddIngredientServerRpc(KitchenGameMultiplayer.Instance.GetKitchenObjectSoIndex(kitchenObjectSo));
                return true;
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void AddIngredientServerRpc(int kitchenObjectSoIndex)
        {
            this.AddIngredientClientRpc(kitchenObjectSoIndex);
        }
        
        [ClientRpc]
        private void AddIngredientClientRpc(int index)
        {
            var kitchenObjectSo = KitchenGameMultiplayer.Instance.GetKitchenObjectSoFromIndex(index);
            this.kitchenObjectSoList.Add(kitchenObjectSo);
                
            this.OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs
            {
                KitchenObjectSo = kitchenObjectSo
            });
        }

        public List<KitchenObjectSo> GetKitchenObjectSoList() => this.kitchenObjectSoList;
    }
}