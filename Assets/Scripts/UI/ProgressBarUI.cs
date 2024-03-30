using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    using Interfaces;

    public class ProgressBarUI : MonoBehaviour
    {
        [SerializeField] private GameObject hasProgressGameObject;
        [SerializeField] private Image      barImage;

        private IHasProgress hasProgress;
        private void Start()
        {
            this.hasProgress = this.hasProgressGameObject.GetComponent<IHasProgress>();
            if (this.hasProgress is null)
            {
                Debug.Log($"Game object {this.hasProgressGameObject} does not have a component that implements IHasProgress !");
            }
            this.hasProgress.OnProgressChange += this.HasProgress_OnProgressChange;

            this.barImage.fillAmount = 0f;
            this.Hide();
        }

        private void HasProgress_OnProgressChange(object sender, IHasProgress.OnProgressChangeEventArgs e)
        {
            this.barImage.fillAmount = e.ProgressNormalized;

            if (e.ProgressNormalized == 0f || e.ProgressNormalized == 1f)
            {
                this.Hide();
            }
            else
            {
                this.Show();
            }
        }

        private void Show()
        {
            this.gameObject.SetActive(true);
        }

        private void Hide()
        {
            this.gameObject.SetActive(false);
        }
    }
}
