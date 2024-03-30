using UnityEngine;

namespace GameBase
{
    public class PlayerAnimator : MonoBehaviour
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
            this.animator.SetBool(IsWalking, this.player.IsWalking());
        }
    }
}