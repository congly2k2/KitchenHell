using GameBase;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GamePauseUI : MonoBehaviour
    {
        [SerializeField] private Button mainMenuBtn;
        [SerializeField] private Button resumeBtn;
        [SerializeField] private Button optionBtn;

        private void Awake()
        {
            this.mainMenuBtn.onClick.AddListener(() =>
            {
                Loader.Load(Loader.Scene.MainMenuScene);
            });
            
            this.resumeBtn.onClick.AddListener(() =>
            {
                KitchenGameManager.Instance.PauseGame();
            });
            
            this.optionBtn.onClick.AddListener(() =>
            {
                OptionsUI.Instance.Show();
            });
        }

        private void Start()
        {
            this.gameObject.SetActive(false);
            KitchenGameManager.Instance.OnGamePause += (_, _) => { this.gameObject.SetActive(true);};
            KitchenGameManager.Instance.OnGameResume += (_, _) => { this.gameObject.SetActive(false);};
        }
    }
}