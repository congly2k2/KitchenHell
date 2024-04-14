namespace GameBase
{
    using RecipeSO;
    using UnityEngine;
    using UnityEngine.UI;

    public class PlateIconSingleUI : MonoBehaviour
    {
        [SerializeField] private Image image;
        public void SetKitchenObjectSo(KitchenObjectSo kitchenObjectSo)
        {
            this.image.sprite = kitchenObjectSo.sprite;
        }
    }
}