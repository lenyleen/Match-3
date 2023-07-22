using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class WheelOfFortunePopup : LvlWindowPopUp
{
        [SerializeField] private Image wheel;
        [SerializeField] private AnimationCurve curve;
        [Range(0, 1)] [SerializeField] private float timeTick;
        private bool spinned = false;
        private event Action onSpinStarted;
        public UnityEvent onSpinEnded;
        [SerializeField] private List<Gift> gifts;
        private const float wheelGiftAngleStep = 45;
        public void Initialize(Action subscriber)
        {
                onSpinStarted += subscriber;
        }
        private void Awake()
        {
                var items = ItemsDatabase.Instance.allItems;
                for (int i = 0; i < gifts.Count; i++)
                {
                        gifts[i].Initialize(items[i].sprite,items[i].GameID,items[i].Count);
                }
                
        }

        public void Spin()
        {
                if(spinned)
                        return;
                onSpinStarted?.Invoke();
                StartCoroutine(SpinTheWheel());
        }

        private IEnumerator SpinTheWheel()
        {
                spinned = true;
                float maxTime = Random.Range(0.75f, 2f);
                curve.MoveKey(curve.length - 1, new Keyframe(maxTime,0));
                float time = 0;
                while (time < maxTime)
                {
                        time += timeTick * Time.deltaTime;
                        wheel.rectTransform.Rotate(new Vector3(0, 0, curve.Evaluate(time)));
                        yield return 0;
                }  
                onSpinEnded?.Invoke();
                var wheelAngle = wheel.transform.rotation.eulerAngles.z;
                var giftIndex = Mathf.CeilToInt(wheelAngle / wheelGiftAngleStep);
                giftIndex = giftIndex > gifts.Count - 1 ? 0 : giftIndex;
                giftIndex = Mathf.Abs(giftIndex);
                GiveAGift(gifts[giftIndex]);
        }
        private void GiveAGift(Gift gift)
        {
                gameScene.OpenPopup<WheelOfFortuneCollectedGiftPopUp>(popup => popup.Initialize(gift));
                Debug.Log(gift.ID);
        }
}
