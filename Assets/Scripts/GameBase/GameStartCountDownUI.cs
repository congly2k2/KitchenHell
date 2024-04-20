using UnityEngine;

namespace GameBase
{
    using System;
    using System.Globalization;
    using TMPro;

    public class GameStartCountDownUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI countDownText;

        private void Start()
        {
            KitchenGameManager.Instance.OnStateChanged += this.KitchenGameManager_OnStateChanged;
            this.gameObject.SetActive(false);
        }

        private void Update()
        {
            this.countDownText.text = Mathf.Ceil(KitchenGameManager.Instance.GetCountDownToStartTimer()).ToString(CultureInfo.InvariantCulture);
        }

        private void KitchenGameManager_OnStateChanged(object sender, EventArgs e) { this.gameObject.SetActive(KitchenGameManager.Instance.IsCountDownStartActive()); }
    }
}
