using System;
using GameBase;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    public class OptionsUI : MonoBehaviour
    {
        public static OptionsUI Instance { get; private set; }

        [SerializeField] private Button soundBtn;
        [SerializeField] private Button musicBtn;
        [SerializeField] private Button closeBtn;
        [SerializeField] private Button moveUpBtn;
        [SerializeField] private Button moveDownBtn;
        [SerializeField] private Button moveLeftBtn;
        [SerializeField] private Button moveRightBtn;
        [SerializeField] private Button interactBtn;
        [SerializeField] private Button interactAlternateBtn;
        [SerializeField] private Button pauseBtn;
        [SerializeField] private TextMeshProUGUI soundText;
        [SerializeField] private TextMeshProUGUI musicText;
        [SerializeField] private TextMeshProUGUI moveUpText;
        [SerializeField] private TextMeshProUGUI moveDownText;
        [SerializeField] private TextMeshProUGUI moveLeftText;
        [SerializeField] private TextMeshProUGUI moveRightText;
        [SerializeField] private TextMeshProUGUI interactText;
        [SerializeField] private TextMeshProUGUI interactAlternateText;
        [SerializeField] private TextMeshProUGUI pauseText;

        [SerializeField] private Transform pressToRebind;

        private void Awake()
        {
            Instance = this;

            this.Hide();
            this.HidePress();

            this.musicBtn.onClick.AddListener(() =>
            {
                MusicManager.Instance.ChangeVolume();
                this.UpdateVisual();
            });

            this.soundBtn.onClick.AddListener(() =>
            {
                SoundManager.Instance.ChangeVolume();
                this.UpdateVisual();
            });

            this.closeBtn.onClick.AddListener(this.Hide);

            this.moveUpBtn.onClick.AddListener(() => { this.RebindBinding(GameInput.Binding.MoveUp); });
            this.moveDownBtn.onClick.AddListener(() => { this.RebindBinding(GameInput.Binding.MoveDown); });
            this.moveLeftBtn.onClick.AddListener(() => { this.RebindBinding(GameInput.Binding.MoveLeft); });
            this.moveRightBtn.onClick.AddListener(() => { this.RebindBinding(GameInput.Binding.MoveRight); });
            this.interactBtn.onClick.AddListener(() => { this.RebindBinding(GameInput.Binding.Interact); });
            this.interactAlternateBtn.onClick.AddListener(() => { this.RebindBinding(GameInput.Binding.InteractAlternate); });
            this.pauseBtn.onClick.AddListener(() => { this.RebindBinding(GameInput.Binding.Pause); });
        }

        private void Start()
        {
            KitchenGameManager.Instance.OnGameResume += this.KitchenGameManager_OnGameResume;
            this.UpdateVisual();
        }

        private void KitchenGameManager_OnGameResume(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void UpdateVisual()
        {
            this.soundText.text = "Sound Effects: " + Mathf.Round(SoundManager.Instance.GetVolume() * 10f);
            this.musicText.text = "Music: " + Mathf.Round(MusicManager.Instance.GetVolume() * 10f);

            this.moveUpText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveUp);
            this.moveDownText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveDown);
            this.moveLeftText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveLeft);
            this.moveRightText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveRight);
            this.interactText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
            this.interactAlternateText.text = GameInput.Instance.GetBindingText(GameInput.Binding.InteractAlternate);
            this.pauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);
        }

        public void Show() => this.gameObject.SetActive(true);
        private void Hide() => this.gameObject.SetActive(false);

        private void ShowPress() => this.pressToRebind.gameObject.SetActive(true);

        private void HidePress() => this.pressToRebind.gameObject.SetActive(false);

        private void RebindBinding(GameInput.Binding binding)
        {
            this.ShowPress();
            GameInput.Instance.RebindBinding(binding, () =>
            {
                this.HidePress();
                this.UpdateVisual();
            });
        }
    }
}