using UnityEngine;
using System;
using Counter;
using RecipeSO;
using Random = UnityEngine.Random;

namespace GameBase
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance { get; private set; }
        
        [SerializeField] private AudioClipRefsSo audioClipRefsSo;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            DeliveryManager.Instance.OnRecipeSuccess += this.DeliveryManager_OnRecipeSuccess;
            DeliveryManager.Instance.OnRecipeFailed  += this.DeliveryManager_OnRecipeFailed;
            CuttingCounter.OnAnyCut                  += this.CuttingCounter_OnAnyCut;
            Player.Instance.OnPickedSomething        += this.Player_OnPickedSomething;
            BaseCounter.OnAnyObjectPlacedHere        += this.BaseCounter_OnAnyObjectPlacedHere;
            TrashCounter.OnAnyObjectTrashed          += this.TrashCounter_OnAnyObjectTrashed;
        }

        private void TrashCounter_OnAnyObjectTrashed(object sender, EventArgs e)
        {
            var trashCounter = sender as TrashCounter;
            if (trashCounter != null) this.PlaySound(this.audioClipRefsSo.trash, trashCounter.transform.position);
        }

        private void BaseCounter_OnAnyObjectPlacedHere(object sender, EventArgs e)
        {
            var baseCounter = sender as BaseCounter;
            if (baseCounter != null) this.PlaySound(this.audioClipRefsSo.objectDrop, baseCounter.transform.position);
        }

        private void Player_OnPickedSomething(object sender, EventArgs e)
        {
            this.PlaySound(this.audioClipRefsSo.objectPickup, Player.Instance.transform.position);
        }

        private void CuttingCounter_OnAnyCut(object sender, EventArgs e)
        {
            var cuttingCounter = sender as CuttingCounter;
            if (cuttingCounter != null) this.PlaySound(this.audioClipRefsSo.chop, cuttingCounter.transform.position);
        }

        private void DeliveryManager_OnRecipeFailed(object sender, EventArgs e)
        {
            var deliveryCounter = DeliveryCounter.Instance;
            if (Camera.main != null) this.PlaySound(this.audioClipRefsSo.deliveryFailed, deliveryCounter.transform.position);
        }

        private void DeliveryManager_OnRecipeSuccess(object sender, EventArgs e)
        {
            var deliveryCounter = DeliveryCounter.Instance;
            if (Camera.main != null) this.PlaySound(this.audioClipRefsSo.deliverySuccess, deliveryCounter.transform.position);
        }

        private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volume = 1f)
        {
            this.PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)], position, volume);
        }
        
        private void PlaySound(AudioClip audioClip, Vector3 position, float volume = 1f)
        {
            AudioSource.PlayClipAtPoint(audioClip, position, volume);
        }

        public void PlayFootStepSound(Vector3 position, float volume)
        {
            this.PlaySound(this.audioClipRefsSo.footStep, position, volume);
        }
    }
}