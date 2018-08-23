using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardVisualScaler : MonoBehaviour
{
    [Header("Caps position and key objects")]
    [SerializeField] private Transform m_CapsPosition;
    [SerializeField] private GameObject m_CapsKey;

    [Header("Clear position and key objects")]
    [SerializeField] private Transform m_ClearPosition;
    [SerializeField] private GameObject m_ClearKey;

    [Header("Delete position and key objects")]
    [SerializeField] private Transform m_DeletePosition;
    [SerializeField] private GameObject m_DeleteKey;

    [Header("Cancel position and key objects")]
    [SerializeField] private Transform m_CancelPosition;
    [SerializeField] private GameObject m_CancelKey;

    [Header("Done position and key objects")]
    [SerializeField] private Transform m_DonePosition;
    [SerializeField] private GameObject m_DoneKey;

    [Header("Change position and key objects")]
    [SerializeField] private Transform m_ChangePosition;
    [SerializeField] private GameObject m_ChangeKey;

    [Tooltip("Use this to set the scale of the keyboard background. The length of this array needs to match the total number of 'segments' on the scroll bar. " +
        "The first index is going to be set to the initial scale of the keyboard visuals. Everything after that index is up to you to change. Change it so that it" +
        "scales to the size you want at the given index")]
    [SerializeField] private float[] m_KeyboardBackgroundVisualXScale = new float[5];

    [Tooltip("Use this to set the scale of the keyboard background. The length of this array needs to match the total number of 'segments' on the scroll bar. " +
        "The first index is going to be set to the initial scale of the keyboard visuals. Everything after that index is up to you to change. Change it so that it" +
        "scales to the size you want at the given index")]
    [SerializeField] private float[] m_KeyboardBackgroundVisualZScale = new float[5];

    [Header("Offset that is added to the keys for spacing")]
    public float m_VerticalOffset = 0.11f;

    [Header("Value added to the offset based on the sliders value")]
    public float m_VerticalOffsetAdd = 0.02f;

    [Header("Center gameobject of the keyboard")]
    [SerializeField] private GameObject m_CenterGameObject;

    [Header("Keyboard row objects")]
    [SerializeField] private GameObject[] m_RowObjects;


    private void Awake()
    {
        m_KeyboardBackgroundVisualXScale[0] = transform.localScale.x;

        if (m_CapsPosition == null || m_CapsKey == null)
        {
            Debug.LogError("Caps key has not been setup in " + this);
        }

        if (m_ClearPosition == null || m_ClearKey == null)
        {
            Debug.LogError("Clear key has not been setup in " + this);
        }

        if (m_DeletePosition == null || m_DeleteKey == null)
        {
            Debug.LogError("Delete key has not been setup in " + this);
        }

        if (m_CancelPosition == null || m_CancelKey == null)
        {
            Debug.LogError("Cancel key has not been setup in " + this);
        }

        if (m_DonePosition == null || m_DoneKey == null)
        {
            Debug.LogError("Done key has not been setup in " + this);
        }

        if (m_ChangePosition == null || m_ChangeKey == null)
        {
            Debug.LogError("Change key has not been setup in " + this);
        }
    }

    public void ScaleHorizontalVisuals(int sliderValue)
    {
        //Scale the background of the keyboard to the appropriate size
        float xScale = m_KeyboardBackgroundVisualXScale[sliderValue];
        Vector3 newScale = new Vector3(xScale, transform.localScale.y, transform.localScale.z);
        transform.localScale = newScale;

        //Caps key
        m_CapsKey.transform.position = m_CapsPosition.position;

        //Clear key
        m_ClearKey.transform.position = m_ClearPosition.position;

        //Delete key
        m_DeleteKey.transform.position = m_DeletePosition.position;

        //Cancel key
        m_CancelKey.transform.position = m_CancelPosition.position;

        //Done key
        m_DoneKey.transform.position = m_DonePosition.position;

        //Change key
        m_ChangeKey.transform.position = m_ChangePosition.position;
    }

    public void UpdateVerticalSpacing(int sliderValue)
    {
        float offset = m_VerticalOffset;

        for (int i = 0; i < sliderValue; i++)
        {
            offset += m_VerticalOffsetAdd;
        }

        Vector3 previousPos = m_RowObjects[0].transform.localPosition;

        //Add offset to top rows
        for (int i = 1; i < m_RowObjects.Length; i++)
        {
            Vector3 newPos = previousPos;

            newPos.z += offset;

            m_RowObjects[i].transform.localPosition = newPos;
            previousPos = newPos;
        }

        ////Scale the background of the keyboard to the appropriate size
        float zScale = m_KeyboardBackgroundVisualZScale[sliderValue];
        Vector3 newScale = new Vector3(transform.localScale.x, transform.localScale.y, zScale);
        transform.localScale = newScale;
    }
}
