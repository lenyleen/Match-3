using System;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TextMeshProUGUI))][RequireComponent(typeof(Animator))]
public class AppearingWord : MonoBehaviour, ILvlCompletedObserver
{
        private TextMeshProUGUI Text => this.GetComponent<TextMeshProUGUI>();

        private void OnEnable()
        {
            SoundManager.instance.PlaySound("AppearingWord");
        }

        public void Initialize()
        {
            CellGridShuffler.onShuffleStarted += OnShuffle;
            EventBus.Subscribe(this);
        }

        private void OnShuffle()
        {
            Text.text = "SHUFFLE TIME!";
            this.gameObject.SetActive(true);
        }

        public void OnGoalReached()
        {
            Text.text = "BONUS TIME!";
            this.gameObject.SetActive(true);
        }

        public void DisableObject()
        {
            this.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            CellGridShuffler.onShuffleStarted -= OnShuffle;
            EventBus.Unsubscribe(this);
        }
}
