using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class MovesCounter : MonoBehaviour, IMoveEndedObserver, IGameMovesLimiter
{
        private Text text => this.GetComponent<Text>();
        public int CurrentNumberOfMoves
        {
                get => currentNumberOfMoves;
                private set
                {
                    currentNumberOfMoves = value;
                    if (currentNumberOfMoves >= 0)
                    {
                        text.text = $"{currentNumberOfMoves}";
                        return;
                    }
                    onOutOfLimit?.Invoke();
                }
        }
        private int currentNumberOfMoves;
        public event Action onOutOfLimit;
        private int movesToAddAfterPayment;
        public void Initialize(int maxCountOfMoves)
        {
            this.gameObject.SetActive(true);
            EventBus.Subscribe(this);
            CurrentNumberOfMoves = maxCountOfMoves;
        }

        public void MoveEnded()
        {
            CurrentNumberOfMoves--;
        }

        public void AddMovesAfterPayment()
        {
            CurrentNumberOfMoves += 5;
        }
        private void OnDisable()
        {
            EventBus.Unsubscribe(this);
        }

        
}