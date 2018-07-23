using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text), typeof(RectTransform))]
public class VREditableText : VRInteractable
{
    Text text;

    void Awake()
    {
        MathUtility.AddTriggerBoxToRectTransform(GetComponent<RectTransform>());
        text = GetComponent<Text>();
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

        }
    }
}
