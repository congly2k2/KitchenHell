using System;
using System.Globalization;
using GameBase;
using TMPro;
using UnityEngine;

namespace UI
{
    public class GameStartCountDownUI : MonoBehaviour
    {
        private const string NumberPopup = "NumberPopup";
        [SerializeField] private TextMeshProUGUI countDownText;

        private Animator animator;
        private int previousCountdownNumber;
        private static readonly int Popup = Animator.StringToHash(GameStartCountDownUI.NumberPopup);

        private void Awake()
        {
            this.animator = this.GetComponent<Animator>();
        }

        private void Start()
        {
            KitchenGameManager.Instance.OnStateChanged += this.KitchenGameManager_OnStateChanged;
            this.gameObject.SetActive(false);
        }

        private void Update()
        {
            var countdownNumber = Mathf.CeilToInt(KitchenGameManager.Instance.GetCountDownToStartTimer());
            this.countDownText.text = countdownNumber.ToString(CultureInfo.InvariantCulture);

            if (this.previousCountdownNumber != countdownNumber)
            {
                this.previousCountdownNumber = countdownNumber;
                this.animator.SetTrigger(GameStartCountDownUI.Popup);
                SoundManager.Instance.PlayCountDownSound();
            }
        }

        private void KitchenGameManager_OnStateChanged(object sender, EventArgs e) { this.gameObject.SetActive(KitchenGameManager.Instance.IsCountDownStartActive()); }
    }
}
