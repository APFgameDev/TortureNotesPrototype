using UnityEngine;
using Brinx.SO;

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
    [SerializeField]private string m_LeftJoystickPressed = "LeftStickPress";
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
    [SerializeField] private VoidEvent m_OnRightTriggerPressed;
    [SerializeField] private VoidEvent m_OnLeftGripPressed;
    [SerializeField] private VoidEvent m_OnRightGripPressed;
    [SerializeField] private VoidEvent m_OnLeftJoystickPressed;
    [SerializeField] private VoidEvent m_OnRightJoystickPressed;
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

        //Store the grip held percent value and publish the event if the value pressed is greater then the dead zone value
        m_LeftGripHeldPercent.Value = Input.GetAxis(m_LeftGrip);
        if (m_LeftGripHeldPercent.Value >= m_LeftGripDeadZone.Value)
        {
            m_LeftGripHeld.Value = true;
            m_OnLeftGripPressed.Publish();
        }
        else
            m_LeftGripHeld.Value = false;

        //Store the grip held percent value and publish the event if the value pressed is greater then the dead zone value
        m_RightGripHeldPercent.Value = Input.GetAxis(m_RightGrip);
        if (m_RightGripHeldPercent.Value >= m_RightGripDeadZone.Value)
        {
            m_RightGripHeld.Value = true;
            m_OnRightGripPressed.Publish();
        }
        else
            m_RightGripHeld.Value = false;
        #endregion

        #region Handle Triggers
        if (m_LeftTrigger == null || m_LeftTriggerHeldPercent == null ||
            m_RightTriggerHeld == null || m_RightTriggerHeldPercent == null)
        {
            Debug.LogError("All of the trigger data is not set yet. Please set the trigger data in the InputManager and start again.\n Location == " + gameObject.name, this);
        }

        //Store the trigger held percent value and publish the event if the value pressed is greater then the dead zone value
        m_LeftTriggerHeldPercent.Value = Input.GetAxis(m_LeftTrigger);
        if (m_LeftTriggerHeldPercent.Value >= m_LeftTriggerDeadZone.Value)
        {
            m_LeftTriggerHeld.Value = true;
            m_OnLeftTriggerPressed.Publish();
        }
        else
            m_LeftTriggerHeld.Value = false;

        //Store the trigger held percent value and publish the event if the value pressed is greater then the dead zone value
        m_RightTriggerHeldPercent.Value = Input.GetAxis(m_RightTrigger);
        if (m_RightTriggerHeldPercent.Value >= m_RightTriggerDeadZone.Value)
        {
            m_LeftTriggerHeld.Value = true;
            m_OnRightTriggerPressed.Publish();
        }
        else
            m_RightTriggerHeld.Value = false;

        #endregion

        #region Handle Axis Sticks
        if (m_LeftAxisDirection == null || m_LeftJoystickPressed == null ||
            m_RightAxisDirection == null || m_RightJoystickPressed == null)
        {
            Debug.LogError("All of the axis data is not set yet. Please set the axis data in the InputManager and start again.\n Location == " + gameObject.name, this);
        }

        m_LeftAxisDirection.Value = new Vector2(Input.GetAxis(m_LeftStickHorizontal), Input.GetAxis(m_LeftStickVertical));
        m_LeftJoystickDown.Value = Input.GetButton(m_LeftJoystickPressed);

        m_RightAxisDirection.Value = new Vector2(Input.GetAxis(m_RightStickHorizontal), Input.GetAxis(m_RightStickVertical));
        m_RightJoystickDown.Value = Input.GetButton(m_RightJoystickPressed);
        #endregion
    }
}
