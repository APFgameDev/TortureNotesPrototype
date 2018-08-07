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

    [Header("Do we want to lerp the colors?")]
    [SerializeField]
    private BoolVariable m_LerpColor;

    [Header("Main Keyboard")]
    [SerializeField]
    private KeyboardSO m_KeyboardSO;

    [SerializeField]
    private IntVariable m_HitAngle;

    private byte m_Count;
    private Color m_OriginalColor;
    private Material m_material;

    protected virtual void Awake()
    {
        m_material = GetComponent<MeshRenderer>().material;

        if (m_material != null)
        {
            m_OriginalColor = m_material.color;
        }
    }

    /// <summary>
    /// Called when the collider has been hit with the mallet
    /// </summary>
    protected abstract void OnHit();

    protected virtual void OnHoverExit()
    {
        //Only call the hover exit code for the last object triggering it
        if (m_Count <= 1)
        {
            ////Stop the previous coroutine
            //StopAllCoroutines();
            ////Go from the current color to the original color
            //StartCoroutine(LerpColors(m_material.color, m_OriginalColor, m_ColorHoverExitLerpTime.m_Value));

            SetMaterialColor(m_OriginalColor);
        }

        if (m_Count >= 1)
        {
            m_Count--;
        }

        Debug.Log("Hover Exit");
    }

    protected virtual void OnHoverEnter()
    {
        //Only call the hover enter code for the first object triggering it
        if (m_Count < 1)
        {
            ////Stop the previous coroutine
            //StopAllCoroutines();
            ////Go from the current color to the hover color
            //StartCoroutine(LerpColors(m_material.color, m_HoverColor.m_Value, m_ColorHoverEnterLerpTime.m_Value));

            SetMaterialColor(m_HoverColor.m_Value);
        }

        m_Count++;
    }

    private void OnCollisionEnter(Collision collision)
    {
        ////Stop the previous coroutine
        //StopCoroutine("LerpColors");
        ////Go from the current color to the hit color
        //StartCoroutine(LerpColors(m_material.color, m_HitColor.m_Value, m_ColorHitLerpTime.m_Value));

        SetMaterialColor(m_HitColor.m_Value);

        //Call the on hit
        OnHit();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Keyboard Mallet")
        {
            Vector3 direction = other.transform.position - transform.position;
            
            float angleBetween = Vector3.Angle(transform.up.normalized, direction.normalized);

            if (angleBetween < m_HitAngle.Value)
            {
                OnHoverEnter();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Keyboard Mallet")
        {
            OnHoverExit();
        }
    }

    /// <summary>
    /// Lerps from one color to an other color at a given duration
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    public IEnumerator LerpColors(Color from, Color to, float duration)
    {
        if (m_LerpColor.Value == true)
        {
            float timer = 0.0f;

            while (timer <= duration)
            {
                yield return null;

                m_material.color = Color.Lerp(from, to, timer / duration);
                timer += Time.deltaTime;
            }
        }
    }

    /// <summary>
    /// Will set the material's color to the color passed in
    /// </summary>
    /// <param name="newColor"></param>
    public void SetMaterialColor(Color newColor)
    {
        m_material.color = newColor;
    }
}

