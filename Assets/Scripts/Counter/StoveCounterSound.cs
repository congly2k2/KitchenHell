using System;
using GameBase;
using Interfaces;
using UnityEngine;

namespace Counter
{
    public class StoveCounterSound : MonoBehaviour
    {
        [SerializeField] private StoveCounter stoveCounter;
        private                  AudioSource  audioSource;
        private float warningSoundTimer;
        private bool isPlayingWarningSound;

        private void Awake()
        {
            this.audioSource = this.GetComponent<AudioSource>();
        }

        private void Start()
        {
            this.stoveCounter.OnStateChanged += this.StoveCounter_OnStateChanged;
            this.stoveCounter.OnProgressChange += StoveCounter_OnProgressChange;
        }

        private void Update()
        {
            if (!this.isPlayingWarningSound) return;
            this.warningSoundTimer -= Time.deltaTime;
            
            if (this.warningSoundTimer > 0) return;
            
            const float warningSoundTimerMax = .2f;
            this.warningSoundTimer = warningSoundTimerMax;
            
            SoundManager.Instance.PlayWarningSound(this.stoveCounter.transform.position);
        }

        private void StoveCounter_OnProgressChange(object sender, IHasProgress.OnProgressChangeEventArgs e)
        {
            const float burnShowProgressAmount = .5f;
            this.isPlayingWarningSound = this.stoveCounter.IsFried() && e.ProgressNormalized >= burnShowProgressAmount;
        }

        private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
        {
            var playSound = e.StateChanged is StoveCounter.State.Frying or StoveCounter.State.Fried;

            if (playSound)
            {
                this.audioSource.Play();
            }
            else
            {
                this.audioSource.Pause();
            }
        }
    }
}
