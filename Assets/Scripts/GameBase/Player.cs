using System;
using System.Collections.Generic;
using Counter;
using Interfaces;
using Unity.Netcode;
using UnityEngine;

namespace GameBase
{
    public class Player : NetworkBehaviour, IKitchenObjectParent
    {
        public static event EventHandler OnAnyPlayerSpawned;
        public static event EventHandler OnAnyPickedSomething;
        
        public static void ResetStaticData()
        {
            Player.OnAnyPlayerSpawned = null;
        }
        public static Player LocalInstance { get; private set; }

        public event EventHandler                                    OnPickedSomething; 
        public event EventHandler<SelectedCounterChangedEventAgrs> OnSelectedCounterChange;
        public class SelectedCounterChangedEventAgrs : EventArgs
        {
            public BaseCounter SelectedCounter;
        }
    
        [SerializeField] private float     moveSpeed = 7f;
        [SerializeField] private LayerMask countersLayerMask;
        [SerializeField] private LayerMask collisionLayerMask;
        [SerializeField] private Transform kitchenObjectHoldPoint;
        [SerializeField] private List<Vector3> spawnPositionList;

        private bool          isWalking;
        private Vector3       lastInteractDir;
        private BaseCounter   selectedCounter;
        private KitchenObject kitchenObject;

        private void Awake()
        {
            // if (Instance != null)
            // {
            //     Debug.LogError("There is more than one Player");
            // }
            // Instance = this;
        }

        private void Start()
        {
            GameInput.Instance.OnInteractAction          += this.GameInput_OnInteraction;
            GameInput.Instance.OnInteractAlternateAction += this.GameInput_OnInteractAlternateAction;
        }

        public override void OnNetworkSpawn()
        {
            if (this.IsOwner) LocalInstance = this;

            this.transform.position = this.spawnPositionList[(int)this.OwnerClientId];
            
            OnAnyPlayerSpawned?.Invoke(this, EventArgs.Empty);
        }

        private void GameInput_OnInteractAlternateAction(object sender, EventArgs e)
        {
            if (!KitchenGameManager.Instance.IsPlaying()) return;
            
            if (this.selectedCounter != null)
            {
                this.selectedCounter.InteractAlternate(this);
            }
        }

        private void GameInput_OnInteraction(object sender, EventArgs e)
        {
            if (!KitchenGameManager.Instance.IsPlaying()) return;
            
            if (this.selectedCounter != null)
            {
                this.selectedCounter.Interact(this);
            }
        }

        private void Update()
        {
            if (!this.IsOwner) return;
            // this.HandleMovementServerAuth();
            this.HandleMovement();
            this.HandleInteractions();
        }

        public bool IsWalking()
        {
            return this.isWalking;
        }

        private void HandleInteractions()
        {
            var inputVector = GameInput.Instance.GetMovementVectorNormalized();
            var moveDir     = new Vector3(inputVector.x, 0f, inputVector.y);

            if (moveDir != Vector3.zero)
            {
                this.lastInteractDir = moveDir;
            }

            var interactDistance = 2f;
            if (Physics.Raycast(this.transform.position, this.lastInteractDir, out RaycastHit raycastHit, interactDistance, this.collisionLayerMask))
            {
                if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
                {
                    // Has a clearCounter
                    if (baseCounter != this.selectedCounter)
                    {
                        this.SetSelectedCounter(baseCounter);
                    }
                }
                else
                {
                    this.SetSelectedCounter(null);
                }
            }
            else
            {
                this.SetSelectedCounter(null);
            }
        }

        private void HandleMovementServerAuth()
        {
            var inputVecotr = GameInput.Instance.GetMovementVectorNormalized();
            this.HandleMovementServerRpc(inputVecotr);
        }

        [ServerRpc]
        private void HandleMovementServerRpc(Vector2 inputVector)
        {
            var moveDir      = new Vector3(inputVector.x, 0f, inputVector.y);
            var moveDistance = this.moveSpeed * Time.deltaTime;
            var playerRadius = .7f;
            var playerHeight = 2f;
            var canMove      = !Physics.BoxCast(this.transform.position, Vector3.one * playerRadius, moveDir, Quaternion.identity, moveDistance, this.collisionLayerMask);

            if (!canMove)
            {
                var moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
                canMove = moveDir.x != 0 && !Physics.BoxCast(this.transform.position, Vector3.one * playerRadius, moveDirX, Quaternion.identity, moveDistance, this.collisionLayerMask);

                if (canMove)
                {
                    moveDir = moveDirX;
                }
                else
                {
                    var moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                    canMove = moveDir.z != 0 && !Physics.BoxCast(this.transform.position, Vector3.one * playerRadius, moveDirZ, Quaternion.identity, moveDistance, this.collisionLayerMask);

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

        private void HandleMovement()
        {
            var inputVector  = GameInput.Instance.GetMovementVectorNormalized();
            var moveDir      = new Vector3(inputVector.x, 0f, inputVector.y);
            var moveDistance = this.moveSpeed * Time.deltaTime;
            var playerRadius = .7f;
            var playerHeight = 2f;
            var canMove      = !Physics.CapsuleCast(this.transform.position, this.transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

            if (!canMove)
            {
                var moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
                canMove = moveDir.x != 0 && !Physics.CapsuleCast(this.transform.position, this.transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

                if (canMove)
                {
                    moveDir = moveDirX;
                }
                else
                {
                    var moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                    canMove = moveDir.z != 0 && !Physics.CapsuleCast(this.transform.position, this.transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

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
            this.OnSelectedCounterChange?.Invoke(this, new SelectedCounterChangedEventAgrs()
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

            if (e != null)
            {
                this.OnPickedSomething?.Invoke(this, EventArgs.Empty);
                OnAnyPickedSomething?.Invoke(this, EventArgs.Empty);
            }
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

        public NetworkObject GetNetworkObject()
        {
            return this.NetworkObject;
        }
    }
}