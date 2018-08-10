using UnityEngine;
using Annotation.SO;

public class InputManager : MonoBehaviour
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
    [SerializeField] private FloatVariable m_LeftGripHeldPercent;
    [SerializeField] private FloatReference m_LeftGripDeadZone;
    [SerializeField] private BoolVariable m_LeftGripHeld;
    [SerializeField] private FloatVariable m_LeftTriggerHeldPercent;
    [SerializeField] private FloatReference m_LeftTriggerDeadZone;
    [SerializeField] private BoolVariable m_LeftTriggerHeld;
    [SerializeField] private Vector2Variable m_LeftAxisDirection;
    [SerializeField] private BoolVariable m_LeftJoystickDown;
    #endregion

    #region Right Controller Values
    [Header("Right Controller Values")]
    [SerializeField] private FloatVariable m_RightGripHeldPercent;
    [SerializeField] private FloatReference m_RightGripDeadZone;
    [SerializeField] private BoolVariable m_RightGripHeld;
    [SerializeField] private FloatVariable m_RightTriggerHeldPercent;
    [SerializeField] private FloatReference m_RightTriggerDeadZone;
    [SerializeField] private BoolVariable m_RightTriggerHeld;
    [SerializeField] private Vector2Variable m_RightAxisDirection;
    [SerializeField] private BoolVariable m_RightJoystickDown;
    #endregion

    #region Events 
    [Header("Input Events")]
    [SerializeField] private VoidEvent m_OnLeftTriggerPressed;
    [SerializeField] private VoidEvent m_OnLeftTriggerHeld;
    [SerializeField] private VoidEvent m_OnLeftTriggerReleased;
    [SerializeField] private VoidEvent m_OnRightTriggerPressed;
    [SerializeField] private VoidEvent m_OnRightTriggerHeld;
    [SerializeField] private VoidEvent m_OnRightTriggerReleased;
    [SerializeField] private VoidEvent m_OnLeftGripPressed;
    [SerializeField] private VoidEvent m_OnLeftGripHeld;
    [SerializeField] private VoidEvent m_OnLeftGripReleased;
    [SerializeField] private VoidEvent m_OnRightGripPressed;
    [SerializeField] private VoidEvent m_OnRightGripHeld;
    [SerializeField] private VoidEvent m_OnRightGripReleased;
    [SerializeField] private VoidEvent m_OnLeftJoystickPressed;
    [SerializeField] private VoidEvent m_OnLeftJoystickHeld;
    [SerializeField] private VoidEvent m_OnLeftJoystickReleased;
    [SerializeField] private VoidEvent m_OnRightJoystickPressed;
    [SerializeField] private VoidEvent m_OnRightJoystickHeld;
    [SerializeField] private VoidEvent m_OnRightJoystickReleased;
    #endregion

    /// <summary>
    /// Update will poll the Unity InputManager in order to see any controller changes. If there are any changes
    /// then the InputManager will post the changed values. The InputManager will also call any events that listen to 
    /// the void events related to InputChanged
    /// </summary>
    private void Update()
    {
        #region Handle Grips
        //Error checking to make sure that the grip data is actually set properly
        if (m_LeftGripHeld == null || m_LeftGripHeldPercent == null ||
            m_RightGripHeld == null || m_RightGripHeldPercent == null)
        {
            Debug.LogError("All of the grip data is not set yet. Please set the grip data in the InputManager and start again.\n Location == " + gameObject.name, this);
            return;
        }

        #region Left Grip Evaluation
        float leftGripValue = Input.GetAxis(m_LeftGrip);

        //Check to see if the previous grip value is less then the deadzone and if the current value is greater then the deadzone. If this statement is true then the grip is being grabbed for the first time this frame
        if (m_LeftGripHeldPercent.Value < m_LeftGripDeadZone.Value && leftGripValue >= m_LeftGripDeadZone.Value)
        {
            m_LeftGripHeld.Value = true;
            m_OnLeftGripPressed.Publish();
        }

        //if it wasn't first held on this frame then just check to see if the previous value was greater then the deadzone, but the current value isn't. If this statement is true then the grip is no longer being held
        if (m_LeftGripHeldPercent.Value >= m_LeftGripDeadZone.Value && leftGripValue < m_LeftGripDeadZone.Value)
        {
            m_LeftGripHeld.Value = false;
            m_OnLeftGripReleased.Publish();
        }

        //if the current input value
        if (leftGripValue >= m_LeftGripDeadZone.Value)
        {
            m_OnLeftGripHeld.Publish();
        }
        #endregion

        #region Right Grip Evaluation
        float rightGripValue = Input.GetAxis(m_RightGrip);

        //Check to see if the previous grip value is less then the deadzone and if the current value is greater then the deadzone. If this statement is true then the grip is being grabbed for the first time this frame
        if (m_RightGripHeldPercent.Value < m_RightGripDeadZone.Value && rightGripValue >= m_RightGripDeadZone.Value)
        {
            m_RightGripHeld.Value = true;
            m_OnRightGripPressed.Publish();
        }

        //if it wasn't first held on this frame then just check to see if the previous value was greater then the deadzone, but the current value isn't. If this statement is true then the grip is no longer being held
        if (m_RightGripHeldPercent.Value >= m_RightGripDeadZone.Value && rightGripValue < m_RightGripDeadZone.Value)
        {
            m_RightGripHeld.Value = false;
            m_OnRightGripReleased.Publish();
        }

        //if the current input value
        if (rightGripValue >= m_RightGripDeadZone.Value)
        {
            m_OnRightGripHeld.Publish();
        }
        #endregion
        #endregion

        #region Handle Triggers
        if (m_LeftTrigger == null || m_LeftTriggerHeldPercent == null ||
            m_RightTriggerHeld == null || m_RightTriggerHeldPercent == null)
        {
            Debug.LogError("All of the trigger data is not set yet. Please set the trigger data in the InputManager and start again.\n Location == " + gameObject.name, this);
        }

        #region Left Trigger Evaluation
        float leftTriggerValue = Input.GetAxis(m_LeftTrigger);

        //Check to see if the previous Trigger value is less then the deadzone and if the current value is greater then the deadzone. If this statement is true then the Trigger is being grabbed for the first time this frame
        if (m_LeftTriggerHeldPercent.Value < m_LeftTriggerDeadZone.Value && leftTriggerValue >= m_LeftTriggerDeadZone.Value)
        {
            m_LeftTriggerHeld.Value = true;
            m_OnLeftTriggerPressed.Publish();
        }

        //if it wasn't first held on this frame then just check to see if the previous value was greater then the deadzone, but the current value isn't. If this statement is true then the Trigger is no longer being held
        if (m_LeftTriggerHeldPercent.Value >= m_LeftTriggerDeadZone.Value && leftTriggerValue < m_LeftTriggerDeadZone.Value)
        {
            m_LeftTriggerHeld.Value = false;
            m_OnLeftTriggerReleased.Publish();
        }

        //if the current input value
        if (leftTriggerValue >= m_LeftTriggerDeadZone.Value)
        {
            m_OnLeftTriggerHeld.Publish();
        }
        #endregion

        #region Right Trigger Evaluation
        float rightTriggerValue = Input.GetAxis(m_RightTrigger);

        //Check to see if the previous Trigger value is less then the deadzone and if the current value is greater then the deadzone. If this statement is true then the Trigger is being grabbed for the first time this frame
        if (m_RightTriggerHeldPercent.Value < m_RightTriggerDeadZone.Value && rightTriggerValue >= m_RightTriggerDeadZone.Value)
        {
            m_RightTriggerHeld.Value = true;
            m_OnRightTriggerPressed.Publish();
        }

        //if it wasn't first held on this frame then just check to see if the previous value was greater then the deadzone, but the current value isn't. If this statement is true then the Trigger is no longer being held
        if (m_RightTriggerHeldPercent.Value >= m_RightTriggerDeadZone.Value && rightTriggerValue < m_RightTriggerDeadZone.Value)
        {
            m_RightTriggerHeld.Value = false;
            m_OnRightTriggerReleased.Publish();
        }

        //if the current input value
        if (rightTriggerValue >= m_RightTriggerDeadZone.Value)
        {
            m_OnRightTriggerHeld.Publish();
        }
        #endregion      
        #endregion

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
            m_OnRightGripPressed.Publish();
        }

        //if it wasn't first held on this frame then just check to see if the previous value was greater then the deadzone, but the current value isn't. If this statement is true then the grip is no longer being held
        if (m_LeftJoystickDown.Value && !leftJoystickValue)
        {
            m_OnRightGripReleased.Publish();
        }

        //if the current input value
        if (m_LeftJoystickDown.Value && leftJoystickValue)
        {
            m_OnRightGripHeld.Publish();
        }

        m_LeftAxisDirection.Value = new Vector2(Input.GetAxis(m_LeftStickHorizontal), Input.GetAxis(m_LeftStickVertical));
        m_LeftJoystickDown.Value = leftJoystickValue;
        #endregion  

        #region Right Joystick
        bool rightJoystickValue = Input.GetButton(m_RightJoystickPressed);

        //Check to see if the previous grip value is less then the deadzone and if the current value is greater then the deadzone. If this statement is true then the grip is being grabbed for the first time this frame
        if (!m_RightJoystickDown.Value && rightJoystickValue)
        {
            m_OnRightGripPressed.Publish();
        }

        //if it wasn't first held on this frame then just check to see if the previous value was greater then the deadzone, but the current value isn't. If this statement is true then the grip is no longer being held
        if (m_RightJoystickDown.Value && !rightJoystickValue)
        {
            m_OnRightGripReleased.Publish();
        }

        //if the current input value
        if (m_RightJoystickDown.Value && rightJoystickValue)
        {
            m_OnRightGripHeld.Publish();
        }

        m_RightAxisDirection.Value = new Vector2(Input.GetAxis(m_RightStickHorizontal), Input.GetAxis(m_RightStickVertical));
        m_RightJoystickDown.Value = Input.GetButton(m_RightJoystickPressed);
        #endregion
        #endregion
    }
}
