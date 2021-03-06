﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Text), typeof(RectTransform))]
public class VREditableText : VRInteractable
{
    TextMeshProUGUI text;

    void Awake()
    {
        MathUtility.AddTriggerBoxToRectTransform(GetComponent<RectTransform>(), Vector3.forward * -0.1f, gameObject);
        text = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    { 
    }

    override public void OnHoverEnter(VRInteractionData vrInteraction)
    {
        if(enabled)
            vrInteraction.changeColor(Color.green);
    }

    override public void OnHoverExit(VRInteractionData vrInteraction)
    {
        if (enabled)
            vrInteraction.changeColor(Laser.DEFAULTLASERCOLOR);
    }

    public override void OnClick(VRInteractionData vrInteraction)
    {
        //enable keyboard set context
        if(enabled)
        {
            VRControllerInputManager.SetActive();
            //VRControllerInputManager.SetText(text);
        }
        else
        {
            VRControllerInputManager.SetActive();
        }
    }
}
