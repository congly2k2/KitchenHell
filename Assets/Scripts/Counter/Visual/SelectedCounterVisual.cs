
using UnityEngine;

namespace Counter.Visual
{
    public class SelectedCounterVisual : MonoBehaviour
    {
        [SerializeField] private BaseCounter  baseCounter;
        [SerializeField] private GameObject[] visualGameObjectArray;

        private void Start()
        {
            Player.Instance.OnSelectedCounterChange += this.Player_OnSelectedCounterChange;
        }

        private void Player_OnSelectedCounterChange(object sender,
            Player.OnSelectedCounterChangedEventAgrs e)
        {
            if (e.SelectedCounter == this.baseCounter)
            {
                this.Show();
            }
            else
            {
                this.Hide();
            }
        }

        private void Show()
        {
            foreach (var visualGameObject in this.visualGameObjectArray)
            {
                visualGameObject.SetActive(true);
            }
        }

        private void Hide()
        {
            foreach (var visualGameObject in this.visualGameObjectArray)
            {
                visualGameObject.SetActive(false);
            }
        }
    }
}