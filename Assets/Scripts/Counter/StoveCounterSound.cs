using UnityEngine;

namespace Counter
{
    public class StoveCounterSound : MonoBehaviour
    {
        [SerializeField] private StoveCounter stoveCounter;
        private                  AudioSource  audioSource;

        private void Awake()
        {
            this.audioSource = this.GetComponent<AudioSource>();
        }

        private void Start()
        {
            this.stoveCounter.OnStateChanged += this.StoveCounter_OnStateChanged;
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
