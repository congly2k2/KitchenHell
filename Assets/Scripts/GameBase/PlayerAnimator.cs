using Unity.Netcode;
using UnityEngine;

namespace GameBase
{
    public class PlayerAnimator : NetworkBehaviour
    {
        [SerializeField] private Player   player;
        private                  Animator animator;
        private const            string   IsWalking = "IsWalking";

        private void Awake()
        {
            this.animator = this.GetComponent<Animator>();
        }

        private void Update()
        {
            if (!this.IsOwner) return;
            this.animator.SetBool(IsWalking, this.player.IsWalking());
        }
    }
}