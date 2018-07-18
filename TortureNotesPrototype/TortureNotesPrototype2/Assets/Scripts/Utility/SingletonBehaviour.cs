using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Defines singleton of a Monobehaviour class that can be persistent between scenes
//It is important to call Init Singleton when Awke is called with keyword this
public class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T> {

	// Use this for initialization
	void Awake ()
    {
		InitSingleton((T)this);
	}
	
	protected bool InitSingleton(T instance, bool dontDestroyOnLoad = true)
    {
        if (instance != null)
        {
            Destroy(gameObject);
            //complain
            Debug.Log("More than one instance of singleton " + instance.GetType().ToString() + " Was Made /n");
            return false;
        }

        if (dontDestroyOnLoad)
            DontDestroyOnLoad(gameObject);

        Instance = instance;
        return true;

    }

    protected static T Instance { get; private set; }
}
