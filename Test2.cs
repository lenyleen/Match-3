using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Test2 : MonoBehaviour
{
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            GameSystemsManager.instance.advertisementSystem.ShowInterstitialAd((sener,o) => { Debug.Log("Sus");});
    }
}
