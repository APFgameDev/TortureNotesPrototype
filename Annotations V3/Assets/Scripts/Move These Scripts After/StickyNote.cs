using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Annotation.SO;
using TMPro;

public class StickyNote : MonoBehaviour
{
    [SerializeField]
    private KeyboardSO m_KeyboardSO;

    [SerializeField]
    private Whiteboard m_Whiteboard;

    [SerializeField]
    private Follower m_Follower;

    [Header("StickyNote Text")]
    [SerializeField]
    private TextMeshProUGUI m_Text;

    [Header("StickyNote Background Panel Image")]
    [SerializeField]
    private Image m_BackgroundImage;

    [Header("Sticky flying around Matrices")]
    [SerializeField]
    private MatrixVariable m_KeyboardMatrix;

    private void Awake()
    {
        if(m_BackgroundImage == null)
        {
            m_BackgroundImage = GetComponent<Image>();
        }

        if(m_Follower == null)
        {
            m_Follower = GetComponent<Follower>();
        }
    }

    /// <summary>
    /// Will set the background color of the sticky note
    /// </summary>
    /// <param name="color"></param>
    public void SetColor(Color color)
    {
        m_BackgroundImage.color = color;
    }

    /// <summary>
    /// Will turn on the keyboard.
    /// Will send the sticky to the keyboard
    /// </summary>
    public void EditSticky()
    {
        m_KeyboardSO.InvokeTurnOn();
        SetStickyTarget(m_KeyboardMatrix);
    }

    /// <summary>
    /// Sets the target of the follower script in the sticky note to the target passed in
    /// </summary>
    /// <param name="target"></param>
    public void SetStickyTarget(MatrixVariable target)
    {
        m_Follower.enabled = true;
        m_Follower.SetTarget(target);
    }

    public void OnHeldCallBack()
    {
        //Overlap box check for whiteboard
        //Call place sticky on whiteboard

        //m_Whiteboard.AddStickyNote(this);
    }

    public void OnGrabReleasedCallBack()
    {
        // Lerp background local pos to zero.
        // Check for trash can overlap.
        // Refold comments
    }

    /// <summary>
    /// Edit the sticky note
    /// </summary>
    public void OnClickCallBack()
    {

    }

    public void OnGrabUnfoldComments()
    {

    }

    public void SetWhiteboard(Whiteboard whiteboard)
    {
        m_Whiteboard = whiteboard;
    }

    /// <summary>
    /// Will set its transform back to normal and tell the follower script to stop
    /// </summary>
    public void SetBackToParent()
    {
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;
        m_Follower.enabled = false;
    }
}
