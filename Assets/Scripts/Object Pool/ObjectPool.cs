using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private Vector3 poolPosition;
    [SerializeField] private GameObject poolObject;
    [SerializeField] private int poolSize;

    private Queue<GameObject> _poolQueue;

    public int CurrentPoolSize => _poolQueue.Count;

    public void InitPool()
    {
        _poolQueue = new Queue<GameObject>();

        for(int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(poolObject, poolPosition, Quaternion.identity, transform);
            obj.SetActive(false);
            _poolQueue.Enqueue(obj);
        }
    }

    public GameObject GetObject()
    {
        if (_poolQueue.Count <= 0)
        {
            Debug.LogError("Pool is empty");
            return null;
        }
        GameObject obj = _poolQueue.Dequeue();
        obj.SetActive(true);
        obj.transform.SetParent(null);
        return obj;
    }

    public void ReturnObject(GameObject obj)
    {
        obj.transform.position = poolPosition;
        obj.SetActive(false);
        obj.transform.SetParent(transform);
        _poolQueue.Enqueue(obj);
    }
}
