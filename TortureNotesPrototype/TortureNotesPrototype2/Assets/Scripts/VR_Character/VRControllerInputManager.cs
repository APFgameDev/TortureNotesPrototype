using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class VRControllerInputManager : MonoBehaviour
{
    //Knowing that i want the top most quadrant to be the 0 point for cycling.
    //I need to implement a way to convert the quadrants to this system.
    //IE: if the quadrant (8 section quadrant) section is 1, i know that that is the top right section
    // Quadrant | Section
    //        0 = Right
    //        1 = Top Right
    //        2 = Top
    //        3 = Top Left
    //        4 = Left
    //        5 = Bottom Left
    //        6 = Bottom
    //        7 = Bottom Right
    public enum QuadrantDirections
    {
        Right,
        TopRight,
        Top,
        TopLeft,
        Left,
        BottomLeft,
        Bottom,
        BottomRight,
        None = -1
    }

    public QuadrantDirections LeftControllerSection;
    public QuadrantDirections RightControllerSection;

    public static LeftVRControllerInput LeftControllerInput;
    public static RightVRControllerInput RightControllerInput;

    private bool isDocked = false;
    private bool doOnce = true;

    public GameObject DockedPanel;
    public bool NewDock = false;

    private bool capsDoOnce = true;
    private bool switchInputStateDoOnce = true;
    private bool switchStateDoOnce = true;
    private VRController.ControllerState previousLeftState;
    private VRController.ControllerState previousRightState;

    void Start()
    {
        if (LeftControllerInput == null)
        {
            LeftControllerInput = FindObjectOfType<LeftVRControllerInput>();
        }

        if (RightControllerInput == null)
        {
            RightControllerInput = FindObjectOfType<RightVRControllerInput>();
        }

        //SetActive();
    }

    private void Update()
    {
        //LeftControllerSection = (QuadrantDirections)LeftControllerInput.StickQuadrantDirection;
       // RightControllerSection = (QuadrantDirections)RightControllerInput.StickQuadrantDirection;

        #region Dock/UnDock

        if (Input.GetAxis("LeftGripTrigger") > 0.8f && Input.GetAxis("RightGripTrigger") > 0.8f && doOnce == true)
        {
            doOnce = false;
            Dock();
        }
        else if (Input.GetAxis("LeftGripTrigger") <= 0.0f && Input.GetAxis("RightGripTrigger") <= 0.0 && doOnce == false)
        {
            doOnce = true;
        }

        #endregion

        #region Toggle Caps

        if (Input.GetAxis("LeftIndexTrigger") > 0.5f && capsDoOnce == true)
        {
            capsDoOnce = false;
            RightControllerInput.IsCaps = !RightControllerInput.IsCaps;
            LeftControllerInput.IsCaps = !LeftControllerInput.IsCaps;
        }
        else if (Input.GetAxis("LeftIndexTrigger") <= 0.25f && capsDoOnce == false)
        {
            capsDoOnce = true;
        }

        #endregion

        #region Switch Input State

        //Switch 
        if (Input.GetButtonDown("LeftStickPress") && switchInputStateDoOnce == true)
        {
            VRController.ControllerInputState LeftState = LeftControllerInput.InputState;
            VRController.ControllerInputState RightState = RightControllerInput.InputState;

            if (LeftState >= VRController.ControllerInputState.Number || RightState >= VRController.ControllerInputState.Number)
            {
                LeftControllerInput.InputState = VRController.ControllerInputState.Alphabet;
                RightControllerInput.InputState = VRController.ControllerInputState.Alphabet;
            }
            else
            {
                LeftControllerInput.InputState += 1;
                RightControllerInput.InputState += 1;
                Debug.Log(RightControllerInput.InputState);
            }

            switchInputStateDoOnce = false;
        }
        else if (Input.GetButtonUp("LeftStickPress") && switchInputStateDoOnce == false)
        {
            switchInputStateDoOnce = true;
        }

        #endregion

        #region Switch Controller State

        //IDLE STATE
        //No left input, switch to idle state
        if (LeftControllerInput.StickQuadrantDirection == -1)
        {
            //Menu state overrides all states
            if (LeftControllerInput.State != VRController.ControllerState.Menu && RightControllerInput.State != VRController.ControllerState.Menu)
            {
                //If we are already in the idle state, dont assign it again.
                if (LeftControllerInput.State != VRController.ControllerState.Idle && RightControllerInput.State != VRController.ControllerState.Idle)
                {
                    LeftControllerInput.State = VRController.ControllerState.Idle;
                    RightControllerInput.State = VRController.ControllerState.Idle;
                }
            }
        }

        //MENU STATE
        //Right Trigger Pressed, switch to menu state
        if (Input.GetAxis("RightIndexTrigger") > 0.5f && switchStateDoOnce == true)
        {
            switchStateDoOnce = false;
            //If you are already on the menu state, switch to the previous state
            if (LeftControllerInput.State == VRController.ControllerState.Menu && RightControllerInput.State == VRController.ControllerState.Menu)
            {
                LeftControllerInput.State = previousLeftState;
                RightControllerInput.State = previousRightState;
            }
            else
            {
                previousLeftState = LeftControllerInput.State;
                previousRightState = RightControllerInput.State;

                LeftControllerInput.State = VRController.ControllerState.Menu;
                RightControllerInput.State = VRController.ControllerState.Menu;
            }
        }
        else if (Input.GetAxis("RightIndexTrigger") <= 0.25f && switchStateDoOnce == false)
        {
            switchStateDoOnce = true;
        }

        //TYPING STATE
        //Left stick has something selected
        if (LeftControllerInput.StickQuadrantDirection != -1)
        {
            if (LeftControllerInput.State != VRController.ControllerState.Menu && RightControllerInput.State != VRController.ControllerState.Menu)
            {
                LeftControllerInput.State = VRController.ControllerState.Typing;
                RightControllerInput.State = VRController.ControllerState.Typing;
            }
        }

        #endregion
    }

    private void Dock()
    {
        isDocked = !isDocked;

        if (isDocked)
        {
            //if (NewDock)
            {
                //Enable the new dock and disable the old one
                DockedPanel.SetActive(true);
                RightControllerInput.EnableDisableBackgroundCanvas();

                //Find the point between both hands and place the dock there facing the camera
                Vector3 sum = Vector3.zero;
                Vector3 newPos = Vector3.zero;

                sum += LeftControllerInput.gameObject.transform.position;
                sum += RightControllerInput.gameObject.transform.position;

                newPos = sum / 2;

                DockedPanel.transform.position = newPos;
                DockedPanel.transform.rotation = Camera.main.transform.rotation;

                //Get the docked panel info
                VRPanel dockedPanel = DockedPanel.GetComponent<VRPanel>();

                //Assign the info to the controller
                LeftControllerInput.gameObject.transform.SetParent(dockedPanel.LeftControllerLocation);   //
                LeftControllerInput.gameObject.transform.localPosition = Vector3.zero;                    // Left
                LeftControllerInput.gameObject.transform.localRotation = Quaternion.identity;             //

                RightControllerInput.gameObject.transform.SetParent(dockedPanel.RightControllerLocation); //
                RightControllerInput.gameObject.transform.localPosition = Vector3.zero;                   // Right
                RightControllerInput.gameObject.transform.localRotation = Quaternion.identity;            //

                //string currentText = RightControllerInput.TextObject.text;
                string currentText = RightControllerInput.ControllerText.Value;

                //Assign the text object
                //SetText(dockedPanel.TextObject);
                //Assign the text
                dockedPanel.TextObject.text = currentText;
            }
        }
        else
        {
            RightControllerInput.EnableDisableBackgroundCanvas();
            //SetText(RightControllerInput.BackgroundCanvas.GetComponent<VRPanel>().TextObject);
            //RightControllerInput.TextObject.text = DockedPanel.GetComponent<VRPanel>().TextObject.text;

            RightControllerInput.gameObject.transform.SetParent(RightControllerInput.ControllerCanvasLocation.transform);
            RightControllerInput.gameObject.transform.localPosition = Vector3.zero;
            RightControllerInput.gameObject.transform.localRotation = Quaternion.identity;

            LeftControllerInput.gameObject.transform.SetParent(LeftControllerInput.ControllerCanvasLocation.transform);
            LeftControllerInput.gameObject.transform.localPosition = Vector3.zero;
            LeftControllerInput.gameObject.transform.localRotation = Quaternion.identity;

            DockedPanel.SetActive(false);
        }

    }

    /// <summary>
    /// Will return the section from the quadrant passed in
    /// </summary>
    /// <param name="quadrant"></param>
    /// <returns></returns>
    public int GetSectionFromQuadrant(int quadrant)
    {
        int section = -1;

        switch (quadrant)
        {
            case 0:
                section = 2;
                break;
            case 1:
                section = 1;
                break;
            case 2:
                section = 0;
                break;
            case 3:
                section = 7;
                break;
            case 4:
                section = 6;
                break;
            case 5:
                section = 5;
                break;
            case 6:
                section = 4;
                break;
            case 7:
                section = 3;
                break;
            default:
                break;
        }

        return section;
    }

    /// <summary>
    /// Will return the quadrant from the section passed in
    /// </summary>
    /// <param name="section"></param>
    /// <returns></returns>
    public int GetQuadrantFromSection(int section)
    {
        int quadrant = -1;

        switch (section)
        {
            case 0:
                quadrant = 2;
                break;
            case 1:
                quadrant = 1;
                break;
            case 2:
                quadrant = 0;
                break;
            case 3:
                quadrant = 7;
                break;
            case 4:
                quadrant = 6;
                break;
            case 5:
                quadrant = 5;
                break;
            case 6:
                quadrant = 4;
                break;
            case 7:
                quadrant = 3;
                break;
            default:
                break;
        }

        return quadrant;
    }

    /// <summary>
    /// The first child in the parent should be the top most section
    /// | Quadrant Right        (0) = Child 2 
    /// | Quadrant Top Right    (1) = Child 1 
    /// | Quadrant Top          (2) = Child 0 
    /// | Quadrant Top Left     (3) = Child 7 
    /// | Quadrant Left         (4) = Child 6 
    /// | Quadrant Bottom Left  (5) = Child 5 
    /// | Quadrant Bottom       (6) = Child 4 
    /// | Quadrant Bottom Right (7) = Child 3 |
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="quadrant"></param>
    /// <returns></returns>
    private GameObject GetChildFromParentFromQuadrant(GameObject parent, int quadrant)
    {
        int childindex = GetSectionFromQuadrant(quadrant);

        if (childindex != -1)
        {
            return parent.transform.GetChild(childindex).gameObject;
        }
        else
        {
            Debug.Log("Invalid quadrant passed in!");
            return null;
        }
    }

    public static void SetActive()
    {
        LeftControllerInput.gameObject.SetActive(!LeftControllerInput.gameObject.activeInHierarchy);
        RightControllerInput.gameObject.SetActive(!RightControllerInput.gameObject.activeInHierarchy);
    }

    //public static void SetText(TextMeshProUGUI text)
    //{
    //    RightControllerInput.TextObject = text;
    //    LeftControllerInput.TextObject = text;
    //
    //    RightControllerInput.TypingEvent.Publish();
    //    LeftControllerInput.TypingEvent.Publish();
    //}
}
