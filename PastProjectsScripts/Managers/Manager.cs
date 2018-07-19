using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using NS_Managers.NS_GameManagement;

namespace NS_Managers
{
    [System.Serializable]
    public class ManagedObject <T,I>
    {
        [SerializeField]
        public T m_Value;
        [SerializeField]
        public I m_Key;
    }

    // Templated Types:
    //T =  inherited type : the class type inherting off Manager
    //V = value : the type of value we are managing
    //I = key Type : the key for storing the Value
    // 
    [System.Serializable]
    public class Manager<T, V, I, M> : SingletonBehaviour<T> where M : ManagedObject<V,I> where T : Manager<T,V,I,M>
    {
        [SerializeField]
        M[] m_objectsArray;
        protected Dictionary<I, V> m_objects = new Dictionary<I, V>();

        protected void Initialize(T instance)
        {
            InitSingleton(instance);
            AddRange(m_objectsArray);
        }

        public V FindObject(I aID)
        {
            V o;
            m_objects.TryGetValue(aID, out o);
            if (o == null)
            {
                Debug.LogWarning("Manager.FindObject could not findrequested id");
            }
            return o;
        }

        public void AddRange(M[] values)
        {
            for (int i = 0; i < values.Length; i++)
                m_objects.Add(values[i].m_Key, values[i].m_Value);
        }
    }
}