using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Annotation.SO;

public class Whiteboard : MonoBehaviour
{
    [SerializeField]
    private GameObject m_StickyNotePrefab;

    [SerializeField]
    private List<ColumnManager> m_Columns;

    [SerializeField]
    private Transform m_Workbar;
    [SerializeField]
    private int m_MaxWorkbarChildCount = 5;

    [SerializeField]
    private KeyboardSO m_KeyboardSO;

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    /// <summary>
    /// Will find the closest column to the sticky, set its parent, set its color and scale.
    /// </summary>
    /// <param name="note"></param>
    public void AddStickyNote(StickyNote note)
    {
        //Find closest column
        //Place sticky (Set Parent)
        //Set color
        //Set scale


        //Testing for now
        if(!m_Columns[0].AddStickyToColumn(note))
        {
            Debug.Log("Not enough room in this column!");
            //Send the sticky to its previous position
            //If it doesnt have a previous position, send it to the workbar
        }
    }

    /// <summary>
    /// Will instantiate a sticky note and call edit sticky on the sticky note
    /// </summary>
    public void CreateStickyNote()
    {
        if(m_Workbar.childCount < m_MaxWorkbarChildCount)
        {
            GameObject sticky = Instantiate(m_StickyNotePrefab, m_Workbar);

            StickyNote stickyNote = sticky.GetComponentInChildren<StickyNote>();

            stickyNote.SetWhiteboard(this);
            stickyNote.EditSticky();

            //Testing for now
            sticky.GetComponentInChildren<VRText>().StartListening();
        }
        else
        {
            Debug.Log("Too many sticky notes on work bar");
        }
    }

    /// <summary>
    /// Will delete the whiteboard object
    /// </summary>
    public void DeleteWhiteboard()
    {
        Destroy(gameObject);
    }
}
