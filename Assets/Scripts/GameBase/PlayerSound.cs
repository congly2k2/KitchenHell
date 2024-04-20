namespace GameBase
{
    using System;
    using UnityEngine;

    public class PlayerSound : MonoBehaviour
    {
        private Player player;

        private float footStepTimer;
        private float footStepTimerMax = .1f;

        private void Awake()
        {
            this.player = this.GetComponent<Player>();
        }

        private void Update()
        {
            this.footStepTimer -= Time.deltaTime;
            if (this.footStepTimer < 0)
            {
                this.footStepTimer = this.footStepTimerMax;

                if (this.player.IsWalking())
                {
                    SoundManager.Instance.PlayFootStepSound(this.player.transform.position, 1f);
                }
            }
        }
    }
}