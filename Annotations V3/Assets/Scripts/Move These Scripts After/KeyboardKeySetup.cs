using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardKeySetup : MonoBehaviour
{
    [Header("The object that is the middle of the left and right keys")]
    [SerializeField] private GameObject m_CenterObject;
    [Header("Is center object a key?")]
    [SerializeField] private bool m_IsCenterObjectAKey;

    [Header("Keys")]
    [SerializeField] private GameObject[] m_LeftSideKeys;
    [SerializeField] private GameObject[] m_RightSideKeys;

    [Header("Offset that is added to the keys for spacing")]
    public float m_HozirontalOffset = 0.22f;

    [Header("Value added to the offset based on the sliders value")]
    public float m_HozirontalOffsetAddPerSliderValue = 0.02f;

    [Header("Use this to see what the spacing is going to look like")]
    [Range(0.0f, 1.0f)] public float m_InspectorKeyHorizontalSpacing = 0.22f;


    public void SetHorzontalSpacingForKeysWithValue(int sliderValue)
    {
        if (m_IsCenterObjectAKey)
        {
            UpdateHorizontalSpacingWithKeyCenterObject(sliderValue);
        }
        else
        {
            UpdateHorizontalSpacingWithCenterObject(sliderValue);
        }
    }

    private void UpdateHorizontalSpacingWithCenterObject(float sliderValue)
    {
        float offset = m_HozirontalOffset;

        for (int i = 0; i < sliderValue; i++)
        {
            offset += m_HozirontalOffsetAddPerSliderValue;
        }

        Vector3 rightPreviousPosition = m_CenterObject.transform.localPosition;
        Vector3 leftPreviousPosition = m_CenterObject.transform.localPosition;

        //Add offset to right side
        for (int i = 0; i < m_RightSideKeys.Length; i++)
        {
            Vector3 newPos = rightPreviousPosition;

            //First key
            if (i == 0)
            {
                newPos.x += offset / 2;
            }
            else
            {
                newPos.x += offset;
            }

            m_RightSideKeys[i].transform.localPosition = newPos;

            rightPreviousPosition = m_RightSideKeys[i].transform.localPosition;
        }

        //Add offset to left side
        for (int i = m_LeftSideKeys.Length - 1; i >= 0; i--)
        {
            Vector3 newPos = leftPreviousPosition;

            //Only add half the offset for the first set of keys
            if (i == m_LeftSideKeys.Length - 1)
            {
                newPos.x -= offset / 2;
            }
            else
            {
                newPos.x -= offset;
            }


            m_LeftSideKeys[i].transform.localPosition = newPos;

            leftPreviousPosition = m_LeftSideKeys[i].transform.localPosition;
        }
    }

    private void UpdateHorizontalSpacingWithKeyCenterObject(float sliderValue)
    {
        float offset = m_HozirontalOffset;

        for (int i = 0; i < sliderValue; i++)
        {
            offset += m_HozirontalOffsetAddPerSliderValue;
        }

        Vector3 rightPreviousPosition = m_CenterObject.transform.localPosition;
        Vector3 leftPreviousPosition = m_CenterObject.transform.localPosition;

        //This offset is to account for the fact we are starting on a key as the center position,
        //So we need to add half the offset to account for the starting position
        rightPreviousPosition.x += m_HozirontalOffset / 2;
        leftPreviousPosition.x -= m_HozirontalOffset / 2;


        //Add offset to right side
        for (int i = 0; i < m_RightSideKeys.Length; i++)
        {
            Vector3 newPos = rightPreviousPosition;

            //First key
            if (i == 0)
            {
                newPos.x += offset / 2;
            }
            else
            {
                newPos.x += offset;
            }

            m_RightSideKeys[i].transform.localPosition = newPos;

            rightPreviousPosition = m_RightSideKeys[i].transform.localPosition;
        }

        //Add offset to left side
        for (int i = m_LeftSideKeys.Length - 1; i >= 0; i--)
        {
            Vector3 newPos = leftPreviousPosition;

            //Only add half the offset for the first set of keys
            if (i == m_LeftSideKeys.Length - 1)
            {
                newPos.x -= offset / 2;
            }
            else
            {
                newPos.x -= offset;
            }


            m_LeftSideKeys[i].transform.localPosition = newPos;

            leftPreviousPosition = m_LeftSideKeys[i].transform.localPosition;
        }
    }

    #region Inspector Functions

    /// <summary>
    /// Used by the button in the inspector to show the spacing for the keys
    /// </summary>
    public void SetHorzontalSpacingForKeysWithSlider()
    {
        if (m_IsCenterObjectAKey)
        {
            UpdateHorizontalSpacingWithKeyCenterObjectWithSlider();
        }
        else
        {
            UpdateHorizontalSpacingWithCenterObjectWithSlider();
        }
    }

    private void UpdateHorizontalSpacingWithCenterObjectWithSlider()
    {
        Vector3 rightPreviousPosition = m_CenterObject.transform.localPosition;
        Vector3 leftPreviousPosition = m_CenterObject.transform.localPosition;

        //Add offset to right side
        for (int i = 0; i < m_RightSideKeys.Length; i++)
        {
            Vector3 newPos = rightPreviousPosition;

            //First key
            if (i == 0)
            {
                newPos.x += m_InspectorKeyHorizontalSpacing / 2;
            }
            else
            {
                newPos.x += m_InspectorKeyHorizontalSpacing;
            }

            m_RightSideKeys[i].transform.localPosition = newPos;

            rightPreviousPosition = m_RightSideKeys[i].transform.localPosition;
        }

        //Add offset to left side
        for (int i = m_LeftSideKeys.Length - 1; i >= 0; i--)
        {
            Vector3 newPos = leftPreviousPosition;

            //Only add half the offset for the first set of keys
            if (i == m_LeftSideKeys.Length - 1)
            {
                newPos.x -= m_InspectorKeyHorizontalSpacing / 2;
            }
            else
            {
                newPos.x -= m_InspectorKeyHorizontalSpacing;
            }


            m_LeftSideKeys[i].transform.localPosition = newPos;

            leftPreviousPosition = m_LeftSideKeys[i].transform.localPosition;
        }
    }

    private void UpdateHorizontalSpacingWithKeyCenterObjectWithSlider()
    {
        Vector3 rightPreviousPosition = m_CenterObject.transform.localPosition;
        Vector3 leftPreviousPosition = m_CenterObject.transform.localPosition;

        //This offset is to account for the fact we are starting on a key as the center position,
        //So we need to add half the offset to account for the starting position
        rightPreviousPosition.x += m_InspectorKeyHorizontalSpacing / 2;
        leftPreviousPosition.x -= m_InspectorKeyHorizontalSpacing / 2;

        //Add offset to right side
        for (int i = 0; i < m_RightSideKeys.Length; i++)
        {
            Vector3 newPos = rightPreviousPosition;

            //First key
            if (i == 0)
            {
                newPos.x += m_InspectorKeyHorizontalSpacing / 2;
            }
            else
            {
                newPos.x += m_InspectorKeyHorizontalSpacing;
            }

            m_RightSideKeys[i].transform.localPosition = newPos;

            rightPreviousPosition = m_RightSideKeys[i].transform.localPosition;
        }

        //Add offset to left side
        for (int i = m_LeftSideKeys.Length - 1; i >= 0; i--)
        {
            Vector3 newPos = leftPreviousPosition;

            //Only add half the offset for the first set of keys
            if (i == m_LeftSideKeys.Length - 1)
            {
                newPos.x -= m_InspectorKeyHorizontalSpacing / 2;
            }
            else
            {
                newPos.x -= m_InspectorKeyHorizontalSpacing;
            }


            m_LeftSideKeys[i].transform.localPosition = newPos;

            leftPreviousPosition = m_LeftSideKeys[i].transform.localPosition;
        }
    }


    #endregion
}
