using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NS_ObjectPooling
{
    [System.Serializable]
    //	Class Definition: The ObjectPool holds unEnabled GameObjects.
    public class ObjectPool
    {
        //List of unEnabled Gameobjects
        List<GameObject> m_objects = new List<GameObject>();
        //object used to clone if pool runs out
        public GameObject m_cloneObject = null;
        //determines whether we add more objects to m_objects when m_objects is Empty
        public bool m_autoExpand = false;
        [Range(1, 100)]
        public int m_expandAmount = 1;

        public int m_startSize = 1;

        public void Start()
        {
            m_objects.Clear();
            if (m_cloneObject == null)
                return;

            CloneToPool(m_cloneObject, m_startSize);
        }

        //Function Definition: Returns gO from m_objects
        public GameObject GetObjectFromPool(bool startActive = true)
        {
            if (m_objects.Count != 0)
            {
                GameObject gO = m_objects[m_objects.Count - 1];
                m_objects.RemoveAt(m_objects.Count - 1);
                if (gO == null)
                {
                    return GetObjectFromPool();
                }
                gO.SetActive(startActive);
                gO.transform.parent = null;
                return gO;
            }
            else if (m_autoExpand == true && m_cloneObject != null && m_expandAmount > 0)
            {
                CloneToPool(m_cloneObject, m_expandAmount);
                GameObject gO = m_objects[m_objects.Count - 1];
                m_objects.RemoveAt(m_objects.Count - 1);
                gO.transform.parent = null;
                gO.SetActive(true);
                return gO;
            }
            return null;
        }

        // 	Function Definition: adds pre instantiated GameObjects to m_objects
        public void AddToPool(GameObject[] goS)
        {
            for (int i = 0; i < goS.Length; i++)
                AddToPool(goS[i]);
        }
        // 	Function Definition: add pre instantiated GameObject to m_objects
        public void AddToPool(GameObject go)
        {
            if (go.activeSelf)
                go.SetActive(false);
            m_objects.Add(go);
        }

        //Function Definition: Instantiates a number of clones to pool adding them to m_objects
        public void CloneToPool(GameObject a_GO, int a_NumClones)
        {
            m_objects.Capacity = m_objects.Count + a_NumClones;
            for (int i = 0; i < a_NumClones; i++)
            {
                GameObject gO = GameObject.Instantiate(a_GO);
                gO.SetActive(false);
                m_objects.Add(gO);
                ReturnBackToPool returnBackToPool = gO.AddComponent<ReturnBackToPool>();
                returnBackToPool.m_returnPool = this;
            }
        }
    }
}