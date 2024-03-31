using UnityEngine;

namespace Counter
{
    using System;
    using GameBase;

    public class PlatesCounter : BaseCounter
    {
        public event EventHandler                OnPlateSpawned;
        [SerializeField] private KitchenObjectSo plateKitchenObjectSo;

        private float spawnPlateTimer;
        private float spawnPlateTimerMax = 4f;

        private int platesSpawnAmount;
        private int platesSpawnAmountMax = 4;

        private void Update()
        {
            this.spawnPlateTimer += Time.deltaTime;
            if (this.spawnPlateTimer > this.spawnPlateTimerMax)
            {
                this.spawnPlateTimer = 0;

                if (this.platesSpawnAmount < this.platesSpawnAmountMax)
                {
                    this.platesSpawnAmount++;
                    
                    this.OnPlateSpawned?.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }
}