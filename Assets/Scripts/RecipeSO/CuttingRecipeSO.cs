using UnityEngine;

namespace RecipeSO
{
    [CreateAssetMenu()]
    public class CuttingRecipeSo : ScriptableObject
    {
        public KitchenObjectSo input;
        public KitchenObjectSo output;
        public int             cuttingProgressMax;
    }
}
