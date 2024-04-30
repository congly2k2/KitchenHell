using Unity.Netcode;
using UnityEngine;

namespace Counter
{
    using System;
    using GameBase;
    using RecipeSO;

    public class PlatesCounter : BaseCounter
    {
        public event EventHandler OnPlateSpawned;
        public event EventHandler OnPlateRemoved;
        
        [SerializeField] private KitchenObjectSo plateKitchenObjectSo;

        private float spawnPlateTimer;
        private float spawnPlateTimerMax = 4f;

        private int platesSpawnAmount;
        private int platesSpawnAmountMax = 4;

        private void Update()
        {
            if (!this.IsServer) return;
            this.spawnPlateTimer += Time.deltaTime;
            if (this.spawnPlateTimer > this.spawnPlateTimerMax)
            {
                this.spawnPlateTimer = 0;

                if (this.platesSpawnAmount < this.platesSpawnAmountMax)
                {
                    this.SpawnPlateServerRpc();
                }
            }
        }

        [ServerRpc]
        private void SpawnPlateServerRpc()
        {
            this.SpawnPlateClientRpc();
        }

        [ClientRpc]
        private void SpawnPlateClientRpc()
        {
            this.platesSpawnAmount++;
                    
            this.OnPlateSpawned?.Invoke(this, EventArgs.Empty);
        }

        public override void Interact(Player player)
        {
            if (!player.HasKitchenObject())
            {
                // Player is empty handed
                if (this.platesSpawnAmount > 0)
                {
                    // There's at least one plate here
                    KitchenObject.SpawnKitchenObject(this.plateKitchenObjectSo, player);
                    
                    this.InteractLogicServerRpc();
                }
            }
        }
        
        [ServerRpc(RequireOwnership = false)]
        private void InteractLogicServerRpc()
        {
            this.InteractLogicClientRpc();
        }

        [ClientRpc]
        private void InteractLogicClientRpc()
        {
            this.platesSpawnAmount--;
            this.OnPlateRemoved?.Invoke(this, EventArgs.Empty);
        }
    }
}