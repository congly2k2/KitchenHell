using UnityEngine;

namespace UI
{
    using System;
    using GameBase;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;

    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] private Button playButton;
        [SerializeField] private Button quitButton;

        private void Awake()
        {
            this.playButton.onClick.AddListener(this.OnClickPlay);
            this.quitButton.onClick.AddListener(this.OnClickQuit);
        }

        private void OnClickQuit()
        {
            Application.Quit();
        }

        private void OnClickPlay()
        {
            Loader.Load(Loader.Scene.GameScene);
        }
    }
}
