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
    /// Will instantiate a sticky note, and have it go to the keyboard
    /// </summary>
    public void CreateStickyNote()
    {
        GameObject sticky = Instantiate(m_StickyNotePrefab);

        //Turn on keyboard
    }

    /// <summary>
    /// Will delete the whiteboard object
    /// </summary>
    public void DeleteWhiteboard()
    {
        Destroy(gameObject);
    }
}
