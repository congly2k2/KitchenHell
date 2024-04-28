using GameBase;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GamePlayingClockUI : MonoBehaviour
    {
        [SerializeField] private Image timerImage;

        private void Update()
        {
            this.timerImage.fillAmount = KitchenGameManager.Instance.GetGamePlayingTimerNormalized();
        }
    }
}
