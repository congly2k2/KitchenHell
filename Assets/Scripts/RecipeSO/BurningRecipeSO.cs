using UnityEngine;

namespace RecipeSO
{
    [CreateAssetMenu()]
    public class BurningRecipeSo : ScriptableObject
    {
        public KitchenObjectSo input;
        public KitchenObjectSo output;
        public float           burningTimerMax;
    }
}