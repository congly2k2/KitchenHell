namespace Counter.Visual
{
    using System;
    using UnityEngine;

    public class PlatesCounterVisual : MonoBehaviour
    {
        [SerializeField] private PlatesCounter platesCounter;
        [SerializeField] private Transform     counterTopPoint;
        [SerializeField] private Transform     platesVisualPrefab;

        private void Start()
        {
            this.platesCounter.OnPlateSpawned += this.PlatesCounter_OnPlateSpawned;
        }

        private void PlatesCounter_OnPlateSpawned(object sender, EventArgs e)
        {
            var plateVisualTransform = Instantiate(this.platesVisualPrefab, this.counterTopPoint);
        }
    }
}