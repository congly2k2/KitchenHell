using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKitchenObjectParent
{
    public Transform GetKitchenObjectFollowTransform();


    public void SetKitchenObject(KitchenObject e);


    public KitchenObject GetKitchenObject();


    public void ClearKitchenObject();


    public bool HasKitchenObject();

}
