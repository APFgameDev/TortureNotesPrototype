﻿using Annotation.SO;
using Annotation.SO.UnityEvents;
using UnityEngine;

public class InputController : MonoBehaviour
{
    #region Constant Unity InputManger strings
    [Header("Inputmanager Input Names")]
    [Tooltip("Make sure that the value of this string matches up with the value in the Unity InputManager.")]
    [SerializeField] private string m_LeftTrigger = "LeftIndexTrigger";
    [Tooltip("Make sure that the value of this string matches up with the value in the Unity InputManager.")]
    [SerializeField] private string m_RightTrigger = "RightIndexTrigger";
    [Tooltip("Make sure that the value of this string matches up with the value in the Unity InputManager.")]
    [SerializeField] private string m_LeftGrip = "LeftGripTrigger";
    [Tooltip("Make sure that the value of this string matches up with the value in the Unity InputManager.")]
    [SerializeField] private string m_RightGrip = "RightGripTrigger";
    [Tooltip("Make sure that the value of this string matches up with the value in the Unity InputManager.")]
    [SerializeField] private string m_RightStickVertical = "RightStickVertical";
    [Tooltip("Make sure that the value of this string matches up with the value in the Unity InputManager.")]
    [SerializeField] private string m_RightStickHorizontal = "RightStickHorizontal";
    [Tooltip("Make sure that the value of this string matches up with the value in the Unity InputManager.")]
    [SerializeField] private string m_RightJoystickPressed = "RightStickPress";
    [Tooltip("Make sure that the value of this string matches up with the value in the Unity InputManager.")]
    [SerializeField] private string m_LeftStickVertical = "LeftStickVertical";
    [Tooltip("Make sure that the value of this string matches up with the value in the Unity InputManager.")]
    [SerializeField] private string m_LeftStickHorizontal = "LeftStickHorizontal";
    [Tooltip("Make sure that the value of this string matches up with the value in the Unity InputManager.")]
    [SerializeField] private string m_LeftJoystickPressed = "LeftStickPress";
    #endregion

    #region Left Controller Values
    [Header("Left Controller Values")]
    [SerializeField] private InputEventContainer m_LeftGripContainer;
    [SerializeField] private InputEventContainer m_LeftTriggerContainer;
    [SerializeField] private Vector2Variable m_LeftAxisDirection;
    [SerializeField] private BoolVariable m_LeftJoystickDown;
    #endregion

    #region Right Controller Values
    [Header("Right Controller Values")]
    [SerializeField] private InputEventContainer m_RightGripContainer;
    [SerializeField] private InputEventContainer m_RightTriggerContainer;
    [SerializeField] private Vector2Variable m_RightAxisDirection;
    [SerializeField] private BoolVariable m_RightJoystickDown;
    #endregion

    #region Joystick Events
    [Header("Joystick Events")]
    [SerializeField] private UnityEventVoidSO m_OnLeftJoystickPressed;
    [SerializeField] private UnityEventVoidSO m_OnLeftJoystickHeld;
    [SerializeField] private UnityEventVoidSO m_OnLeftJoystickReleased;
    [SerializeField] private UnityEventVoidSO m_OnRightJoystickPressed;
    [SerializeField] private UnityEventVoidSO m_OnRightJoystickHeld;
    [SerializeField] private UnityEventVoidSO m_OnRightJoystickReleased;
    #endregion

    /// <summary>
    /// Update will poll the Unity InputManager in order to see any controller changes. If there are any changes
    /// then the InputManager will post the changed values. The InputManager will also call any events that listen to 
    /// the void events related to InputChanged
    /// </summary>
    private void Update()
    {
        m_LeftGripContainer.CalculateInputData(Input.GetAxis(m_LeftGrip));
        m_LeftTriggerContainer.CalculateInputData(Input.GetAxis(m_LeftTrigger));

        m_RightGripContainer.CalculateInputData(Input.GetAxis(m_RightGrip));
        m_RightTriggerContainer.CalculateInputData(Input.GetAxis(m_RightTrigger));

        #region Handle Axis Sticks
        if (m_LeftAxisDirection == null || m_LeftJoystickPressed == null ||
            m_RightAxisDirection == null || m_RightJoystickPressed == null)
        {
            Debug.LogError("All of the axis data is not set yet. Please set the axis data in the InputManager and start again.\n Location == " + gameObject.name, this);
        }

        #region Left Joystick
        bool leftJoystickValue = Input.GetButton(m_LeftJoystickPressed);

        //Check to see if the previous grip value is less then the deadzone and if the current value is greater then the deadzone. If this statement is true then the grip is being grabbed for the first time this frame
        if (!m_LeftJoystickDown.Value && leftJoystickValue)
        {
            m_OnLeftJoystickPressed.UnityEvent.Invoke();
        }

        //if it wasn't first held on this frame then just check to see if the previous value was greater then the deadzone, but the current value isn't. If this statement is true then the grip is no longer being held
        if (m_LeftJoystickDown.Value && !leftJoystickValue)
        {
            m_OnLeftJoystickReleased.UnityEvent.Invoke();
        }

        //if the current input value
        if (leftJoystickValue)
        {
            m_OnLeftJoystickHeld.UnityEvent.Invoke();
        }

        m_LeftAxisDirection.Value = new Vector2(Input.GetAxis(m_LeftStickHorizontal), Input.GetAxis(m_LeftStickVertical));
        m_LeftJoystickDown.Value = leftJoystickValue;
        #endregion  

        #region Right Joystick
        bool rightJoystickValue = Input.GetButton(m_RightJoystickPressed);

        //Check to see if the previous grip value is less then the deadzone and if the current value is greater then the deadzone. If this statement is true then the grip is being grabbed for the first time this frame
        if (!m_RightJoystickDown.Value && rightJoystickValue)
        {
            m_OnRightJoystickPressed.UnityEvent.Invoke();
        }

        //if it wasn't first held on this frame then just check to see if the previous value was greater then the deadzone, but the current value isn't. If this statement is true then the grip is no longer being held
        if (m_RightJoystickDown.Value && !rightJoystickValue)
        {
            m_OnRightJoystickReleased.UnityEvent.Invoke();
        }

        //if the current input value
        if (rightJoystickValue)
        {
            m_OnRightJoystickHeld.UnityEvent.Invoke();
        }

        m_RightAxisDirection.Value = new Vector2(Input.GetAxis(m_RightStickHorizontal), Input.GetAxis(m_RightStickVertical));
        m_RightJoystickDown.Value = Input.GetButton(m_RightJoystickPressed);
        #endregion
        #endregion
    }
}