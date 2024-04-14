using UnityEngine;

namespace GameBase
{
    using Interfaces;
    using RecipeSO;

    public class KitchenObject : MonoBehaviour
    {
        [SerializeField] private KitchenObjectSo kitchenObjectSo;

        private IKitchenObjectParent kitchenObjectParent;

        public KitchenObjectSo GetKitchenObjectSo() { return this.kitchenObjectSo; }

        public void SetKitchenObjectParent(IKitchenObjectParent e)
        {
            if (this.kitchenObjectParent != null)
            {
                this.kitchenObjectParent.ClearKitchenObject();
            }

            this.kitchenObjectParent = e;

            if (e.HasKitchenObject())
            {
                Debug.LogError("IKitchenObjectParent already has a KitchenObject");
            }

            e.SetKitchenObject(this);

            this.transform.parent        = this.kitchenObjectParent.GetKitchenObjectFollowTransform();
            this.transform.localPosition = Vector3.zero;
        }

        public IKitchenObjectParent GetKitchenObjectParent() { return this.kitchenObjectParent; }

        public void DestroySelf()
        {
            this.kitchenObjectParent.ClearKitchenObject();

            Destroy(this.gameObject);
        }

        public bool TryGetPlate(out PlateKitchenObject plateKitchenObject)
        {
            if (this is PlateKitchenObject)
            {
                plateKitchenObject = this as PlateKitchenObject;
                return true;
            }

            plateKitchenObject = null;
            return false;
        }

        public static KitchenObject SpawnKitchenObject(KitchenObjectSo kitchenObjectSo,
            IKitchenObjectParent kitchenObjectParent)
        {
            Transform     kitchenObjectTransform = Instantiate(kitchenObjectSo.prefab);
            KitchenObject kitchenObject          = kitchenObjectTransform.GetComponent<KitchenObject>();
            kitchenObject.SetKitchenObjectParent(kitchenObjectParent);

            return kitchenObject;
        }
    }
}