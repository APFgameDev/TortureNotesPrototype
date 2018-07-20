using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeftVRControllerInput : VRInput
{
    public Dictionary<int, List<string>> LetterCollection = new Dictionary<int, List<string>>()
    {
        { 0, new List<string> { "a", "b", "c" } },
        { 1, new List<string> { "d", "e", "f" } },
        { 2, new List<string> { "g", "h", "i" } },  
        { 3, new List<string> { "j", "k", "l" } },
        { 4, new List<string> { "m", "n", "o" } },
        { 5, new List<string> { "p", "q", "r", "s" } },
        { 6, new List<string> { "t", "u", "v" } },
        { 7, new List<string> { "w", "x", "y", "z" } },
    };

    public delegate void OnCapsChangeDelegate();
    public event OnCapsChangeDelegate OnCapsChangeEvent;

    public RightVRControllerInput RightControllerInput;

    private bool IsCaps;
    public bool Caps
    {
        get
        {
            return IsCaps;
        }
        set
        {
            IsCaps = value;
            if (OnCapsChangeEvent != null)
                OnCapsChangeEvent();
        }
    }

    private void Start()
    {
        if(VRControllerManager == null)
        {
            VRControllerManager = FindObjectOfType<VRControllerInputManager>();
        }

        OnCapsChangeEvent += OnCapsChange;
        ChildScale = CircleColorParent.transform.GetChild(0).transform.localScale;
        
        //Populate the left stick visual
        for (int i = 0; i < CircleTextParent.transform.childCount; i++)
        {
            Text child = CircleTextParent.transform.GetChild(i).GetComponent<Text>();

            child.text = GetCharactersFromList(LetterCollection[i]);
        }
    }

    void Update()
    {
        // Get inputs
        ThumbPos.x = Input.GetAxis("LeftStickHorizontal");
        ThumbPos.y = Input.GetAxis("LeftStickVertical");

        if (Input.GetAxis("LeftIndexTrigger") > 0.5f)
        {
            Caps = true;
        }
        else
        {
            Caps = false;
        }

        StickQuadrantDirection = CalculateStickDir(ThumbPos);

        //Something changed
        if (StickQuadrantDirection != PreviousStickPosition)
        {
            StickDirectionChanged(StickQuadrantDirection);
            StickInput();
        }

        // Reset previous
        PreviousStickPosition = StickQuadrantDirection;

        if (ThumbPos.x != 0.0f || ThumbPos.y != 0.0f)
        {
            CurrentLeftAngle = FindAngle(ThumbPos.x, ThumbPos.y);
            SectionIndex = GetSectionFromAngle(CurrentLeftAngle);
        }
    }

    /// <summary>
    /// Is called when the stick direction has been changed
    /// </summary>
    /// <param name="newDir"></param>
    protected override void StickDirectionChanged(int newDir)
    {
        if (StickQuadrantDirection > -1)
        {
            TestText.text = GetCharactersFromStickDirection();
            RightControllerInput.State = RightVRControllerInput.RightControllerState.Typing;
        }
        else
        {
            TestText.text = string.Empty;
            RightControllerInput.State = RightVRControllerInput.RightControllerState.Menu;
        }
    }

    private void OnCapsChange()
    {
        if(StickQuadrantDirection != -1)
        {
            for (int i = 0; i < CircleTextParent.transform.childCount; i++)
            {
                Text child = CircleTextParent.transform.GetChild(i).GetComponent<Text>();
                child.text = GetCharactersFromIndex(i);
            }

            RightControllerInput.AssignCharactersToCircle();
        }
    }

    #region String/List Formating

    public List<string> FormatList(List<string> list)
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

    public List<string> ToUpperList(List<string> list)
    {
        List<string> returnedList = list;

        for (int i = 0; i < returnedList.Count; i++)
        {
            returnedList[i] = returnedList[i].ToUpper();
        }

        return returnedList;
    }

    public List<string> ToLowerList(List<string> list)
    {
        List<string> returnedList = list;

        for (int i = 0; i < returnedList.Count; i++)
        {
            returnedList[i] = returnedList[i].ToLower();
        }

        return returnedList;
    }

    public string FormatString(string str)
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

    public List<string> GetListFromStickDirection()
    {
        //return FormatList(LetterCollection[StickQuadrantDirection]);
        return FormatList(LetterCollection[VRControllerManager.GetSectionFromQuadrant(StickQuadrantDirection)]);
    }

    public string GetCharactersFromStickDirection()
    {
        //return GetCharactersFromList(LetterCollection[StickQuadrantDirection]);
        return GetCharactersFromList(LetterCollection[VRControllerManager.GetSectionFromQuadrant(StickQuadrantDirection)]);
    }

    public string GetCharactersFromIndex(int index)
    {
        //return GetCharactersFromList(LetterCollection[index]);
        return GetCharactersFromList(LetterCollection[index]);
    }

    public string GetCharactersFromList(List<string> list)
    {
        string returnedString = string.Empty;

        for (int i = 0; i < list.Count; i++)
        {
            returnedString += list[i];
        }

        returnedString = FormatString(returnedString);

        return returnedString;
    }

    #endregion
}