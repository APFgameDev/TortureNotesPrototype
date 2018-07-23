using System;
using System.Collections.Generic;
using UnityEngine;


public class ObjectPool<T> where T : MonoBehaviour
{
    [SerializeField]
    GameObject m_prefab;

    List<T> objectsInPool = new List<T>();

    List<T> activeObjects = new List<T>();

    uint expansionAmount;

    public ObjectPool(uint startingSize, uint aExpansionAmount, GameObject prefab)
    {
        m_prefab = prefab;

        if (m_prefab == null || m_prefab.GetComponent<T>() == null)
            Debug.LogError("ObjectPool of " + typeof(T).ToString() + "Is not valid");

        expansionAmount = aExpansionAmount;

        ExpandPool();
    }

    public T GetObjectFromPool()
    {
        if (objectsInPool.Count == 0)
            ExpandPool();

        if (objectsInPool.Count == 0)
            return null;
        else
        {
            T objectToReturn = objectsInPool[objectsInPool.Count - 1];
            objectsInPool.RemoveAt(objectsInPool.Count - 1);
            activeObjects.Add(objectToReturn);
            return objectToReturn;
        }
    }

    public void ReturnAllActiveToPool()
    {
        for (int i = 0; i < activeObjects.Count; i++)
        {
            activeObjects[i].gameObject.SetActive(false);
        }
    }


    void ExpandPool()
    {
        if (objectsInPool.Capacity < expansionAmount)
            objectsInPool.Capacity = (int)expansionAmount;

        T[] objects = new T[expansionAmount];

        for (int i = 0; i < expansionAmount; i++)
        {
            objects[i] = GameObject.Instantiate(m_prefab).GetComponent<T>();
            objects[i].gameObject.SetActive(false);
            objects[i].gameObject.AddComponent<ReturnToPoolOnDisabled>().SetReturnFunction(ReturnObjectToPool);
        }

        objectsInPool.AddRange(objects);
    }

    void ReturnObjectToPool(ReturnToPoolOnDisabled returnToPoolOnDisabled)
    {
        activeObjects.Remove(returnToPoolOnDisabled.GetComponent<T>());
        objectsInPool.Add(returnToPoolOnDisabled.GetComponent<T>());
    }
}
