using UnityEngine;

public class TestDb : MonoBehaviour
{
    [SerializeField] private SomeDBShit db;
    [SerializeField] private Auth auth;

    public void WriteData()
    {
        auth.LoadProfile();
        Debug.Log("SUs");
    }
}
