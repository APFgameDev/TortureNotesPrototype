using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveToggle : MonoBehaviour
{
    public void SetActive()
    {
        gameObject.SetActive(!gameObject.activeInHierarchy);
    }
}
