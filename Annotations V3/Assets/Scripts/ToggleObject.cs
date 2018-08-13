using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleObject : MonoBehaviour
{
    public void ToggleObjectOnOrOff()
    {
        gameObject.SetActive(!gameObject.activeInHierarchy);
    }
}
