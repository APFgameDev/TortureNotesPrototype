using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NS_Managers.NS_Input;

namespace NS_Managers.NS_GameManagement
{
    //Defines singleton of a MonoBehaviour class that is persistent between scenes
    //It is important to call Init Singleton when Awake is called with keyword this
    public abstract class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T>
    {
        //IMPORTANT: CALL INIT ON AWAKE
        private void Awake()
        {         
            InitSingleton((T)this);
        }
        //returns true if instance successfully set
        //IMPORTANT: CALL INIT ON AWAKE
        protected bool InitSingleton(T instance, bool dontDestroyOnLoad = true)
        {
            if(Instance != null)
            {
                Destroy(gameObject);
                Debug.Log("More than one of Instance: " + Instance.GetType().ToString() + " Was Made /n");
                return false;
            }

            if (dontDestroyOnLoad)
                DontDestroyOnLoad(gameObject);

            Instance = instance;
            return true;
        }

       protected static T Instance { get; private set; }
    } 
}
