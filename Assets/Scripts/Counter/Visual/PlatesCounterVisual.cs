namespace Counter.Visual
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class PlatesCounterVisual : MonoBehaviour
    {
        [SerializeField] private PlatesCounter platesCounter;
        [SerializeField] private Transform     counterTopPoint;
        [SerializeField] private Transform     platesVisualPrefab;

        private List<GameObject> plateVisualGameObjectList;

        private void Awake() { this.plateVisualGameObjectList = new List<GameObject>(); }

        private void Start()
        {
            this.platesCounter.OnPlateSpawned += this.PlatesCounter_OnPlateSpawned;
            this.platesCounter.OnPlateRemoved += this.PlatesCounter_OnPlateRemoved;
        }

        private void PlatesCounter_OnPlateRemoved(object sender, EventArgs e)
        {
            var plateGameObject = this.plateVisualGameObjectList[^1];
            this.plateVisualGameObjectList.Remove(plateGameObject);
            Destroy(plateGameObject);
        }

        private void PlatesCounter_OnPlateSpawned(object sender, EventArgs e)
        {
            var plateVisualTransform = Instantiate(this.platesVisualPrefab, this.counterTopPoint);

            var plateOffSetY = .1f;
            plateVisualTransform.localPosition = new Vector3(0, plateOffSetY * this.plateVisualGameObjectList.Count, 0);
            
            this.plateVisualGameObjectList.Add(plateVisualTransform.gameObject);
        }
    }
}