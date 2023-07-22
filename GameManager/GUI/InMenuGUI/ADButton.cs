using System;
using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Button))]
public class ADButton : MonoBehaviour
{
        private Button addHearthButton;
        public event Action OnUserEarnedReward;
        private void Awake()
        {
                addHearthButton = this.GetComponent<Button>();
                addHearthButton.onClick.AddListener(() => {GameSystemsManager.instance.advertisementSystem.
                        ShowRewardedAd(((sender, reward) =>
                        {
                                OnUserEarnedReward?.Invoke();
                        } ));});
        }
}
