using UnityEngine;

namespace Counter.Visual
{
    using System;
    using System.Collections.Generic;
    using GameBase;

    public class PlateCompleteVisual : MonoBehaviour
    {
        // ReSharper disable once InconsistentNaming
        [Serializable]
        public struct KitchenObjectSO_GameObject
        {
            public KitchenObjectSo kitchenObjectSo;
            public GameObject      gameObject;
        }

        [SerializeField] private PlateKitchenObject               plateKitchenObject;
        [SerializeField] private List<KitchenObjectSO_GameObject> kitchenObjectSoGameObjectList;

        private void Start()
        {
            this.plateKitchenObject.OnIngredientAdded += this.PlateKitchenObject_OnIngredientAdded;
            foreach (var kitchenObjectSoGameObject in this.kitchenObjectSoGameObjectList)
            {
                kitchenObjectSoGameObject.gameObject.SetActive(false);
            }
        }

        private void PlateKitchenObject_OnIngredientAdded(object sender,
            PlateKitchenObject.OnIngredientAddedEventArgs e)
        {
            foreach (var kitchenObjectSoGameObject in this.kitchenObjectSoGameObjectList)
            {
                if (kitchenObjectSoGameObject.kitchenObjectSo == e.KitchenObjectSo)
                {
                    kitchenObjectSoGameObject.gameObject.SetActive(true);
                }
            }
            // e.KitchenObjectSo
        }
    }
}