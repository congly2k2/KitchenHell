using System;
using UnityEngine;

namespace GameBase
{
    public class MusicManager : MonoBehaviour
    {
        private const string PlayerPrefsMusicVolume = "MusicVolume";
        public static MusicManager Instance { get; private set; }
        private AudioSource audioSource;
        private float volume = .3f;

        private void Awake()
        {
            Instance = this;
            this.audioSource = this.GetComponent<AudioSource>();
            this.volume = PlayerPrefs.GetFloat(MusicManager.PlayerPrefsMusicVolume, .3f);
            this.audioSource.volume = this.volume;
        }

        public void ChangeVolume()
        {
            this.volume += .1f;
            if (this.volume > 1f)
            {
                this.volume = 0f;
            }

            this.audioSource.volume = this.volume;

            PlayerPrefs.SetFloat(MusicManager.PlayerPrefsMusicVolume, this.volume);
            PlayerPrefs.Save();
        }

        public float GetVolume() => this.volume;
    }
}