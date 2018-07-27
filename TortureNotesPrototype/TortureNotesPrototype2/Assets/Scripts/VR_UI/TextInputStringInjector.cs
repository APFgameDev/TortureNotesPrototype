using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using NS_Annotation.NS_SO;

public class TextInputStringInjector : MonoBehaviour
{

    public TextMeshProUGUI Text;
    public StringVariable KeyboardStringInput;


    public void AssignText()
    {
        if (Text != null)
        {
            Text.text = KeyboardStringInput.Value;
        }
        else
        {
            Debug.Log("No valid text object passed in!");
        }
    }
}
