using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[CreateAssetMenu(fileName = "PopupHolder", menuName = "PopupHolder", order = 0)] 
public sealed class PopupHolderSingleton : SingletonScriptableObject<PopupHolderSingleton>
{
        [SerializeField] private List<LvlWindowPopUp> allPopups;

        private void OnEnable()
        {
                var popups = Resources.LoadAll<LvlWindowPopUp>("Prefabs/GUI/Popups"); 
                Debug.Log($"{popups.Length}");
                foreach (var popup in popups)
                {
                        if(!allPopups.Contains(popup))
                                allPopups.Add(popup);
                }
        }

        public LvlWindowPopUp SpawnPrefab(Type type, Canvas canvas)
        {
                var window = allPopups.FirstOrDefault(popup => popup.GetType() == type);
                var spawnedWindow = Instantiate(window, canvas.transform);
                spawnedWindow.enabled = true;
                return spawnedWindow;
        }
}
