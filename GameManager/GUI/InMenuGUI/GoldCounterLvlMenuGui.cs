using System;
using TMPro;
using UnityEngine;

public class GoldCounterLvlMenuGui : MonoBehaviour
{
       [SerializeField] private TextMeshProUGUI goldCount;

       private void Start()
       {
              var coinSystem = GameSystemsManager.instance.coinsSystem;
              coinSystem.Subscribe(UpdateGoldCount);
              goldCount.text = coinSystem.totalCoins.ToString();
       }

       private void UpdateGoldCount(int count)
       {
              goldCount.text = count.ToString();
       }

       private void OnDisable()
       {
              GameSystemsManager.instance.coinsSystem.Unsubscribe(UpdateGoldCount);
       }
}
