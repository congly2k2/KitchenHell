using System;
using UnityEngine;

namespace Counter.Visual
{
    public class CuttingCounterVisual : MonoBehaviour
    {
        private const string CUT = "Cut"; 
    
        [SerializeField] private CuttingCounter cuttingCounter;
        private                  Animator       animator;

        private void Awake()
        {
            this.animator = this.GetComponent<Animator>();
        }

        private void Start()
        {
            this.cuttingCounter.OnCut += this.CuttingCounter_OnCut;
        }

        private void CuttingCounter_OnCut(object sender,
            EventArgs e)
        {
            this.animator.SetTrigger(CUT);
        }
    
    }
}