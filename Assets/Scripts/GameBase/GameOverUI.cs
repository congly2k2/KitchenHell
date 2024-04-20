using UnityEngine;

namespace GameBase
{
    using System;
    using TMPro;

    public class GameOverUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI number;
        
        private void Start()
        {
            KitchenGameManager.Instance.OnStateChanged += this.KitchenGameManager_OnStateChanged;
            this.gameObject.SetActive(false);
        }

        private void KitchenGameManager_OnStateChanged(object sender, EventArgs e)
        {
            this.number.text = DeliveryManager.Instance.GetSuccessfulRecipeAmount().ToString();
            this.gameObject.SetActive(KitchenGameManager.Instance.IsGameOver());
        }
    }
}
