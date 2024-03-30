using System;
using Counter;
using GameBase;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    public static Player                                         Instance { get; private set; }
    
    public event EventHandler<OnSelectedCounterChangedEventAgrs> OnSelectedCounterChange;
    public class OnSelectedCounterChangedEventAgrs : EventArgs
    {
        public BaseCounter SelectedCounter;
    }
    
    [SerializeField] private float     moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint;

    private bool          isWalking;
    private Vector3       lastInteractDir;
    private BaseCounter   selectedCounter;
    private KitchenObject kitchenObject;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one Player");
        }
        Instance = this;
    }

    private void Start()
    {
        this.gameInput.OnInteractAction += GameInput_OnInteraction;
        this.gameInput.OnInteractAlternateAction += GameInputOnOnInteractAlternateAction;
    }

    private void GameInputOnOnInteractAlternateAction(object sender,
                                                      EventArgs e)
    {
        if (this.selectedCounter != null)
        {
            this.selectedCounter.InteractAlternate(this);
        }
    }

    private void GameInput_OnInteraction(object sender,
                                         EventArgs e)
    {
        if (this.selectedCounter != null)
        {
            this.selectedCounter.Interact(this);
        }
    }

    private void Update()
    {
        this.HandleMovement();
        this.HandleInteractions();
    }

    public bool IsWalking()
    {
        return this.isWalking;
    }

    private void HandleInteractions()
    {
        var inputVector = this.gameInput.GetMovementVectorNormalized();
        var moveDir     = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDir != Vector3.zero)
        {
            this.lastInteractDir = moveDir;
        }

        var interactDistance = 2f;
        if (Physics.Raycast(transform.position, this.lastInteractDir, out RaycastHit raycastHit, interactDistance, this.countersLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                // Has a clearCounter
                if (baseCounter != this.selectedCounter)
                {
                    SetSelectedCounter(baseCounter);
                }
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }
    }

    private void HandleMovement()
    {
        var inputVector  = this.gameInput.GetMovementVectorNormalized();
        var moveDir      = new Vector3(inputVector.x, 0f, inputVector.y);
        var moveDistance = this.moveSpeed * Time.deltaTime;
        var playerRadius = .7f;
        var playerHeight = 2f;
        var canMove      = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        if (!canMove)
        {
            var moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = moveDir.x != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

            if (canMove)
            {
                moveDir = moveDirX;
            }
            else
            {
                var moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = moveDir.z != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

                if (canMove)
                {
                    moveDir = moveDirZ;
                }
                else
                {
                    // Stop
                }
            }
        }

        if (canMove)
        {
            this.transform.position += moveDir * (this.moveSpeed * Time.deltaTime);
        }

        this.isWalking = moveDir != Vector3.zero;
        const float rotateSpeed = 10f;
        this.transform.forward = Vector3.Slerp(this.transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }

    private void SetSelectedCounter(BaseCounter selectedCounterParam)
    {
        this.selectedCounter = selectedCounterParam;
        OnSelectedCounterChange?.Invoke(this, new OnSelectedCounterChangedEventAgrs()
        {
            SelectedCounter = this.selectedCounter
        });
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return this.kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject e)
    {
        this.kitchenObject = e;
    }

    public KitchenObject GetKitchenObject()
    {
        return this.kitchenObject;
    }

    public void ClearKitchenObject()
    {
        this.kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return this.kitchenObject != null;
    }
}