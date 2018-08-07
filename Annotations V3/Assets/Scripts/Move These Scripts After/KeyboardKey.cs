using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Annotation.SO;

public abstract class KeyboardKey : MonoBehaviour
{
    [Header("Colors for lerping")]
    [SerializeField]
    private ColorSO m_HoverColor;

    [SerializeField]
    private ColorSO m_HitColor;

    [Header("Timing for color lerping")]
    [SerializeField]
    private FloatRangeSO m_ColorHoverEnterLerpTime;

    [SerializeField]
    private FloatRangeSO m_ColorHoverExitLerpTime;

    [SerializeField]
    private FloatRangeSO m_ColorHitLerpTime;

    [Header("Main Keyboard")]
    [SerializeField]
    protected KeyboardSO m_KeyboardSO;

    [SerializeField]
    private IntVariable m_HitAngle;

    private byte m_TriggerCount;
    private Color m_OriginalColor;
    private Color m_PreviousColor;
    private Material m_Material;

    protected virtual void Awake()
    {
        m_Material = GetComponent<MeshRenderer>().material;

        if (m_Material != null)
        {
            m_OriginalColor = m_Material.color;
            m_PreviousColor = m_OriginalColor;
        }
    }

    /// <summary>
    /// Called when the collider has been hit with the mallet
    /// </summary>
    protected abstract void OnHit();

    #region OnHover Functions

    /// <summary>
    /// Called when the mallet exits the trigger box of the key
    /// </summary>
    protected virtual void OnHoverExit()
    {
        //Only call the hover exit code for the last object triggering it
        if (m_TriggerCount <= 1)
        {
            SetMaterialColor(m_OriginalColor);
        }

        if (m_TriggerCount >= 1)
        {
            m_TriggerCount--;
        }
    }

    /// <summary>
    /// Called when the mallet enters the trigger box of the key
    /// </summary>
    protected virtual void OnHoverEnter()
    {
        //Only call the hover enter code for the first object triggering it
        if (m_TriggerCount < 1)
        {
            SetMaterialColor(m_HoverColor.m_Value);
        }

        m_TriggerCount++;
    }

    #endregion

    #region OnHit Functions

    /// <summary>
    /// Called from the child box collider when the mallet enters the box collider of the key
    /// </summary>
    public virtual void OnHitEnter(Collider other)
    {
        Vector3 direction = other.gameObject.transform.position - transform.position;

        float angleBetween = Vector3.Angle(transform.up.normalized, direction.normalized);

        if (angleBetween < m_HitAngle.Value)
        {
            SetMaterialColor(m_HitColor.m_Value);

            //Call the on hit
            OnHit();
        }
    }

    /// <summary>
    /// Called from the child box collider when the mallet exits the box collider of the key
    /// </summary>
    public virtual void OnHitExit(Collider other)
    {
        SetMaterialColor(m_PreviousColor);
    }

    #endregion

    #region OnPhysics Functions

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Keyboard Mallet")
        {
            OnHoverEnter();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Keyboard Mallet")
        {
            OnHoverExit();
        }
    }

    #endregion

    #region Utility Functions

    /// <summary>
    /// Will set the material's color to the color passed in. Also sets the previous color
    /// </summary>
    /// <param name="newColor"></param>
    public void SetMaterialColor(Color newColor)
    {
        m_PreviousColor = m_Material.color;
        m_Material.color = newColor;
    }

    #endregion
}

