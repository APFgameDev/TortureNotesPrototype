using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ColumnManager : MonoBehaviour
{
    [Header("StickyNote Prefabs")]
    public GameObject m_StickyNote;

    [Header("Update Vertical Layout")]
    public bool m_UpdateLayout;

    [Header("Spawn a Sticky Note")]
    public bool m_SpawnStickyNote;

    [Header("What color you want the sticky notes to be in this column")]
    public Color m_ColumnColor;

    [Header("How many notes per colum?")]
    [SerializeField]
    private int m_MaxStickyNotesPerColum = 4;
    private int m_CurrentStickyNoteCount = 0;

    void Update()
    {
        if (m_UpdateLayout == true)
        {
            m_UpdateLayout = false;
            GetComponent<VerticalLayoutGroup>().SetLayoutVertical();
        }

        if (m_SpawnStickyNote == true)
        {
            m_SpawnStickyNote = false;
            GameObject obj = Instantiate(m_StickyNote);//, transform);

            AddStickyToColumn(obj.GetComponentInChildren<StickyNote>());
        }
    }

    /// <summary>
    /// Will add the sticky passed in to the column.
    /// Will parent the parent to the column.
    /// Will set the childs position to zero.
    /// Will reset the childs scale to 1.
    /// Will set the color of the sticky to the columns color.
    /// Will return true if there is space on this column and false otherwise
    /// </summary>
    /// <param name="stickynote"></param>
    public bool AddStickyToColumn(StickyNote stickynote)
    {
        if(m_CurrentStickyNoteCount < m_MaxStickyNotesPerColum)
        {
            //Set the parents parent to the column
            stickynote.transform.parent.SetParent(transform);

            //Reset its position
            stickynote.transform.position = Vector3.zero;

            //Reset its scale
            stickynote.transform.localScale = Vector3.one;
            stickynote.transform.parent.localScale = Vector3.one;

            //Set color of sticky
            stickynote.SetColor(m_ColumnColor);

            //Update the layout group
            GetComponent<VerticalLayoutGroup>().SetLayoutVertical();

            //Increment the count
            m_CurrentStickyNoteCount++;

            return true;
        }     
        else
        {
            return false;
        }
    }
}

