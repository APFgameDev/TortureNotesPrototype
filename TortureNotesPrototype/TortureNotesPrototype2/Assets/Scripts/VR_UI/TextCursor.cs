using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using NS_Annotation.NS_SO;

public class TextCursor : MonoBehaviour
{
    public TextMeshProUGUI Text;
    public StringVariable KeyboardInputStringData;
    public StringVariable KeyboardInputCharData;

    private float timeStamp;
    private bool cursor = false;
    private string cursorString = string.Empty;

    private int cursorLocation;
    

    private void Start()
    {
        if(Text == null)
        {
            Text = GetComponent<TextMeshProUGUI>();
        }
    }

    private void Update()
    {
        if (Time.time - timeStamp >= 0.5)
        {
            timeStamp = Time.time;
            if (cursor == false)
            {
                cursor = true;
                cursorString += "|";
            }
            else
            {
                cursor = false;
                if (cursorString.Length != 0)
                {
                    cursorString = string.Empty;
                }
            }
        }

        Text.text = KeyboardInputStringData.Value + cursorString;
    }

    public void TypeEvent()
    {
        string output = KeyboardInputStringData.Value;


    }

    public void MoveCursor()
    {
        string output = KeyboardInputStringData.Value;

        cursorLocation = output.Length;


    }

    private string FormatOutPutString()
    {
        string keyboardEntry = KeyboardInputStringData.Value;

        string returnedString = string.Empty;

        string firstHalf;
        string secondHalf;

        int firstIndex = keyboardEntry.Length - cursorLocation;

        //firstHalf = keyboardEntry.Substring(0, )

        return returnedString;
    }
}