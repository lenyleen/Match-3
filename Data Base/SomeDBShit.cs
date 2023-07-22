using System;
using Firebase.Database;
using UnityEngine;

public class SomeDBShit : MonoBehaviour
{
        private DatabaseReference dbRef;
        private void Start()
        {
                dbRef = FirebaseDatabase.DefaultInstance.RootReference;
        }

        public void SaveData(string name)
        {
                dbRef.Child("gocno").SetValueAsync(name);
        }

}
