using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private CuttingCounter cuttingCounter;
    [SerializeField] private Image barImage;

    private void Start()
    {
        this.cuttingCounter.OnProgressChange += CuttingCounter_OnProgressChange;

        this.barImage.fillAmount = 0f;
        this.Hide();
    }

    private void CuttingCounter_OnProgressChange(object sender,
                                                  CuttingCounter.OnProgressChangeEventArgs e)
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
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
