using UnityEngine;

namespace GameBase
{
    using System;

    public class KitchenGameManager : MonoBehaviour
    {
        public static KitchenGameManager Instance { get; private set; }

        public event EventHandler OnStateChanged;
        
        private enum State
        {
            WaitingToStart,
            CountDownToStart,
            GamePlaying,
            GameOver
        }

        private State state;

        private float waitingToStartTimer = 1f;
        private float countDownToStartTimer = 3f;
        private float gamePlayingTimer;
        private float gamePlayingTimerMax = 10f;

        private void Awake()
        {
            Instance   = this;
            this.state = State.WaitingToStart;
        }

        private void Update()
        {
            switch (this.state)
            {
                case State.WaitingToStart:
                    this.waitingToStartTimer -= Time.deltaTime;
                    if (this.waitingToStartTimer < 0f)
                    {
                        this.state = State.CountDownToStart;
                        this.OnStateChanged?.Invoke(this, EventArgs.Empty);
                    }
                    break;
                case State.CountDownToStart:
                    this.countDownToStartTimer -= Time.deltaTime;
                    if (this.countDownToStartTimer < 0f)
                    {
                        this.gamePlayingTimer = this.gamePlayingTimerMax;
                        this.state            = State.GamePlaying;
                        this.OnStateChanged?.Invoke(this, EventArgs.Empty);
                    }
                    break;
                case State.GamePlaying:
                    this.gamePlayingTimer -= Time.deltaTime;
                    if (this.gamePlayingTimer < 0f)
                    {
                        this.state            = State.GameOver;
                        this.OnStateChanged?.Invoke(this, EventArgs.Empty);
                    }
                    break;
                case State.GameOver:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public bool IsPlaying() => this.state == State.GamePlaying;

        public bool IsCountDownStartActive() => this.state == State.CountDownToStart;

        public bool IsGameOver() => this.state == State.GameOver;

        public float GetCountDownToStartTimer() => this.countDownToStartTimer;

        public float GetGamePlayingTimerNormalized() => 1 - this.gamePlayingTimer / this.gamePlayingTimerMax;
    }
}
