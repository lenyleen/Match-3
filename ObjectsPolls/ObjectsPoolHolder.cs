using System.Collections.Generic;
using UnityEngine;

public class ObjectsPoolHolder : MonoBehaviour,IObjectsPool
{
    public CandyColor type;
    [SerializeField]private PooledObject objectPrefab;
    private Stack<PooledObject> objects;
    [SerializeField] private int poolObjectsCount = 32;
    private Transform poolTransform;
    private void OnEnable()
    {
        poolTransform = this.transform;
        objects = new Stack<PooledObject>();
        for (var i = 0; i < poolObjectsCount; i++)
        {
            CreateInstance(); 
        }
    }
    public T GetObject<T>()
    {
        if (objects.Count <= 0)
            CreateInstance();
        var objectToReturn =objects.Pop();
        objectToReturn.gameObject.SetActive(true);
        return objectToReturn.GetComponent<T>();
    }

    private void CreateInstance()
    {
        var newObject =  Instantiate(objectPrefab, poolTransform.position, Quaternion.identity, poolTransform);
        objects.Push(newObject);
        newObject.gameObject.SetActive(false);
        newObject.Initialize(ReturnObject);
    }
    
    public void ReturnObject(PooledObject obj)
    {
        if(objects.Contains(obj))
            return;
        obj.transform.SetParent(poolTransform);
        obj.gameObject.SetActive(false);
        objects.Push(obj);
    }
}
