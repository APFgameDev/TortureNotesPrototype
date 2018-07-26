using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NS_Annotation.NS_SO;
using TMPro;

public class RightVRControllerInput : VRController
{
    #region Public

    public enum QuadrantDirections
    {
        Right,
        Top,
        Left,
        Bottom,
        None = -1
    }

    public QuadrantDirections QuadrantSelection;
    public VoidEvent DoneTypingEvent;

    public LeftVRControllerInput LeftControllerInput;
    public GameObject ControllerCanvasLocation;
    public GameObject BackgroundCanvas;

    public Dictionary<int, List<string>> MenuCollection = new Dictionary<int, List<string>>()
    {
        { 0, new List<string> { "[Space]" } },  //Right
        { 1, new List<string> { "" } },         //Top Right
        { 2, new List<string> { "[Done]" } },   //Top
        { 3, new List<string> { "" } },         //Top Left
        { 4, new List<string> { "[Del]" } },    //Left
        { 5, new List<string> { "" } },         //Bottom Left
        { 6, new List<string> { "[Enter]" } },  //Bottom
        { 7, new List<string> { "" } }          //Bottom Right
    };

    public Dictionary<int, List<string>> NumberCollection = new Dictionary<int, List<string>>()
    {
        { 0, new List<string> { "9" } },
        { 1, new List<string> { "8" } },
        { 2, new List<string> { "7" } },
        { 3, new List<string> { "6" } },
        { 4, new List<string> { "5" } },
        { 5, new List<string> { "" } },
        { 6, new List<string> { "" } },
        { 7, new List<string> { "" } },
    };

    public Dictionary<int, List<string>> SymbolCollection = new Dictionary<int, List<string>>()
    {
        { 0, new List<string> { "-" } },
        { 1, new List<string> { "_" } },
        { 2, new List<string> { ")" } },
        { 3, new List<string> { "(" } },
        { 4, new List<string> { "*" } },
        { 5, new List<string> { "&" } },
        { 6, new List<string> { "=" } },
        { 7, new List<string> { "+" } }
    };

    public Dictionary<int, List<string>> CurrentDictionary;

    #endregion

    #region Private

    private bool isSelected = false;

    #endregion

    public override void Start()
    {
        base.Start();

        if (VRControllerManager == null)
        {
            VRControllerManager = FindObjectOfType<VRControllerInputManager>();
        }
        
        ChildScale = CircleColorParent.transform.GetChild(0).transform.localScale;
    }

    void Update()
    {
        // Get inputs
        ThumbPos.x = Input.GetAxis("RightStickHorizontal");
        ThumbPos.y = Input.GetAxis("RightStickVertical");

        StickQuadrantDirection = CalculateStickDir(ThumbPos);

        //Something changed
        if (StickQuadrantDirection != PreviousStickPosition)
        {
            if (StickQuadrantDirection != -1 && PreviousStickPosition <= -1)
            {
                Select();
            }

            StickDirectionChanged(StickQuadrantDirection);
            StickInput();
        }

        // Reset previous
        PreviousStickPosition = StickQuadrantDirection;
    }

    #region OnEventChange Functions

    public override void OnCapsChange()
    {
        base.OnCapsChange();

        UpdateVisuals();
    }

    public override void OnStateChange()
    {
        base.OnStateChange();

        UpdateVisuals();
    }

    public override void OnInputStateChange()
    {
        base.OnInputStateChange();

        UpdateVisuals();
    }

    #endregion

    #region Selection Functions

    public override void Select()
    {
        switch (State)
        {
            case ControllerState.Idle:
                TypingSelect();
                break;
            case ControllerState.Typing:
                TypingSelect();
                break;
            case ControllerState.Menu:
                MenuSelect();
                break;
            default:
                break;
        }
    }

    #region Typing Selection

    private void TypingSelect()
    {
        switch (InputState)
        {
            case ControllerInputState.Alphabet:
                AlphabetSelect();
                //ControllerText.Value = TextObject.text;
                break;
            case ControllerInputState.Symbol:
                SymbolSelect();
                //ControllerText.Value = TextObject.text;
                break;
            case ControllerInputState.Number:
                NumberSelect();
                //ControllerText.Value = TextObject.text;
                break;
            default:
                break;
        }

        TypingEvent.Publish();
    }

    private void AlphabetSelect()
    {
        if (LeftControllerInput.StickQuadrantDirection != -1 && StickQuadrantDirection != -1)
        {
            List<string> charactersFromLeftStickSelection = new List<string>(LeftControllerInput.GetListFromStickDirection(LeftControllerInput.CurrentDictionary));

            charactersFromLeftStickSelection.Reverse();

            if (StickQuadrantDirection < charactersFromLeftStickSelection.Count)
            {
                //TextObject.text += charactersFromLeftStickSelection[StickQuadrantDirection];
                ControllerText.Value += charactersFromLeftStickSelection[StickQuadrantDirection];
            }
        }
    }

    private void NumberSelect()
    {
        if (StickQuadrantDirection != -1)
        {
            if (StickQuadrantDirection < NumberCollection.Count)
            {
                //Hard coded to use the first index. For now
                //TextObject.text += NumberCollection[StickQuadrantDirection][0];
                ControllerText.Value += NumberCollection[StickQuadrantDirection][0];
            }
        }
    }

    private void SymbolSelect()
    {
        if (StickQuadrantDirection != -1)
        {
            if (StickQuadrantDirection < SymbolCollection.Count)
            {
                //Hard coded to use the first index. For now
                //TextObject.text += SymbolCollection[StickQuadrantDirection][0];
                ControllerText.Value += SymbolCollection[StickQuadrantDirection][0];
            }
        }
    }

    #endregion

    private void IdleSelect()
    {

    }

    private void MenuSelect()
    {
        if (StickQuadrantDirection != -1)
        {
            if (MenuCollection.ContainsKey(StickQuadrantDirection))
            {
                string test = GetCharactersFromList(MenuCollection[StickQuadrantDirection]);
                if (test == "[Del]" || test == "[del]" || test == "[DEL]")
                {
                    string text = ControllerText.Value;
                    if (text != string.Empty)
                        text = text.Remove(text.Length - 1);
                    //TextObject.text = text;
                    ControllerText.Value = text;
                }
                else if (test == "[Space]" || test == "[space]" || test == "[SPACE]")
                {
                    //TextObject.text += " ";
                    ControllerText.Value += " ";
                }
                else if (test == "[Done]" || test == "[done]" || test == "[DONE]")
                {
                    DoneTypingEvent.Publish();
                }
                else if (test == "[Enter]" || test == "[enter]" || test == "[ENTER]")
                {
                    //ControllerText.Value = TextObject.text += "\n";
                    ControllerText.Value += "\n";
                }
            }
        }
    }

    #endregion    

    #region Assigning Characters To Visual

    public override void AssignCharactersToCircle()
    {
        switch (InputState)
        {
            case ControllerInputState.Alphabet:
                AssignLettersToCircle();
                break;
            case ControllerInputState.Symbol:
                AssignDictionaryToTextChildren(SymbolCollection);
                break;
            case ControllerInputState.Number:
                AssignDictionaryToTextChildren(NumberCollection);
                break;
            default:
                break;
        }
    }

    public override void AssignLettersToCircle()
    {
        ClearCharactersOnCircle();

        if (LeftControllerInput.StickQuadrantDirection != -1)
        {
            List<string> charactersFromLeftStickSelection = new List<string>(LeftControllerInput.GetListFromStickDirection(LeftControllerInput.CurrentDictionary));

            charactersFromLeftStickSelection.Reverse();

            for (int i = 0; i < charactersFromLeftStickSelection.Count; i++)
            {
                TextMeshProUGUI child = CircleTextParent.transform.GetChild(i).GetComponent<TextMeshProUGUI>();
                child.text = charactersFromLeftStickSelection[i];
            }
        }
    }

    public void AssignMenusOnCircle()
    {
        ClearCharactersOnCircle();

        for (int i = 0; i < MenuCollection.Count; i++)
        {
            TextMeshProUGUI child = CircleTextParent.transform.GetChild(i).GetComponent<TextMeshProUGUI>();
            child.text = GetCharactersFromList(GetListFromFromIndex(i, MenuCollection), false);
        }
    }

    #endregion

    #region Utility Functions

    public void EnableDisableBackgroundCanvas()
    {
        BackgroundCanvas.SetActive(!BackgroundCanvas.activeSelf);
    }

    public Dictionary<int, List<string>> GetDictionaryFromControllerState()
    {
        switch (State)
        {
            case ControllerState.Idle:
                return null;
            case ControllerState.Typing:
                switch (InputState)
                {
                    case ControllerInputState.Alphabet:
                        return null;
                    case ControllerInputState.Symbol:
                        return SymbolCollection;
                    case ControllerInputState.Number:
                        return NumberCollection;
                    default:
                        return null;
                }
            case ControllerState.Menu:
                return MenuCollection;
            default:
                return null;
        }
    }

    public override void UpdateVisuals()
    {
        switch (State)
        {
            case ControllerState.Idle:
                AssignCharactersToCircle();
                break;
            case ControllerState.Typing:
                AssignCharactersToCircle();
                break;
            case ControllerState.Menu:
                AssignMenusOnCircle();
                break;
            default:
                break;
        }
    }

    #endregion
}