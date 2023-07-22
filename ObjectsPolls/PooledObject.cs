using System;
using UnityEngine;

public class PooledObject : MonoBehaviour
{
    private Action<PooledObject> returnToPool;

    public void Initialize(Action<PooledObject> returnAction)
    {
        returnToPool = returnAction;
    }

    public void ReturnToPool()
    {
        if (returnToPool == null)
        {
            gameObject.SetActive(false);
            return;
        }
        returnToPool.Invoke(this);
    }
}