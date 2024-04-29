using System;
using GameBase;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class DeliveryResultUI : MonoBehaviour
    {
        private const string Popup = "Popup";
        
        [SerializeField] private Image bgImage;
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI msgText;
        [SerializeField] private Color successColor;
        [SerializeField] private Color failColor;
        [SerializeField] private Sprite successSprite;
        [SerializeField] private Sprite failSprite;

        private Animator animator;
        private static readonly int Popup1 = Animator.StringToHash(DeliveryResultUI.Popup);

        private void Awake()
        {
            this.animator = this.GetComponent<Animator>();
        }

        private void Start()
        {
            DeliveryManager.Instance.OnRecipeSuccess += this.DeliveryManager_OnRecipeSuccess;
            DeliveryManager.Instance.OnRecipeFailed += this.DeliveryManager_OnRecipeFailed;
            
            this.gameObject.SetActive(false);
        }

        private void DeliveryManager_OnRecipeFailed(object sender, EventArgs e)
        {
            this.gameObject.SetActive(true);
            this.animator.SetTrigger(DeliveryResultUI.Popup1);
            this.bgImage.color = this.failColor;
            this.iconImage.sprite = this.failSprite;
            this.msgText.text = "DELIVERY\nFAILED";
        }

        private void DeliveryManager_OnRecipeSuccess(object sender, EventArgs e)
        {
            this.gameObject.SetActive(true);
            this.animator.SetTrigger(DeliveryResultUI.Popup1);
            this.bgImage.color = this.successColor;
            this.iconImage.sprite = this.successSprite;
            this.msgText.text = "DELIVERY\nSUCCESS";
        }
    }
}
