using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Annotation.SO;
using Annotation.SO.UnityEvents;

public class KeyboardOptions : MonoBehaviour
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right,
        None
    }

    [Header("Mallets")]
    [SerializeField] private GameObject m_LeftMallet;
    [SerializeField] private GameObject m_RightMallet;

    [Header("Sliders")]
    [SerializeField] private Slider m_KeyHorizontalSpaceSlider;
    [SerializeField] private Slider m_KeyVerticalSpaceSlider;

    [Header("If set to true, Right hand is dominant")]
    [SerializeField] private Toggle m_DominantHandToggle;

    [Header("Is the keyboard active?")]
    [SerializeField] private BoolVariable m_KeyboardActive;

    [Header("Laser active bool")]
    [SerializeField] private BoolVariable m_LaserActiveBool;

    [Header("Publish these events when when you Open/Close the menu")]
    [SerializeField] private UnityEventVoidSO m_MenuOpenEvent;
    [SerializeField] private UnityEventVoidSO m_MenuCloseEvent;

    [Header("Keyboard key setup")]
    [SerializeField] private KeyboardKeySetup[] m_KeySetup;

    [Tooltip("Controller Axis")]
    [SerializeField] private Vector2Variable m_LeftJoyStickAxis;
    [SerializeField] private Vector2Variable m_RightJoyStickAxis;

    [Header("How far the stick axis need to be pushed for their input to be used")]
    [SerializeField] [Range(0.0f, 1.0f)] private float m_EventThreshold = 0.9f;

    [Header("Speed that the mallet will move at when using the axis")]
    [SerializeField] [Range(0.01f, 0.3f)] private float m_MalletMoveSpeed = 0.05f;
    [Header("Max Mallet Distance")]
    [SerializeField] [Range(0.0f, 10.0f)] private float m_MaxMalletDistance = 7.0f;
    [Header("Min Mallet Distance")]
    [SerializeField] private float m_MinMalletDistance = 1.0f;

    [Header("Speed that the mallet will scale at when using the axis")]
    [SerializeField] [Range(0.01f, 0.3f)] private float m_MalletScaleSpeed = 0.025f;
    [Header("Max Mallet Scale")]
    [SerializeField] private Vector3 m_MaxMalletScale = new Vector3(2.0f, 2.0f, 2.0f);
    [Header("Min Mallet Scale")]
    [SerializeField] private Vector3 m_MinMalletScale = new Vector3(0.01f, 0.01f, 0.01f);

    [Header("Row Key Objects")]
    [SerializeField] private GameObject[] m_TopRows;
    [SerializeField] private GameObject[] m_BottomRows;
    [SerializeField] private GameObject m_CenterRowObject;


    [Header("Offset that is added to the keys for spacing")]
    public float m_VerticalOffset = 0.11f;

    [Header("Value added to the offset based on the sliders value")]
    public float m_VerticalOffsetAdd = 0.02f;


    private void Start()
    {
        if (m_KeySetup != null)
        {
            m_KeySetup = Resources.FindObjectsOfTypeAll<KeyboardKeySetup>();
        }

        if(m_LeftMallet == null)
        {
            m_LeftMallet = GameObject.Find("Left Mallet");
        }

        if (m_RightMallet == null)
        {
            m_RightMallet = GameObject.Find("Right Mallet");
        }
    }

    void Update()
    {
        //Only do this code if the keyboard is active
        if (m_KeyboardActive.Value != true)
        {
            return;
        }

        ProcessStickAxis();
    }

    public void UpdateHorizontalKeySpace()
    {
        if (m_KeyboardActive.Value == true)
        {
            foreach (KeyboardKeySetup keySetup in m_KeySetup)
            {
                //Set the spacing between the keys
                keySetup.SetHorzontalSpacingForKeysWithValue((int)m_KeyHorizontalSpaceSlider.value);
            }
        }
    }

    private void UpdateVerticalSpacing()
    {
        float offset = m_VerticalOffset;

        for (int i = 0; i < m_KeyVerticalSpaceSlider.value; i++)
        {
            offset += m_VerticalOffsetAdd;
        }

        Vector3 previousPos = m_CenterRowObject.transform.localPosition;

        //Add offset to top rows
        for (int i = 0; i < m_TopRows.Length; i++)
        {
            Vector3 newPos = previousPos;

            if (i == 0)
            {
                newPos.z += offset / 2;
            }
            else
            {
                newPos.z += offset;
            }

            m_TopRows[i].transform.localPosition = newPos;
            previousPos = newPos;
        }

        previousPos = m_CenterRowObject.transform.localPosition;

        //Add offset to top rows
        for (int i = 0; i < m_BottomRows.Length; i++)
        {
            Vector3 newPos = previousPos;

            if(i == 0)
            {
                newPos.z -= offset / 2;
            }
            else
            {
                newPos.z -= offset;
            }


            m_BottomRows[i].transform.localPosition = newPos;
            previousPos = newPos;
        }


    }

    public void UpdateVerticalKeySpace()
    {
        if (m_KeyboardActive.Value == true)
        {
            UpdateVerticalSpacing();

            //Update the horizontal space to avoid overlapping keys
            UpdateHorizontalKeySpace();
        }
    }

    private void ProcessStickAxis()
    {
        //Process Mallet Distance
        if (m_DominantHandToggle.isOn) // IsOn = Right Hand Dominant
        {
            AdjustMalletDistance(m_RightJoyStickAxis.Value);
            AdjustMalletScale(m_LeftJoyStickAxis.Value);
        }
        else
        {
            AdjustMalletDistance(m_LeftJoyStickAxis.Value);
            AdjustMalletScale(m_RightJoyStickAxis.Value);
        }
    }

    private void AdjustMalletScale(Vector2 axis)
    {
        //If there is no Direction frome the axis, dont do anything
        if (GetDirectionFromAxis(axis) == Direction.None)
        {
            return;
        }

        Vector3 newScale = m_LeftMallet.transform.localScale;

        if (GetDirectionFromAxis(axis) == Direction.Up)
        {
            newScale += new Vector3(m_MalletScaleSpeed, m_MalletScaleSpeed, m_MalletScaleSpeed);

            if (newScale.x > m_MaxMalletScale.x || newScale.y > m_MaxMalletScale.y || newScale.z > m_MaxMalletScale.z)
            {
                newScale = m_MaxMalletScale;

            }
        }
        else if (GetDirectionFromAxis(axis) == Direction.Down)
        {
            newScale -= new Vector3(m_MalletScaleSpeed, m_MalletScaleSpeed, m_MalletScaleSpeed);

            if (newScale.x < m_MinMalletScale.x || newScale.y < m_MinMalletScale.y || newScale.z < m_MinMalletScale.z)
            {
                newScale = m_MinMalletScale;
            }
        }

        m_LeftMallet.transform.localScale = newScale;
        m_RightMallet.transform.localScale = newScale;
    }

    private void AdjustMalletDistance(Vector2 axis)
    {
        //If there is no Direction frome the axis, dont do anything
        if (GetDirectionFromAxis(axis) == Direction.None)
        {
            return;
        }

        Vector3 newPos = m_LeftMallet.transform.localPosition;

        if (GetDirectionFromAxis(axis) == Direction.Up)
        {
            newPos.z += m_MalletMoveSpeed;

            if (newPos.z > m_MaxMalletDistance)
            {
                newPos.z = m_MaxMalletDistance;
            }
        }
        else if (GetDirectionFromAxis(axis) == Direction.Down)
        {
            newPos.z -= m_MalletMoveSpeed;

            if (newPos.z < m_MinMalletDistance)
            {
                newPos.z = m_MinMalletDistance;
            }
        }

        m_LeftMallet.transform.localPosition = newPos;
        m_RightMallet.transform.localPosition = newPos;
    }

    private Direction GetDirectionFromAxis(Vector2 axis)
    {
        if (axis.y > m_EventThreshold)       //Up
        {
            return Direction.Up;
        }
        else if (axis.y < -m_EventThreshold)//Down
        {
            return Direction.Down;
        }
        else if (axis.x < -m_EventThreshold) //Left
        {
            return Direction.Left;
        }
        else if (axis.x > m_EventThreshold) //Right
        {
            return Direction.Right;
        }
        else
        {
            return Direction.None;
        }
    }

    public void TurnOn()
    {
        gameObject.SetActive(true);

        m_MenuOpenEvent.UnityEvent.Invoke();
    }
}
