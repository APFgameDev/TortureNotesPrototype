using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NS_ObjectPooling
{
    //Class Definition: ReturnBackToPool is a component that will return gamobject back to pool when gameobject is disabled
    public class ReturnBackToPool : MonoBehaviour
    {
        //ObjectPool to return to
        public ObjectPool m_returnPool = null;

        void OnDisable()
        {
            if (m_returnPool != null)
                m_returnPool.AddToPool(gameObject);
        }
    }
}
