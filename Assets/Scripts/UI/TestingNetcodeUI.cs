using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TestingNetcodeUI : MonoBehaviour
    {
        [SerializeField] private Button startHostBtn;
        [SerializeField] private Button startClientBtn;

        private void Awake()
        {
            this.startHostBtn.onClick.AddListener(() =>
            {
                Debug.Log("HOST");
                NetworkManager.Singleton.StartHost();
                this.Hide();
            });
            this.startClientBtn.onClick.AddListener(() =>
            {
                Debug.Log("CLIENT");
                NetworkManager.Singleton.StartClient();
                this.Hide();
            });
        }

        private void Hide() => this.gameObject.SetActive(false);
    }
}