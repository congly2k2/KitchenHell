using System;
using UnityEngine;

public class CuttingCounterVisual : MonoBehaviour
{
    private const string CUT = "Cut"; 
    
    [SerializeField] private CuttingCounter cuttingCounter;
    private                  Animator         animator;

    private void Awake()
    {
        this.animator = this.GetComponent<Animator>();
    }

    private void Start()
    {
        this.cuttingCounter.OnCut += CuttingCounter_OnCut;
    }

    private void CuttingCounter_OnCut(object sender,
                                       EventArgs e)
    {
        this.animator.SetTrigger(CUT);
    }
    
}