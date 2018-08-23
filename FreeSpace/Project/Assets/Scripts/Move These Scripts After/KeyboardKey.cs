using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Annotation.SO;
using TMPro;
using Annotation.SO.UnityEvents;

public abstract class KeyboardKey : MonoBehaviour
{
    [Header("Colors for lerping")]
    [SerializeField]
    private ColorSO m_HoverColor;

    [SerializeField]
    private ColorSO m_HitColor;

    [Header("Main Keyboard")]
    [SerializeField]
    protected KeyboardSO m_KeyboardSO;

    [Header("Key Geometry to be animated")]
    [SerializeField] protected GameObject m_KeyGeometry;

    private byte m_TriggerCount;
    private Color m_OriginalColor;
    private Color m_PreviousColor;

    readonly static Color m_disabledColor = Color.clear;
    private Material m_Material;

    [Header("Animation variables")]
    [SerializeField] protected Vector3 m_NormalPos;
    [SerializeField] protected float m_KeyPressYPos = -3.30f;
    [SerializeField] protected float m_AnimationSpeed = 5.0f;
    [SerializeField] protected bool m_AnimateKey;
    [SerializeField] MeshRenderer m_KeyButtonMesh;
    protected bool m_IsAnimating = false;

    [Header("Can you hold the key down for continous input?")]
    [SerializeField] private bool m_SpamKey = true;

    [Header("Audio Manager SO")]
    [SerializeField] private AudioManagerSO m_AudioManagerSO;

    [Header("Text Object Associated with this Key")]
    [SerializeField] protected TextMeshProUGUI m_TextObject;

    [Header("Are we in the keyboard options?")]
    [SerializeField] protected BoolVariable m_IsInKeyboardOptions;
    [Header("Allow this key's input while in the options menu?")]
    [SerializeField] protected bool m_OverrideKeyboardOptions = false;

    //Spam key input variables
    private bool m_StartKeyDowntimer = false;
    private float m_HeldKeyDownTimer;
    private float m_HeldKeyDownTimerAmount = 0.6f;

    private float m_SpamKeyTimer;
    private float m_SpamKeyTimerAmount = 0.1f;

    [SerializeField]
    UnityEventVoidSO onOptionsOpened;
    [SerializeField]
    UnityEventVoidSO onOptionsClosed;

    protected virtual void Awake()
    {
        onOptionsOpened.UnityEvent.AddListener(delegate { SetMaterialColor(m_disabledColor); });
        onOptionsClosed.UnityEvent.AddListener(delegate { SetMaterialColor(m_OriginalColor); });


        m_HeldKeyDownTimer = m_HeldKeyDownTimerAmount;
        m_SpamKeyTimer = m_SpamKeyTimerAmount;

        if (m_KeyButtonMesh != null)
        {
            m_Material = m_KeyButtonMesh.material;
        }
        else
        {
            Debug.Log("Key Button Mesh has not been set in " + this);
        }

        if (m_KeyButtonMesh != null)
        {
            m_OriginalColor = m_Material.color;
            m_PreviousColor = m_OriginalColor;
        }
    }

    protected virtual void OnDisable()
    {
        SetMaterialColor(m_PreviousColor);
    }

    protected virtual void OnEnable()
    {
        m_KeyGeometry.transform.localPosition = m_NormalPos;
    }

    /// <summary>
    /// Called when the collider has been hit with the mallet
    /// </summary>
    protected abstract void OnHit();

    protected virtual void Update()
    {
        //Check if the key is being held down
        //If so and they have it held for more than 1 second, spam that key's input
        if (m_SpamKey == true)
        {
            if (m_StartKeyDowntimer == true)
            {
                m_HeldKeyDownTimer -= Time.deltaTime;

                if (m_HeldKeyDownTimer <= 0.0f)
                {
                    m_SpamKeyTimer -= Time.deltaTime;

                    //Spam the keys at a giving time
                    if (m_SpamKeyTimer <= 0.0f)
                    {
                        OnHit();
                        m_SpamKeyTimer = m_SpamKeyTimerAmount;
                    }
                }
            }
        }

        //If we want the key to animate
        if (m_AnimateKey == true)
        {
            if (m_IsAnimating)
            {
                m_KeyGeometry.transform.localPosition = Vector3.Lerp(m_KeyGeometry.transform.localPosition, m_NormalPos, Time.deltaTime * m_AnimationSpeed);

                //Check if we have arrived at our target destination
                float distanceBetweenPoints = Vector3.Distance(m_KeyGeometry.transform.localPosition, m_NormalPos);
                if (distanceBetweenPoints <= 0.01f)
                {
                    m_IsAnimating = false;
                }
            }
        }
    }

    #region OnHover Functions
    /// <summary>
    /// Called when the mallet enters the trigger box of the key
    /// </summary>
    public virtual void OnHoverEnter()
    {
        if (m_IsInKeyboardOptions.Value == true && m_OverrideKeyboardOptions == false)
        {
            return;
        }

        //Only call the hover enter code for the first object triggering it
        if (m_TriggerCount < 1)
        {
            SetMaterialColor(m_HoverColor.m_Value);
        }

        m_TriggerCount++;
    }

    /// <summary>
    /// Called when the mallet exits the trigger box of the key
    /// </summary>
    public virtual void OnHoverExit()
    {
        if (m_IsInKeyboardOptions.Value == true && m_OverrideKeyboardOptions == false)
        {
            return;
        }

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
    #endregion

    #region OnHit Functions

    /// <summary>
    /// Called from the child box collider when the mallet enters the box collider of the key
    /// </summary>
    public virtual void OnHitEnter(Vector3 direction)
    {
        if (m_IsInKeyboardOptions.Value == true && m_OverrideKeyboardOptions == false)
        {
            return;
        }

        float dot = Vector3.Dot(direction.normalized, transform.up);

        //Are we above the key?
        if (dot >= 0.5f)
        {
            SetMaterialColor(m_HitColor.m_Value);

            //Call the on hit
            OnHit();

            //Play the clip
            m_AudioManagerSO.m_AudioManager.PlaySound();

            //Start the timer
            m_StartKeyDowntimer = true;
        }
    }

    /// <summary>
    /// Called from the child box collider when the mallet exits the box collider of the key
    /// </summary>
    public virtual void OnHitExit()
    {
        if (m_IsInKeyboardOptions.Value == true && m_OverrideKeyboardOptions == false)
        {
            return;
        }

        if (m_TriggerCount < 1)
        {
            SetMaterialColor(m_PreviousColor);
        }
        else
        {
            SetMaterialColor(m_HoverColor.m_Value);
        }

        //Start the animation
        m_IsAnimating = true;

        //Reset the timer
        m_StartKeyDowntimer = false;
        m_HeldKeyDownTimer = m_HeldKeyDownTimerAmount;
    }

    #endregion

    #region Utility Functions

    /// <summary>
    /// Will set the material's color to the color passed in. Also sets the previous color
    /// </summary>
    /// <param name="newColor"></param>
    public void SetMaterialColor(Color newColor)
    {
        m_Material.color = newColor;
    }
    #endregion
}

