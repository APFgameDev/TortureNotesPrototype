using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveToggle : MonoBehaviour
{
    [Tooltip("These objects will flip the value opposite of the toggle")]
    [SerializeField]
    GameObject[] gameObjectsFlipDisable;
    [Tooltip("These objects will flip to the value of the toggle")]
    [SerializeField]
    GameObject[] gameObjectsFlipEnable;

    public void SetActive(bool toggle)
    {
        for (int i = 0; i < gameObjectsFlipDisable.Length; i++)
            gameObjectsFlipDisable[i].SetActive(!toggle);

        for (int i = 0; i < gameObjectsFlipEnable.Length; i++)
            gameObjectsFlipEnable[i].SetActive(toggle);
    }
}
