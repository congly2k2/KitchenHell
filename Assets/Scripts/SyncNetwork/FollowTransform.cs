using System;
using UnityEngine;

namespace SyncNetwork
{
    public class FollowTransform : MonoBehaviour
    {
        private Transform target;

        public void SetTargetTransform(Transform targetTransform)
        {
            this.target = targetTransform;
        }

        private void LateUpdate()
        {
            if (this.target == null) return;

            this.transform.position = this.target.position;
            this.transform.rotation = this.target.rotation;
        }
    }
}