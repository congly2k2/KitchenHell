using UnityEngine;

namespace RecipeSO
{
    [CreateAssetMenu()]
    public class AudioClipRefsSo : ScriptableObject
    {
        public AudioClip[] chop;
        public AudioClip[] deliveryFailed;
        public AudioClip[] deliverySuccess;
        public AudioClip[] footStep;
        public AudioClip[] objectDrop;
        public AudioClip[] objectPickup;
        public AudioClip[] trash;
        public AudioClip[] warning;
        public AudioClip stoveSizzle;
    }
}