using System;
using Counter;
using Interfaces;
using UnityEngine;

namespace UI
{
    public class StoveBurnWarningUI : MonoBehaviour
    {
        [SerializeField] private StoveCounter stoveCounter;

        private void Start()
        {
            this.stoveCounter.OnProgressChange += this.StoveCounter_OnProgressChange;
            
            this.gameObject.SetActive(false);
        }

        private void StoveCounter_OnProgressChange(object sender, IHasProgress.OnProgressChangeEventArgs e)
        {
            const float burnShowProgressAmount = .5f;
            var show = this.stoveCounter.IsFried() && e.ProgressNormalized >= burnShowProgressAmount;
            
            this.gameObject.SetActive(show);
        }
    }
}