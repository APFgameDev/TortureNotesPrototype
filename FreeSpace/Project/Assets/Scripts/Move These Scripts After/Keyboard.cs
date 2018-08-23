using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keyboard : MonoBehaviour
{
    /// <summary>
    /// Used to turn the keyboard on or off
    /// </summary>
    /// <param name="active"></param>
    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }
}
