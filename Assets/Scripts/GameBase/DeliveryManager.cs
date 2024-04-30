using System;
using System.Collections.Generic;
using RecipeSO;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameBase
{
    public class DeliveryManager : NetworkBehaviour
    {
        public event EventHandler OnRecipeSpawn;
        public event EventHandler OnRecipeCompleted;
        public event EventHandler OnRecipeSuccess;
        public event EventHandler OnRecipeFailed;
        
        public static DeliveryManager Instance { get; private set; }
        
        [SerializeField] private RecipeListSo   recipeListSo;
        
        private       List<RecipeSo> waitingRecipeSoList;
        private       float          spawnRecipeTimer = 4f;
        private const float          SpawnRecipeTimerMax = 4f;
        private const int            WaitingRecipeMax    = 4;
        private       int            successfulRecipeAmount;

        private void Awake()
        {
            Instance                 = this;
            
            this.waitingRecipeSoList = new List<RecipeSo>();
        }

        private void Update()
        {
            if (!this.IsServer) return;
            this.spawnRecipeTimer -= Time.deltaTime;
            if (this.spawnRecipeTimer <= 0f)
            {
                this.spawnRecipeTimer = SpawnRecipeTimerMax;

                if (this.waitingRecipeSoList.Count < WaitingRecipeMax)
                {
                    var waitingRecipeSoIndex = Random.Range(0, this.recipeListSo.recipeSoList.Count);
                    
                    
                    this.SpawnNewWaitingRecipeClientRpc(waitingRecipeSoIndex);
                    
                }
            }
        }

        [ClientRpc]
        private void SpawnNewWaitingRecipeClientRpc(int waitingRecipeSoIndex)
        {
            var waitingRecipeSo = this.recipeListSo.recipeSoList[waitingRecipeSoIndex];
            this.waitingRecipeSoList.Add(waitingRecipeSo);
                    
            this.OnRecipeSpawn?.Invoke(this, EventArgs.Empty);
        }

        public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
        {
            foreach (var waitingRecipeSo in this.waitingRecipeSoList)
            {
                if (waitingRecipeSo.kitchenObjectSoList.Count != plateKitchenObject.GetKitchenObjectSoList().Count) continue;
                // Has the same number of ingredients
                var plateContentMatchesRecipe = true;
                foreach (var recipeKitchenObjectSo in waitingRecipeSo.kitchenObjectSoList)
                {
                    // Cycling through all ingredients in the recipe
                    var ingredientFound = false;
                    foreach (var plateKitchenObjectSo in plateKitchenObject.GetKitchenObjectSoList())
                    {
                        // Cycling through all ingredients in the recipe
                        if (plateKitchenObjectSo != recipeKitchenObjectSo) continue;
                        // Ingredients matches !
                        ingredientFound = true;
                        break;
                    }

                    if (!ingredientFound)
                    {
                        // This recipe ingredient was not found on the Plate
                        plateContentMatchesRecipe = false;
                    }
                }

                if (!plateContentMatchesRecipe) continue;
                // Player deliver correct recipe !
                var index = this.waitingRecipeSoList.IndexOf(waitingRecipeSo);
                this.DeliveryCorrectRecipeServerRpc(index);
                return;
            }
            
            // No matches found
            // Player did not a correct recipe
            this.DeliveryIncorrectRecipeServerRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        private void DeliveryIncorrectRecipeServerRpc()
        {
            this.DeliveryIncorrectRecipeClientRpc();
        }
        
        [ClientRpc]
        private void DeliveryIncorrectRecipeClientRpc()
        {
            this.OnRecipeFailed?.Invoke(this, EventArgs.Empty);
        }

        [ServerRpc(RequireOwnership = false)]
        private void DeliveryCorrectRecipeServerRpc(int index)
        {
            this.DeliveryCorrectRecipeClientRpc(index);
        }

        [ClientRpc]
        private void DeliveryCorrectRecipeClientRpc(int index)
        {
            this.successfulRecipeAmount++;
            this.waitingRecipeSoList.RemoveAt(index);
                
            this.OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
            this.OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
        }

        public List<RecipeSo> GetWaitingRecipeSoList() => this.waitingRecipeSoList;

        public int GetSuccessfulRecipeAmount() => this.successfulRecipeAmount;
    }
}