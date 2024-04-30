
using System;
using UnityEngine;

namespace Counter.Visual
{
    using GameBase;

    public class SelectedCounterVisual : MonoBehaviour
    {
        [SerializeField] private BaseCounter  baseCounter;
        [SerializeField] private GameObject[] visualGameObjectArray;

        private void Start()
        {
            if (Player.LocalInstance != null)
            { 
                Player.LocalInstance.OnSelectedCounterChange += this.Player_OnSelectedCounterChange;
            }
            else
            {
                Player.OnAnyPlayerSpawned += this.Player_OnAnyPlayerSpawned;
            }
            
        }

        private void Player_OnAnyPlayerSpawned(object sender, EventArgs e)
        {
            if (Player.LocalInstance != null)
            { 
                Player.LocalInstance.OnSelectedCounterChange -= this.Player_OnSelectedCounterChange;
                Player.LocalInstance.OnSelectedCounterChange += this.Player_OnSelectedCounterChange;
            }
        }

        private void Player_OnSelectedCounterChange(object sender,
            Player.SelectedCounterChangedEventAgrs e)
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