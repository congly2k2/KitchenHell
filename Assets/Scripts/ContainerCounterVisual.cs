using UnityEngine;

public class ContainerCounterVisual : MonoBehaviour
{
    private const string OPEN_CLOSE = "OpenClose"; 
    
    [SerializeField] private ContainerCounter containerCounter;
    private                  Animator         animator;

    private void Awake()
    {
        this.animator = this.GetComponent<Animator>();
    }

    private void Start()
    {
        this.containerCounter.OnPlayerGrabbedObject += this.ContainerCounter_OnPlayerGrabbedObject;
    }

    private void ContainerCounter_OnPlayerGrabbedObject(object sender,
                                                        System.EventArgs e)
    {
        this.animator.SetTrigger(OPEN_CLOSE);
    }
}