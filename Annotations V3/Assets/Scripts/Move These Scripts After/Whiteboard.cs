using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Annotation.SO;

public class Whiteboard : MonoBehaviour
{
    [SerializeField]
    private GameObject m_StickyNotePrefab;

    [SerializeField]
    private Transform m_Workbar;

    [SerializeField]
    private Transform m_WhiteboardStickyArea;

    [SerializeField]
    private int m_MaxWorkbarChildCount = 5;

    [SerializeField]
    private KeyboardSO m_KeyboardSO;

    [SerializeField]
    private Collider m_SurfaceCollider;

    [Header("Sticky Note Color")]
    [SerializeField] private Color m_RedColor;
    [SerializeField] private Color m_BlueColor;
    [SerializeField] private Color m_GreenColor;
    [SerializeField] private Color m_PurpleColor;

    [Header("Are we editing a sticky note?")]
    [SerializeField]
    private BoolVariable m_DoneEditingStickyNote;

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    /// <summary>
    /// Will find the closest column to the sticky, set its parent and set its scale.
    /// </summary>
    /// <param name="note"></param>
    public void AddStickyNote(StickyNote note)
    {
        note.gameObject.transform.parent.SetParent(m_WhiteboardStickyArea);
        note.SetBackToParent();
    }

    private void CreateStickyNote(Color stickycolor)
    {
        if (m_Workbar.childCount < m_MaxWorkbarChildCount && m_DoneEditingStickyNote.Value == true)
        {
            GameObject sticky = Instantiate(m_StickyNotePrefab, m_Workbar);

            StickyNote stickyNote = sticky.GetComponentInChildren<StickyNote>();
            stickyNote.SetColor(stickycolor);

            stickyNote.SetWhiteboard(this);
            stickyNote.EditSticky();

            m_DoneEditingStickyNote.Value = false;

            //Testing for now
            sticky.GetComponentInChildren<VRText>().StartListening();
        }
        else
        {
            Debug.Log("Too many sticky notes on work bar OR still editing a sticky note!");
        }
    }

    #region Create Colored Sticky Functions

    /// <summary>
    /// Will instantiate a sticky note and call edit sticky on the sticky note
    /// </summary>
    public void CreateRedSticky()
    {
        CreateStickyNote(m_RedColor);
    }

    /// <summary>
    /// Will instantiate a sticky note and call edit sticky on the sticky note
    /// </summary>
    public void CreateBlueSticky()
    {
        CreateStickyNote(m_BlueColor);
    }

    /// <summary>
    /// Will instantiate a sticky note and call edit sticky on the sticky note
    /// </summary>
    public void CreateGreenSticky()
    {
        CreateStickyNote(m_GreenColor);
    }

    /// <summary>
    /// Will instantiate a sticky note and call edit sticky on the sticky note
    /// </summary>
    public void CreatePurpleSticky()
    {
        CreateStickyNote(m_PurpleColor);
    }

    #endregion

    /// <summary>
    /// Will delete the whiteboard object
    /// </summary>
    public void DeleteWhiteboard()
    {
        Destroy(gameObject);
    }

    public void DoneEditingSticky()
    {
        m_DoneEditingStickyNote.Value = true;
    }
}
