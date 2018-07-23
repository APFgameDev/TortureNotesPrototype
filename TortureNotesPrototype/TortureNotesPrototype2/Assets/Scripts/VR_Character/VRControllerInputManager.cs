using UnityEngine.UI;
using UnityEngine;

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

        SetActive();
    }

    private void Update()
    {
        LeftControllerSection = (QuadrantDirections)LeftControllerInput.StickQuadrantDirection;
        RightControllerSection = (QuadrantDirections)RightControllerInput.StickQuadrantDirection;
    }
    
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

    public static void SetText(Text text)
    {
        RightControllerInput.TestText = text;
    }
}
