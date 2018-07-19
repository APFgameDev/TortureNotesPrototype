using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NS_Managers.NS_GameManagement
{
    public class DontDestroyOnLoad : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("checks if there is an object in the scene for conflicting gameobject.name")]
        bool m_checkForDuplicate = true;

        void Awake()
        {
            if (m_checkForDuplicate && GameObject.Find(gameObject.name) == null)
                Destroy(gameObject);
            else
                DontDestroyOnLoad(this);
        }
    }
}