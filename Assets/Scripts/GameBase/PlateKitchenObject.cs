namespace GameBase
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class PlateKitchenObject : KitchenObject
    {
        [SerializeField] private List<KitchenObjectSo> validKitchenObjectSoList;
        
        private List<KitchenObjectSo> kitchenObjectSoList;

        private void Awake()
        {
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
                this.kitchenObjectSoList.Add(kitchenObjectSo);
                return true;
            }
        }
    }
}