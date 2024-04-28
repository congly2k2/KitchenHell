using UnityEngine;

namespace GameBase
{
    using System;

    public class LoaderCallback : MonoBehaviour
    {
        private bool isFirstUpdate = true;

        private void Update()
        {
            if (this.isFirstUpdate)
            {
                this.isFirstUpdate = false;
                
                Loader.LoaderCallback();
            }
        }
    }
}
