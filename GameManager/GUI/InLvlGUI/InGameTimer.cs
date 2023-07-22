using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class InGameTimer : MonoBehaviour, ITimerStop, IGameMovesLimiter
{
        private TextMeshProUGUI timer;
        private int timeToLoseInSeconds;
        private TimeSpan timerCounter;
        private ITimerStop timerStopImplementation;
        public event Action onOutOfLimit;
        private int timeToAddAfterPayment;
        public void Initialize(int timeToLoseInSec)
        {
                this.gameObject.SetActive(true);
                EventBus.Subscribe(this);
                timeToAddAfterPayment = timeToLoseInSec;
                timeToLoseInSeconds = timeToLoseInSec;
                timer = this.GetComponent<TextMeshProUGUI>();
                timerCounter = TimeSpan.FromSeconds(timeToLoseInSeconds);
                timer.text = timerCounter.ToString(@"ss");
                StartTimer();
        }

        private IEnumerator SetTimer()
        {
                while (timeToLoseInSeconds > 0)
                {
                        yield return new WaitForSeconds(1f);
                        timeToLoseInSeconds--;
                        timer.text = timeToLoseInSeconds.ToString();
                }
                onOutOfLimit?.Invoke();
        }

        public void AddMovesAfterPayment()
        {
                timeToLoseInSeconds += timeToAddAfterPayment;
        }

        public void StartTimer()
        {
                StartCoroutine(SetTimer());
        }

        public void StopTimer()
        {
                StopAllCoroutines();
        }

        private void OnDisable()
        {
                EventBus.Unsubscribe(this);
        }
}
