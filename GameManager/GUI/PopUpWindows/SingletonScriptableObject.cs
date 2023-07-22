using System;
using UnityEditor;using UnityEngine;

public class SingletonScriptableObject<T> : ScriptableObject where T : SingletonScriptableObject<T> 
{
      private static T instance;

      public static T Instance
      {
            get
            {
                  if (instance != null) return instance;
                  var assets = Resources.LoadAll<T>("");
                  if (assets == null || assets.Length < 1)
                        throw new SystemException($"assets of type {typeof(T)} not found");
                  else if (assets.Length > 1)
                        Debug.LogError($"more than 1 asset of type {typeof(T)} founded");
                  instance = assets[0];
                  return instance;
            }
      }
}
