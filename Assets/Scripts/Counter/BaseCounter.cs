using System;
using Interfaces;
using GameBase;
using UnityEngine;

namespace Counter
{
    public class BaseCounter : MonoBehaviour, IKitchenObjectParent
    {
        public static event EventHandler   OnAnyObjectPlacedHere;
        
        [SerializeField] private Transform counterTopPoint;

        protected KitchenObject kitchenObject;

        public virtual void Interact(Player player) { Debug.LogError("BaseCounter.Interact();"); }

        public virtual void InteractAlternate(Player player) { Debug.LogError("BaseCounter.InteractAlternate();"); }

        public Transform GetKitchenObjectFollowTransform() { return this.counterTopPoint; }

        public void SetKitchenObject(KitchenObject e)
        {
            this.kitchenObject = e;

            if (e != null)
            {
                OnAnyObjectPlacedHere?.Invoke(this, EventArgs.Empty);
            }
        }

        public KitchenObject GetKitchenObject() { return this.kitchenObject; }

        public void ClearKitchenObject() { this.kitchenObject = null; }

        public bool HasKitchenObject() { return this.kitchenObject != null; }
    }
}