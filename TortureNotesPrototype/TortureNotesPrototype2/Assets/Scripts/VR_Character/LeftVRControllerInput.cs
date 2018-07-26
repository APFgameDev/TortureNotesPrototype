using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NS_Annotation.NS_SO;
using TMPro;

public class LeftVRControllerInput : VRController
{
    #region Public

    public int NumberStartIndex = 0;

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

    public Dictionary<int, List<string>> NumberCollection = new Dictionary<int, List<string>>()
    {
        { 0, new List<string> { "2" } },
        { 1, new List<string> { "3" } },
        { 2, new List<string> { "4" } },
        { 3, new List<string> { "" } },
        { 4, new List<string> { "" } },
        { 5, new List<string> { "" } },
        { 6, new List<string> { "0" } },
        { 7, new List<string> { "1" } },
    };

    public Dictionary<int, List<string>> SymbolCollection = new Dictionary<int, List<string>>()
    {
        { 0, new List<string> { "!" } },
        { 1, new List<string> { "," } },
        { 2, new List<string> { "." } },
        { 3, new List<string> { "^" } },
        { 4, new List<string> { "%" } },
        { 5, new List<string> { "$" } },
        { 6, new List<string> { "#" } },
        { 7, new List<string> { "@" } }
    };

    public GameObject ControllerCanvasLocation;

    #endregion

    #region Private



    #endregion

    public Dictionary<int, List<string>> CurrentDictionary = new Dictionary<int, List<string>>();

    public override void Start()
    {
        CurrentDictionary = GetDictionaryFromInputState();

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
        ThumbPos.x = Input.GetAxis("LeftStickHorizontal");
        ThumbPos.y = Input.GetAxis("LeftStickVertical");

        StickQuadrantDirection = CalculateStickDir(ThumbPos);

        //Something changed
        if (StickQuadrantDirection != PreviousStickPosition)
        {
            StickDirectionChanged(StickQuadrantDirection);
            StickInput();
        }

        //Something changed
        if (StickQuadrantDirection != PreviousStickPosition)
        {
            if (State != ControllerState.Menu && (InputState == ControllerInputState.Number || InputState == ControllerInputState.Symbol))
            {
                if (StickQuadrantDirection != -1 && PreviousStickPosition <= -1)
                {
                    Select();
                }
            }

            StickDirectionChanged(StickQuadrantDirection);
            StickInput();
        }

        // Reset previous
        PreviousStickPosition = StickQuadrantDirection;
    }

    #region Selection Functions

    public override void Select()
    {
        if (StickQuadrantDirection != -1)
        {
            if (StickQuadrantDirection < CurrentDictionary.Count)
            {
                switch (InputState)
                {
                    case ControllerInputState.Symbol:
                        //TextObject.text += GetCharactersFromStickDirection(CurrentDictionary);
                        ControllerText.Value += GetCharactersFromStickDirection(CurrentDictionary);
                        TypingEvent.Publish();
                        break;
                    case ControllerInputState.Number:
                        //TextObject.text += GetCharactersFromStickDirection(NumberCollection);
                        ControllerText.Value += GetCharactersFromStickDirection(NumberCollection);
                        TypingEvent.Publish();

                        break;
                    default:
                        break;
                }
            }
        }
    }

    #endregion    

    #region OnEventChange Functions

    public override void OnCapsChange()
    {
        base.OnCapsChange();

        if (StickQuadrantDirection != -1)
        {
            for (int i = 0; i < CircleTextParent.transform.childCount; i++)
            {
                TextMeshProUGUI child = CircleTextParent.transform.GetChild(i).GetComponent<TextMeshProUGUI>();
                child.text = GetCharactersFromIndex(i, CurrentDictionary);
            }
        }

        UpdateVisuals();
        UpdateDictionary();
    }

    public override void OnStateChange()
    {
        base.OnStateChange();

        UpdateDictionary();
        UpdateVisuals();
    }

    public override void OnInputStateChange()
    {
        base.OnInputStateChange();
        UpdateDictionary();
        UpdateVisuals();
    }

    private void UpdateDictionary()
    {
        CurrentDictionary = GetDictionaryFromInputState();
    }

    public override void UpdateVisuals()
    {
        ClearCharactersOnCircle();

        switch (State)
        {
            case ControllerState.Idle:
                AssignCharactersToCircle();
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

    public override void AssignLettersToCircle()
    {
        for (int i = 0; i < CircleTextParent.transform.childCount; i++)
        {
            TextMeshProUGUI child = CircleTextParent.transform.GetChild(i).GetComponent<TextMeshProUGUI>();

            if (CurrentDictionary.ContainsKey(i))
            {
                child.text = GetCharactersFromList(CurrentDictionary[i]);
            }
        }
    }

    public override void AssignCharactersToCircle()
    {
        switch (InputState)
        {
            case ControllerInputState.Alphabet:
                AssignDictionaryToTextChildren(CurrentDictionary);
                break;
            case ControllerInputState.Symbol:
                AssignDictionaryToTextChildren(CurrentDictionary);
                break;
            case ControllerInputState.Number:
                AssignNumbersToCircle();
                break;
            default:
                break;
        }
    }

    private void AssignNumbersToCircle()
    {
        ClearCharactersOnCircle();
        
        int childcount = CircleTextParent.transform.childCount;
        
        for (int i = 0; i < childcount; i++)
        {
            int currentIndex = (i + NumberStartIndex) % childcount;
            
            TextMeshProUGUI child = CircleTextParent.transform.GetChild(currentIndex).GetComponent<TextMeshProUGUI>();

            if (child != null)
            {
                if (CurrentDictionary.ContainsKey(currentIndex))
                {
                    child.text = GetCharactersFromList(CurrentDictionary[currentIndex]);
                }
            }
        }
    }

    public override void AssignSelectionOptionsToCircle()
    {

    }

    #endregion

    #region Utility Functions

    private Dictionary<int, List<string>> GetDictionaryFromInputState()
    {
        switch (InputState)
        {
            case ControllerInputState.Alphabet:
                return LetterCollection;
            case ControllerInputState.Symbol:
                return SymbolCollection;
            case ControllerInputState.Number:
                return NumberCollection;
            default:
                return null;
        }
    }

    #endregion
}