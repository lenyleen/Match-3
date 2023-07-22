using System;
using Firebase;
using Firebase.Auth;
using UnityEngine;

public class Auth : MonoBehaviour
{
    private FirebaseAuth auth;
    private void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
    }

    public async void CreateProfile()
    {
        try
        {
            await auth.CreateUserWithEmailAndPasswordAsync("Sus@gma.com", "Password");
        }
        catch (FirebaseException e)
        {
            Debug.LogError(e.Message);
            throw;
        }
    }

    public async void LoadProfile()
    {
        try
        {
            await auth.SignInWithEmailAndPasswordAsync("Sus@gma.com", "Password");
        }
        catch (FirebaseException e)
        {
            Debug.LogError(e.Message);
        }
        finally
        {
            Debug.Log("Logined");
        }
    }
}
