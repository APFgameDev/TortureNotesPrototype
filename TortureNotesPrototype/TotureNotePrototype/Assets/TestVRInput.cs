using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestVRInput : MonoBehaviour
{
    public Text TestText;

    [System.Serializable]
    public struct StringStruct
    {
        public string[] strings;
    }

    public StringStruct[] StringStructs;

    public Dictionary<int, List<string>> m_LetterCollection = new Dictionary<int, List<string>>()
    {
        { 0, new List<string> { string.Empty, string.Empty, "5", "4", "3", "2", "1", string.Empty } },
        { 1, new List<string> { string.Empty, string.Empty, "0", "9", "8", "7", "6", string.Empty } },

        { 2, new List<string> { string.Empty, string.Empty, "A", "B", "C", "D", "E", string.Empty } },
        { 3, new List<string> { string.Empty, string.Empty, "F", "G", "H", "I", "J", string.Empty } },
        { 4, new List<string> { string.Empty, string.Empty, "K", "L", "M", "N", "O", string.Empty } },
        { 5, new List<string> { string.Empty, string.Empty, "P", "Q", "R", "S", "T", string.Empty } },
        { 6, new List<string> { string.Empty, string.Empty, "U", "V", "W", "X", "Y", string.Empty } },

        { 7, new List<string> { string.Empty, string.Empty, "Z", "/", ".", "-", "\'", string.Empty } },
    };

    [SerializeField]
    [Range(0f, 1f)]
    float m_EventRadius = 0.7f;

    public float SectionAngle;
    public int SectionIndex;
    public float CurrentLeftAngle;

    [SerializeField]
    Vector2 m_RightThumbPos = Vector2.zero;
    [SerializeField]
    Vector2 m_LeftThumbPos = Vector2.zero;

    [SerializeField]
    int m_LeftStickDir = -1;
    [SerializeField]
    int m_RightStickDir = -1;

    int m_PrevLeftStickDir = -1;
    int m_PrevRightStickDir = -1;

    /// <summary>When a left stick event occurs. Argument is 'true' if past event range</summary>
    public event System.Action<bool> LeftStickEvent = delegate { };
    /// <summary>When a right stick event occurs. Argument is 'true' if past event range</summary>
    public event System.Action<bool> RightStickEvent = delegate { };

    /// <summary>When a left stick directional change occurs</summary>
    public event System.Action<int> LeftStickDirChange = delegate { };
    /// <summary>When a right stick directional change occurs</summary>
    public event System.Action<int> RightStickDirChange = delegate { };

    public bool IsFreeType = false;

    public GameObject LeftCircleTextParent;
    public GameObject LeftCircleColorParent;
    private Vector3 LeftChildScale;

    public GameObject RightCircleTextParent;
    public GameObject RightCircleColorParent;
    private Vector3 RightChildScale;

    private void Start()
    {
        LeftStickDirChange += OnLeftStickDirChanged;
        RightStickDirChange += OnRightStickDirChanged;

        OnStateChange();

        LeftChildScale = LeftCircleColorParent.transform.GetChild(0).transform.localScale;
        RightChildScale = RightCircleColorParent.transform.GetChild(0).transform.localScale;
    }

    void Update()
    {
        // Get inputs
        m_LeftThumbPos.x = Input.GetAxis("LeftStickHorizontal");
        m_LeftThumbPos.y = Input.GetAxis("LeftStickVertical");

        m_RightThumbPos.x = Input.GetAxis("RightStickHorizontal");
        m_RightThumbPos.y = Input.GetAxis("RightStickVertical");

        m_LeftStickDir = CalculateStickDir(m_LeftThumbPos);
        m_RightStickDir = CalculateStickDir(m_RightThumbPos);

        // Check changes
        if (m_LeftStickDir != m_PrevLeftStickDir) // Something left stick changed
        {
            LeftStickDirChange(m_LeftStickDir);
            LeftStickInput();
        }

        if (m_RightStickDir != m_PrevRightStickDir) // Something right stick changed
        {
            RightStickDirChange(m_RightStickDir);
            RightStickInput();
        }

        // Reset previous
        m_PrevLeftStickDir = m_LeftStickDir;
        m_PrevRightStickDir = m_RightStickDir;

        if (m_LeftThumbPos.x != 0.0f || m_LeftThumbPos.y != 0.0f)
        {
            CurrentLeftAngle = FindAngle(m_LeftThumbPos.x, m_LeftThumbPos.y);
            SectionIndex = GetSectionFromAngle(CurrentLeftAngle);
        }
    }

    private void LeftStickInput()
    {
        if (m_LeftStickDir > -1)
        {
            ScaleChildren(m_LeftStickDir, LeftCircleColorParent, LeftChildScale, true);
        }
        else
        {
            ScaleChildren(-1, LeftCircleColorParent, LeftChildScale);
        }
    }

    private void RightStickInput()
    {
        if (m_RightStickDir > -1)
        {
            ScaleChildren(m_RightStickDir, RightCircleColorParent, RightChildScale, true);
        }
        else
        {
            ScaleChildren(-1, RightCircleColorParent, RightChildScale);
        }
    }

    void OnLeftStickDirChanged(int a_NewDir)
    {
        if (m_LeftStickDir > -1 && m_RightStickDir > -1)
        {
            if (IsFreeType == true)
            {
                TestText.text = GetSingleCharacterFromArrayFromQuadrantSection(m_LeftStickDir, m_RightStickDir);
            }
            else
            {
                TestText.text = GetAllCharacterFromArrayFromQuadrantSection(m_LeftStickDir);
            }
        }
        else
        {
            TestText.text = string.Empty;
        }
    }

    void OnRightStickDirChanged(int a_NewDir)
    {
        if (m_LeftStickDir > -1 && m_RightStickDir > -1)
        {
            if (IsFreeType == true)
            {
                TestText.text = GetSingleCharacterFromArrayFromQuadrantSection(m_LeftStickDir, m_RightStickDir);
            }
            else
            {
                TestText.text = GetAllCharacterFromArrayFromQuadrantSection(m_LeftStickDir);
            }
        }
        else
        {
            TestText.text = string.Empty;
        }
    }

    /// <summary>
    /// Will get called when the user switches the state of the controller. IE: Symbols, Numbers, Alphabet, etc
    /// </summary>
    private void OnStateChange()
    {
        for (int i = 0; i < LeftCircleTextParent.transform.childCount; i++)
        {
            Text child = LeftCircleTextParent.transform.GetChild(i).GetComponent<Text>();
            child.text = GetAllCharactersFromStructIndex(i);
        }
    }

    /// <summary>
    /// Will return in degrees the angle as 0 - 360
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private float FindAngle(float x, float y)
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
    private int GetSectionFromAngle(float angle)
    {
        int returnedValue = (int)((int)angle / SectionAngle);

        return returnedValue;
    }

    /// <summary>
    /// Will return the character from the direction of the left stick and the right stick
    /// </summary>
    /// <param name="leftStickSection"></param>
    /// <param name="rightStickSection"></param>
    /// <returns></returns>
    private string GetSingleCharacterFromArrayFromQuadrantSection(int leftStickSection, int rightStickSection)
    {
        return StringStructs[leftStickSection].strings[rightStickSection];
    }

    /// <summary>
    /// Will return all the characters from the direction of the left stick
    /// </summary>
    /// <param name="leftStickSection"></param>
    /// <returns></returns>
    private string GetAllCharacterFromArrayFromQuadrantSection(int leftStickSection)
    {
        string returnedString = string.Empty;

        for (int i = 0; i < StringStructs[leftStickSection].strings.Length; i++)
        {
            returnedString += StringStructs[leftStickSection].strings[i];
        }

        return returnedString;
    }

    /// <summary>
    /// Will return all the strings from the given index in the struct
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    private string GetAllCharactersFromStructIndex(int index)
    {
        string returnedString = string.Empty;

        for (int i = 0; i < StringStructs[index].strings.Length; i++)
        {
            returnedString += StringStructs[index].strings[i];
        }

        return returnedString;
    }

    int CalculateStickDir(Vector2 a_DirToCalc)
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


//void OnLeftStickDirChanged(int a_NewDir)
//{
//    if (m_LeftStickDir > -1 && m_RightStickDir > -1)
//    {
//        if (IsFreeType == true)
//        {
//            TestText.text = GetSingleCharacterFromArrayFromQuadrantSection(m_LeftStickDir, m_RightStickDir);
//        }
//        else
//        {
//            TestText.text = GetAllCharacterFromArrayFromQuadrantSection(m_LeftStickDir);
//        }
//    }
//    else
//    {
//        TestText.text = string.Empty;
//    }
//}
//
//void OnRightStickDirChanged(int a_NewDir)
//{
//    if (m_LeftStickDir > -1 && m_RightStickDir > -1)
//    {
//        if (IsFreeType == true)
//        {
//            TestText.text = GetSingleCharacterFromArrayFromQuadrantSection(m_LeftStickDir, m_RightStickDir);
//        }
//        else
//        {
//            TestText.text = GetAllCharacterFromArrayFromQuadrantSection(m_LeftStickDir);
//        }
//    }
//    else
//    {
//        TestText.text = string.Empty;
//    }
//}