using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VRInput : MonoBehaviour
{
    public Text TestText;

    [Range(0f, 1f)]
    public float m_EventRadius = 0.8f;

    public float SectionAngle;
    public int SectionIndex;
    public float CurrentLeftAngle;

    public Vector2 ThumbPos = Vector2.zero;

    public int StickDirection = -1;
    protected int PreviousStickPosition;

    public bool IsFreeType = false;

    public GameObject CircleColorParent;
    public GameObject CircleTextParent;
    protected Vector3 ChildScale;
    Vector2 m_LeftThumbPos = Vector2.zero;

    /// <summary>
    /// Called when a selection is made
    /// </summary>
    public virtual void Select()
    {

    }

    /// <summary>
    /// Used to get the stick direction
    /// </summary>
    /// <param name="a_DirToCalc"></param>
    /// <returns></returns>
    protected virtual int CalculateStickDir(Vector2 a_DirToCalc)
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
    /// Is called when an input is detected from the stick
    /// </summary>
    protected virtual void StickInput()
    {
        if (StickDirection > -1)
        {
            ScaleChildren(StickDirection, CircleColorParent, ChildScale, true);
        }
        else
        {
            ScaleChildren(-1, CircleColorParent, ChildScale);
        }
    }

    /// <summary>
    /// Is called when the stick direction has been changed
    /// </summary>
    /// <param name="newDir"></param>
    protected virtual void StickDirectionChanged(int newDir)
    {

    }


    /// <summary>
    /// Will return in degrees the angle as 0 - 360
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    protected float FindAngle(float x, float y)
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
    protected int GetSectionFromAngle(float angle)
    {
        //To get the offset of the section so that the section angle matches the input background
        float angleOffset = SectionAngle / (360.0f / SectionAngle);
        float newAngle = angle + angleOffset;

        if (newAngle > 360.0f)
        {
            newAngle -= 360.0f;
        }

        int returnedValue = (int)((int)newAngle / SectionAngle);

        return returnedValue;
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