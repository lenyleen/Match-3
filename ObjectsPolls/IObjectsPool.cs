using UnityEngine;

public interface IObjectsPool
{ 
    public T GetObject<T>();
    public void ReturnObject(PooledObject obj);
}
