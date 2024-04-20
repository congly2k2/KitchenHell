namespace UI
{
    using GameBase;
    using UnityEngine;

    public class PlateIconUI : MonoBehaviour
    {
        [SerializeField] private PlateKitchenObject plateKitchenObject;
        [SerializeField] private Transform          iconTemplate;

        private void Awake()
        {
            this.iconTemplate.gameObject.SetActive(false);
        }

        private void Start()
        {
            this.plateKitchenObject.OnIngredientAdded += this.PlateKitchenObject_OnIngredientAdded;
        }

        private void PlateKitchenObject_OnIngredientAdded(object sender,
            PlateKitchenObject.OnIngredientAddedEventArgs e)
        {
            this.UpdateVisual();
        }
        
        private void UpdateVisual() {
            foreach (Transform child in this.transform)
            {
                if (child == this.iconTemplate) continue;
                Destroy(child.gameObject);
            }
            foreach (var kitchenObjectSo in this.plateKitchenObject.GetKitchenObjectSoList())
            {
                var iconTransform = Instantiate(this.iconTemplate, this.transform);
                iconTransform.gameObject.SetActive(true);
                iconTransform.GetComponent<PlateIconSingleUI>().SetKitchenObjectSo(kitchenObjectSo);
            }
        }
    }
}
