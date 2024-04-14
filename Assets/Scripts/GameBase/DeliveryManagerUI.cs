namespace GameBase
{
    using System;
    using UnityEngine;

    public class DeliveryManagerUI : MonoBehaviour
    {
        [SerializeField] private Transform container;
        [SerializeField] private Transform recipeTemplate;

        private void Awake() { this.recipeTemplate.gameObject.SetActive(false); }

        private void Start()
        {
            DeliveryManager.Instance.OnRecipeSpawn     += this.Instance_OnRecipeSpawn;
            DeliveryManager.Instance.OnRecipeCompleted += this.Instance_OnRecipeCompleted;
        }

        private void Instance_OnRecipeCompleted(object sender, EventArgs e)
        {
            this.UpdateVisual();
        }

        private void Instance_OnRecipeSpawn(object sender, EventArgs e)
        {
            this.UpdateVisual();
        }

        private void UpdateVisual()
        {
            foreach (Transform child in this.container)
            {
                if (child == this.recipeTemplate) continue;
                Destroy(child.gameObject);
            }

            foreach (var recipeSo in DeliveryManager.Instance.GetWaitingRecipeSoList())
            {
                var recipeTransform = Instantiate(this.recipeTemplate, this.container);
                recipeTransform.gameObject.SetActive(true);
            }
        }
    }
}