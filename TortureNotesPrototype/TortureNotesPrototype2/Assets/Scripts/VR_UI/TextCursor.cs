using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using NS_Annotation.NS_SO;

public class TextCursor : MonoBehaviour
{
    public TextMeshProUGUI Text;
    public StringVariable KeyboardOutputStringData;
    public StringVariable KeyboardInputCharData;
    private string outputString;

    private float timeStamp;
    private bool cursor = false;
    private string cursorString = string.Empty;

    private int cursorCharacterIndex = 0;
    private int totalNumberOfCharacters;


    private void Start()
    {
        if (Text == null)
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            cursorCharacterIndex++;
        }

        //Text.text = KeyboardOutputStringData.Value + cursorString;
        //Text.text = FormatOutputString();

        Text.text = PlaceCursor();
    }

    public void TypeEvent()
    {
        //outputString += KeyboardInputcharData + cursorString;
        //if (cursorCharacterIndex == totalNumberOfCharacters)
        //{
        //    cursorCharacterIndex++;
        //}
        //
        //totalNumberOfCharacters++;
    }

    private string FormatOutputString()
    {
        string keyboardEntry = KeyboardOutputStringData.Value;

        string returnedString = string.Empty;

        int index = totalNumberOfCharacters - (totalNumberOfCharacters - cursorCharacterIndex);
        string firstHalf = keyboardEntry.Substring(0, index);

        string secondHalf = string.Empty;
        if (index != totalNumberOfCharacters)
        {
            secondHalf = keyboardEntry.Substring(index, totalNumberOfCharacters);
        }

        returnedString = firstHalf + cursorString + secondHalf;

        return returnedString;
    }

    private string PlaceCursor()
    {
        string keyboardEntry = KeyboardOutputStringData.Value;

        string firstHalf = keyboardEntry.Substring(0, cursorCharacterIndex);

        string secondHalf = keyboardEntry.Substring(cursorCharacterIndex, keyboardEntry.Length);

        string returnedString = firstHalf + cursorString + secondHalf;

        return returnedString;
    }
}