using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NS_Annotation.NS_SO;

public class VRController : VRInput
{
    #region Public
    
    [Range(0f, 1f)]
    public float m_EventRadius = 0.8f;
    public Vector2 ThumbPos = Vector2.zero;
    public float SectionAngle;
    public int StickQuadrantDirection = -1;
    public VRControllerInputManager VRControllerManager;

    public GameObject CircleColorParent;
    public GameObject CircleTextParent;

    public VoidEvent TypingEvent;
    public StringVariable ControllerText;



    #region Enums

    public enum ControllerInputState
    {
        Alphabet,
        Symbol,
        Number
    }

    public enum ControllerState
    {
        Idle,
        Typing,
        Menu
    }

    #endregion

    #region Event / Event Variables

    public delegate void OnStateChangeDelegate();
    public event OnStateChangeDelegate OnStateChangeEvent;
    private ControllerState CurrentState = ControllerState.Idle;
    public ControllerState State
    {
        get
        {
            return CurrentState;
        }
        set
        {
            CurrentState = value;
            if (OnStateChangeEvent != null)
                OnStateChangeEvent();
        }
    }

    public delegate void OnInputStateChangeDelegate();
    public event OnInputStateChangeDelegate OnInputStateChangeEvent;
    private ControllerInputState CurrentInputState = ControllerInputState.Alphabet;
    public ControllerInputState InputState
    {
        get
        {
            return CurrentInputState;
        }
        set
        {
            CurrentInputState = value;
            if (OnInputStateChangeEvent != null)
                OnInputStateChangeEvent();
        }
    }

    public delegate void OnCapsChangeDelegate();
    public event OnCapsChangeDelegate OnCapsChangeEvent;
    private bool Caps;
    public bool IsCaps
    {
        get
        {
            return Caps;
        }
        set
        {
            Caps = value;
            if (OnCapsChangeEvent != null)
                OnCapsChangeEvent();
        }
    }

    #endregion

    #endregion

    #region Protected

    protected Vector3 ChildScale;
    protected int PreviousStickPosition;

    #endregion

    public virtual void Start()
    {
        OnStateChangeEvent += OnStateChange;
        OnInputStateChangeEvent += OnInputStateChange;
        OnCapsChangeEvent += OnCapsChange;

        State = ControllerState.Idle;
        InputState = ControllerInputState.Alphabet;

        ControllerText.Value = string.Empty;
    }

    #region StateChange Functions

    public virtual void OnStateChange()
    {
        //Debug.Log("State changed! Current State: " + State);
    }

    public virtual void OnInputStateChange()
    {
        //Debug.Log("Input state changed! Current Input State: " + InputState);

    }

    public virtual void OnCapsChange()
    {
        //Debug.Log("Caps Changed! Current Caps State: " + IsCaps);
    }

    #endregion

    #region Update Visual Functions

    public virtual void UpdateVisuals()
    {
        switch (State)
        {
            case ControllerState.Idle:
                AssignLettersToCircle();
                break;
            case ControllerState.Typing:
                AssignCharactersToCircle();
                break;
            case ControllerState.Menu:
                AssignSelectionOptionsToCircle();
                break;
            default:
                break;
        }
    }

    public virtual void AssignLettersToCircle()
    {

    }

    public virtual void AssignCharactersToCircle()
    {

    }


    public virtual void AssignSelectionOptionsToCircle()
    {

    }

    public void AssignDictionaryToTextChildren(Dictionary<int, List<string>> dictionary)
    {
        ClearCharactersOnCircle();

        for (int i = 0; i < CircleTextParent.transform.childCount; i++)
        {
            TextMeshProUGUI child = CircleTextParent.transform.GetChild(i).GetComponent<TextMeshProUGUI>();

            if (dictionary.ContainsKey(i))
            {
                child.text = GetCharactersFromList(dictionary[i]);
            }
        }
    }

    /// <summary>
    /// Will loop through all of the text children in the text parent and clear their text components
    /// </summary>
    public virtual void ClearCharactersOnCircle()
    {
        for (int i = 0; i < CircleTextParent.transform.childCount; i++)
        {
            TextMeshProUGUI child = CircleTextParent.transform.GetChild(i).GetComponent<TextMeshProUGUI>();
            child.text = string.Empty;
        }
    }

    #endregion

    #region String/List Formating

    /// <summary>
    /// Will format the list based on if it should be upper case or not
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    public virtual List<string> FormatList(List<string> list)
    {
        if (IsCaps)
        {
            return ToUpperList(list);
        }
        else
        {
            return ToLowerList(list);
        }
    }

    /// <summary>
    /// Will format the list based to be lower case
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    public virtual List<string> ToUpperList(List<string> list)
    {
        List<string> returnedList = list;

        for (int i = 0; i < returnedList.Count; i++)
        {
            returnedList[i] = returnedList[i].ToUpper();
        }

        return returnedList;
    }

    /// <summary>
    /// Will format the list based to be upper case
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    public virtual List<string> ToLowerList(List<string> list)
    {
        List<string> returnedList = list;

        for (int i = 0; i < returnedList.Count; i++)
        {
            returnedList[i] = returnedList[i].ToLower();
        }

        return returnedList;
    }

    /// <summary>
    /// Formats the giving string to be upper case or not
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public virtual string FormatString(string str)
    {
        if (IsCaps)
        {
            return str.ToUpper();
        }
        else
        {
            return str.ToLower();
        }
    }

    #endregion

    #region Get String/List

    /// <summary>
    /// Returns a given list from the dictionary.
    /// Checks if the given index is in the keys from the dictionary
    /// </summary>
    /// <param name="dictionary"></param>
    /// <returns></returns>
    public virtual List<string> GetListFromStickDirection(Dictionary<int, List<string>> dictionary)
    {
        if (dictionary.ContainsKey(VRControllerManager.GetSectionFromQuadrant(StickQuadrantDirection)))
        {
            return FormatList(dictionary[VRControllerManager.GetSectionFromQuadrant(StickQuadrantDirection)]);
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Returns a all characters from the dictionary. 
    /// It composes the given characters into a single string
    /// Checks if the given index is in the keys from the dictionary
    /// </summary>
    /// <param name="dictionary"></param>
    /// <returns></returns>
    public virtual string GetCharactersFromStickDirection(Dictionary<int, List<string>> dictionary)
    {
        if(dictionary.ContainsKey(VRControllerManager.GetSectionFromQuadrant(StickQuadrantDirection)))
        {
            return GetCharactersFromList(dictionary[VRControllerManager.GetSectionFromQuadrant(StickQuadrantDirection)]);
        }
        else
        {
            return string.Empty;
        }
    }

    /// <summary>
    /// Returns a given character from the dictionary.
    /// Checks if the given index is in the keys from the dictionary
    /// </summary>
    /// <param name="index"></param>
    /// <param name="dictionary"></param>
    /// <returns></returns>
    public virtual string GetCharactersFromIndex(int index, Dictionary<int, List<string>> dictionary)
    {
        if(dictionary.ContainsKey(index))
        {
            return GetCharactersFromList(dictionary[index]);
        }
        else
        {
            return string.Empty;
        }
    }

    /// <summary>
    /// Returns a giving string from a list. Then formats the list based on if it should be upper case or not
    /// </summary>
    /// <param name="list"></param>
    /// <param name="formatList"></param>
    /// <returns></returns>
    public virtual string GetCharactersFromList(List<string> list, bool formatList = true)
    {
        string returnedString = string.Empty;

        for (int i = 0; i < list.Count; i++)
        {
            returnedString += list[i];
        }

        if(formatList)
        {
            returnedString = FormatString(returnedString);
        }

        return returnedString;
    }

    /// <summary>
    /// Returns the list from the given dictionary.
    /// Checks if the given index is in the keys from the dictionary
    /// </summary>
    /// <param name="index"></param>
    /// <param name="dictionary"></param>
    /// <returns></returns>
    public virtual List<string> GetListFromFromIndex(int index, Dictionary<int, List<string>> dictionary)
    {
        if(dictionary.ContainsKey(index))
        {
            return dictionary[index];
        }
        else
        {
            return null;
        }
    }

    #endregion

    #region Stick Calulation Functions

    /// <summary>
    /// Used to get the stick direction
    /// </summary>
    /// <param name="a_DirToCalc"></param>
    /// <returns></returns>
    public virtual int CalculateStickDir(Vector2 a_DirToCalc)
    {
        // Below event radius -> no direction assigned
        if (a_DirToCalc.magnitude < m_EventRadius)
        {
            return -1;
        }
        else
        {
            return GetSectionFromAngle(FindAngle(a_DirToCalc.x, a_DirToCalc.y));
        }
    }

    /// <summary>
    /// Is called when an input is detected from the stick
    /// </summary>
    public virtual void StickInput()
    {
        if (StickQuadrantDirection > -1)
        {
            ScaleChildren(StickQuadrantDirection, CircleColorParent, ChildScale, true);
        }
        else
        {
            ScaleChildren(-1, CircleColorParent, ChildScale);
        }
    }

    /// <summary>
    /// Is called when the stick direction has been changed
    /// </summary>
    /// <param name="newDir"></param>
    public virtual void StickDirectionChanged(int newDir)
    {

    }

    /// <summary>
    /// Will return in degrees the angle as 0 - 360
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public float FindAngle(float x, float y)
    {
        float returnedValue = Mathf.Atan2(y, x) * Mathf.Rad2Deg;

        if (returnedValue < 0)
        {
            returnedValue += 360f;
        }

        return returnedValue;
    }

    /// <summary>
    /// Will return the quadrant that the angle is in
    /// </summary>
    /// <param name="angle"></param>
    /// <returns></returns>
    public int GetSectionFromAngle(float angle)
    {
        //To get the offset of the section so that the section angle matches the input background
        float angleOffset = SectionAngle / (360.0f / SectionAngle);
        float newAngle = angle + angleOffset;

        if (newAngle > 360.0f)
        {
            newAngle -= 360.0f;
        }

        int returnedValue = (int)((int)newAngle / SectionAngle);

        return returnedValue;
    }

    #endregion

    /// <summary>
    /// Will scale the children based on the index pass in and the parent object.
    /// Only used for the visual.
    /// </summary>
    /// <param name="index"></param>
    /// <param name="parent"></param>
    /// <param name="changeAlpha"></param>
    private void ScaleChildren(int index, GameObject parent, Vector3 scale, bool changeAlpha = false)
    {
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            if (i == index)
            {
                parent.transform.GetChild(i).localScale = new Vector3(1.0f, 1.0f, 1.0f);
                if (changeAlpha == true)
                {
                    Color selectedAlpha = parent.transform.GetChild(i).GetComponent<Image>().color;
                    selectedAlpha.a = 1.0f;
                    parent.transform.GetChild(i).GetComponent<Image>().color = selectedAlpha;
                }
            }
            else
            {
                parent.transform.GetChild(i).transform.localScale = scale;
                if (changeAlpha == true)
                {
                    Color selectedAlpha = parent.transform.GetChild(i).GetComponent<Image>().color;
                    selectedAlpha.a = 0.60f;
                    parent.transform.GetChild(i).GetComponent<Image>().color = selectedAlpha;
                }
            }
        }
    }
}
