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

    [SerializeField]
    private TextMeshProUGUI m_Text;

    [SerializeField]
    private TextMeshProUGUI m_NumberOfReplies;

    [SerializeField]
    private Image m_BackgroundImage;

    private void Awake()
    {
        if(m_BackgroundImage == null)
        {
            m_BackgroundImage = GetComponent<Image>();
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

    public void OnHeldCallBack()
    {
        //Overlap box check for whiteboard
        //Call place sticky on whiteboard

        //Testing
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
}
