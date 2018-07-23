using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using NS_Annotation.NS_SO;


public class RightVRControllerInput : VRInput
{
    public enum RightControllerState
    {
        Menu,
        Typing
    }

    public enum QuadrantDirections
    {
        Right,
        Top,
        Left,
        Bottom,
        None = -1
    }

    public QuadrantDirections QuadrantSelection;

    public delegate void OnStateChangeDelegate();
    public event OnStateChangeDelegate OnStateChangeEvent;

    public VoidEvent DoneTypingEvent;
    public StringVariable ControllerText;

    public RightControllerState ControllerState = RightControllerState.Menu;
    public RightControllerState State
    {
        get
        {
            return ControllerState;
        }
        set
        {
            ControllerState = value;
            if (OnStateChangeEvent != null)
                OnStateChangeEvent();
        }
    }

    public LeftVRControllerInput LeftControllerInput;

    public Dictionary<int, List<string>> MenuOptions = new Dictionary<int, List<string>>()
    {
        { 0, new List<string> { "[Tab]" } },
        { 1, new List<string> { "[Enter]" } },
        { 2, new List<string> { "[Del]" } },
        { 3, new List<string> { "[Space]" } }
    };

    private bool isSelected = false;

    private void Start()
    {
        if (VRControllerManager == null)
        {
            VRControllerManager = FindObjectOfType<VRControllerInputManager>();
        }

        OnStateChangeEvent += OnControllerStateChange;
        State = RightControllerState.Menu;
        ChildScale = CircleColorParent.transform.GetChild(0).transform.localScale;
    }

    void Update()
    {
        // Get inputs
        ThumbPos.x = Input.GetAxis("RightStickHorizontal");
        ThumbPos.y = Input.GetAxis("RightStickVertical");

        if (Input.GetAxis("RightIndexTrigger") > 0.5f && isSelected == false)
        {
            isSelected = true;
            Debug.Log("Trigger Down!");
            Select();
        }

        if (Input.GetAxis("RightIndexTrigger") < 0.5f && isSelected == true)
        {
            isSelected = false;
            Debug.Log("Trigger Up!");
        }

        StickQuadrantDirection = CalculateStickDir(ThumbPos);
        QuadrantSelection = (QuadrantDirections)StickQuadrantDirection;

        //Something changed
        if (StickQuadrantDirection != PreviousStickPosition)
        {
            //if (StickQuadrantDirection != -1 && PreviousStickPosition <= -1)
            //{
            //    Select();
            //}

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

    public override void Select()
    {
        switch (ControllerState)
        {
            case RightControllerState.Menu:
                if (StickQuadrantDirection != -1)
                {
                    if (MenuOptions.ContainsKey(StickQuadrantDirection))
                    {
                        string test = GetCharactersFromList(MenuOptions[StickQuadrantDirection]);
                        if (test == "[Del]")
                        {
                            string text = TestText.text;
                            if (text != string.Empty)
                                text = text.Remove(text.Length - 1);
                            TestText.text = text;
                        }
                        else if (test == "[Space]")
                        {
                            TestText.text += " ";
                        }
                        else if (test == "[Tab]")
                        {

                        }
                        else if (test == "[Enter]")
                        {
                            ControllerText.Value = TestText.text += "\n";
                            DoneTypingEvent.Publish();
                        }
                    }
                }
                break;
            case RightControllerState.Typing:
                if (LeftControllerInput.StickQuadrantDirection != -1 && StickQuadrantDirection != -1)
                {
                    List<string> charactersFromLeftStickSelection = new List<string>(LeftControllerInput.GetListFromStickDirection());

                    charactersFromLeftStickSelection.Reverse();

                    if (PreviousStickPosition < charactersFromLeftStickSelection.Count)
                    {
                        TestText.text += charactersFromLeftStickSelection[StickQuadrantDirection];
                    }
                }
                break;
            default:
                break;
        }
    }

    private void OnControllerStateChange()
    {
        UpdateWheelText();
    }

    private void UpdateWheelText()
    {
        switch (ControllerState)
        {
            case RightControllerState.Menu:
                AssignMenusOnCircle();
                break;
            case RightControllerState.Typing:
                AssignCharactersToCircle();
                break;
            default:
                break;
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
            //TestText.text = GetCharactersFromList(GetListFromFromIndex(0));
        }
        else
        {
            //TestText.text = string.Empty;
        }
    }

    public void AssignCharactersToCircle()
    {
        ClearCharactersOnCircle();

        if (LeftControllerInput.StickQuadrantDirection != -1)
        {
            List<string> charactersFromLeftStickSelection = new List<string>(LeftControllerInput.GetListFromStickDirection());

            charactersFromLeftStickSelection.Reverse();

            for (int i = 0; i < charactersFromLeftStickSelection.Count; i++)
            {
                Text child = CircleTextParent.transform.GetChild(i).GetComponent<Text>();
                child.text = charactersFromLeftStickSelection[i];
            }
        }
    }

    public void AssignMenusOnCircle()
    {
        ClearCharactersOnCircle();

        for (int i = 0; i < MenuOptions.Count; i++)
        {
            Text child = CircleTextParent.transform.GetChild(i).GetComponent<Text>();
            child.text = GetCharactersFromList(GetListFromFromIndex(i));
        }
    }

    public void ClearCharactersOnCircle()
    {
        for (int i = 0; i < CircleTextParent.transform.childCount; i++)
        {
            Text child = CircleTextParent.transform.GetChild(i).GetComponent<Text>();
            child.text = string.Empty;
        }
    }

    public List<string> GetListFromFromIndex(int index)
    {
        return MenuOptions[index];
    }

    public string GetCharactersFromList(List<string> list)
    {
        string returnedString = string.Empty;

        for (int i = 0; i < list.Count; i++)
        {
            returnedString += list[i];
        }

        return returnedString;
    }
}