using UnityEngine;

namespace Counter.Visual
{
    public class StoveCounterVisual : MonoBehaviour
    {
        [SerializeField] private StoveCounter stoveCounter;
        [SerializeField] private GameObject   stoveOnGameObject;
        [SerializeField] private GameObject   particlesGameObject;

        private void Start() { this.stoveCounter.OnStateChanged += this.StoveCounter_OnStateChanged; }

        private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
        {
            var showVisual = e.StateChanged is StoveCounter.State.Frying or StoveCounter.State.Fried;
            this.stoveOnGameObject.SetActive(showVisual);
            this.particlesGameObject.SetActive(showVisual);
        }
    }
}