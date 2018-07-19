using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NS_Managers;

//	Class Definition: The ObjectPoolManager allows for a hub to access runtime pools
//Singleton
namespace NS_ObjectPooling
{
    public enum PoolType
    {
        EnemyDeathParticle = 1,
        Enemy
    }

    [System.Serializable]
    public class ManagePool : ManagedObject<ObjectPool, PoolType>
    {
    }

    public class ObjectPoolManager : Manager <ObjectPoolManager,ObjectPool, PoolType, ManagePool>
    {
        // Use this for initialization
        void Awake()
        {
            base.Initialize(this);
            foreach (ObjectPool oP in m_objects.Values)
                oP.Start();
        }

        public static GameObject GetObjectFromPool(PoolType aPoolType, bool startActive = true)
        {
            return Instance.FindObject(aPoolType).GetObjectFromPool(startActive);
        }
    }
}
