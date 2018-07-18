using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRInput : MonoBehaviour
{
    Dictionary<int, List<string>> m_LetterCollection = new Dictionary<int, List<string>>()
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

    [SerializeField]
    string m_SelectedChar = string.Empty;

    /// <summary>When a left stick event occurs. Argument is 'true' if past event range</summary>
    public event System.Action<bool> LeftStickEvent = delegate { };
    /// <summary>When a right stick event occurs. Argument is 'true' if past event range</summary>
    public event System.Action<bool> RightStickEvent = delegate { };

    /// <summary>When a left stick directional change occurs</summary>
    public event System.Action<int> LeftStickDirChange = delegate { };
    /// <summary>When a right stick directional change occurs</summary>
    public event System.Action<int> RightStickDirChange = delegate { };

    private void Start()
    {
        LeftStickDirChange += OnLeftStickDirChanged;
        RightStickDirChange += OnRightStickDirChanged;
    }

    void Update()
    {
        // Get inputs
        m_LeftThumbPos.x = Input.GetAxis("LeftStickHorizontal");
        m_LeftThumbPos.y = Input.GetAxis("LeftStickVertical");

        m_RightThumbPos.x = Input.GetAxis("RightStickHorizontal");
        m_RightThumbPos.y = Input.GetAxis("RightStickVertical");

        // Calculate stick directions
        m_LeftStickDir = CalculateStickDir(m_LeftThumbPos);
        m_RightStickDir = CalculateStickDir(m_RightThumbPos);

        // Check changes
        if (m_LeftStickDir != m_PrevLeftStickDir) // Something left stick changed
        {
            LeftStickDirChange(m_LeftStickDir);
        }

        if (m_RightStickDir != m_PrevRightStickDir) // Something right stick changed
        {
            RightStickDirChange(m_RightStickDir);
        }

        // Reset previous
        m_PrevLeftStickDir = m_LeftStickDir;
        m_PrevRightStickDir = m_RightStickDir;
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
            if (a_DirToCalc.x <= 0) // 1 - 4
            {
                if (a_DirToCalc.y >= 0) // 3 - 4
                {
                    if (Mathf.Abs(a_DirToCalc.y) >= Mathf.Abs(a_DirToCalc.x)) return 4;
                    else return 3;
                }
                else // 1 - 2
                {
                    if (Mathf.Abs(a_DirToCalc.y) >= Mathf.Abs(a_DirToCalc.x)) return 1;
                    else return 2;
                }
            }
            else // 5 - 0
            {
                if (a_DirToCalc.y >= 0) // 5 - 6
                {
                    if (Mathf.Abs(a_DirToCalc.y) >= Mathf.Abs(a_DirToCalc.x)) return 5;
                    else return 6;
                }
                else // 7 - 0
                {
                    if (Mathf.Abs(a_DirToCalc.y) >= Mathf.Abs(a_DirToCalc.x)) return 0;
                    else return 7;
                }
            }
        }
    }

    void OnLeftStickDirChanged(int a_NewDir)
    {
        //Debug.Log(string.Format("Left stick direction changed to {0}", a_NewDir));

        if (m_LeftStickDir > -1 && m_RightStickDir > -1)
        {
            m_SelectedChar = m_LetterCollection[m_LeftStickDir][m_RightStickDir];
        }
        else
        {
            m_SelectedChar = string.Empty;
        }
    }

    void OnRightStickDirChanged(int a_NewDir)
    {
        //Debug.Log(string.Format("Right stick direction changed to {0}", a_NewDir));

        if (m_LeftStickDir > -1 && m_RightStickDir > -1)
        {
            m_SelectedChar = m_LetterCollection[m_LeftStickDir][m_RightStickDir];
        }
        else
        {
            m_SelectedChar = string.Empty;
        }
    }
}