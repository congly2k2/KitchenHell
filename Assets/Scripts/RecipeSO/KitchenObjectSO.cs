using UnityEngine;

namespace RecipeSO
{
    [CreateAssetMenu()]
    public class KitchenObjectSo : ScriptableObject
    {
        public Transform prefab;
        public Sprite    sprite;
        public string    objectName;
    }
}
