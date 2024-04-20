namespace UI
{
    using System;
    using RecipeSO;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class DeliveryManagerSingleUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI recipeNameText;
        [SerializeField] private Transform       iconContainer;
        [SerializeField] private Transform       iconTemplate;

        private void Awake()
        {
            this.iconTemplate.gameObject.SetActive(false);
        }

        public void SetRecipeSo(RecipeSo recipeSo)
        {
            this.recipeNameText.text = recipeSo.recipeName;

            foreach (Transform child in this.iconContainer)
            {
                if (child == this.iconTemplate) continue;
                Destroy(child.gameObject);
            }

            foreach (var kitchenObjectSo in recipeSo.kitchenObjectSoList)
            {
                var iconTransform = Instantiate(this.iconTemplate, this.iconContainer);
                iconTransform.gameObject.SetActive(true);
                iconTransform.GetComponent<Image>().sprite = kitchenObjectSo.sprite;
            }
        }
    }
}