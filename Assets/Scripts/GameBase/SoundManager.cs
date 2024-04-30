using UnityEngine;
using System;
using Counter;
using RecipeSO;
using Random = UnityEngine.Random;

namespace GameBase
{
    public class SoundManager : MonoBehaviour
    {
        private const string PlayerPrefsSoundVolume = "SoundVolume";
        public static SoundManager Instance { get; private set; }
        
        [SerializeField] private AudioClipRefsSo audioClipRefsSo;

        private float volume = 1f;

        private void Awake()
        {
            Instance = this;

            this.volume = PlayerPrefs.GetFloat(SoundManager.PlayerPrefsSoundVolume, 1f);
        }

        private void Start()
        {
            DeliveryManager.Instance.OnRecipeSuccess += this.DeliveryManager_OnRecipeSuccess;
            DeliveryManager.Instance.OnRecipeFailed  += this.DeliveryManager_OnRecipeFailed;
            CuttingCounter.OnAnyCut                  += this.CuttingCounter_OnAnyCut;
            Player.OnAnyPickedSomething        += this.Player_OnPickedSomething;
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
            var player = sender as Player;
            if (player != null) this.PlaySound(this.audioClipRefsSo.objectPickup, player.transform.position);
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

        private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volumeParam = 1f)
        {
            this.PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)], position, volumeParam);
        }
        
        private void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1f)
        {
            AudioSource.PlayClipAtPoint(audioClip, position, volumeMultiplier);
        }

        public void PlayFootStepSound(Vector3 position, float volumeParam)
        {
            this.PlaySound(this.audioClipRefsSo.footStep, position, volumeParam);
        }

        public void PlayCountDownSound()
        {
            this.PlaySound(this.audioClipRefsSo.warning, Vector3.zero);
        }
        public void PlayWarningSound(Vector3 position)
        {
            this.PlaySound(this.audioClipRefsSo.warning, position);
        }

        public void ChangeVolume()
        {
            this.volume += .1f;
            if (this.volume > 1f)
            {
                this.volume = 0f;
            }

            PlayerPrefs.SetFloat(SoundManager.PlayerPrefsSoundVolume, this.volume);
            PlayerPrefs.Save();
        }

        public float GetVolume() => this.volume;
    }
}