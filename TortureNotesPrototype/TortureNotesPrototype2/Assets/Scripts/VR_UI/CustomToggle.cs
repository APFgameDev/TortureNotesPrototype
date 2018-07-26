using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomToggle : Toggle
{
    protected override void Awake()
    {
        base.Awake();

        if (targetGraphic == null)
            return;
        targetGraphic.material = new Material(targetGraphic.material);

        //onValueChanged.AddListener(ChangeColor);
    }

    public void ChangeColor(bool toggleOn)
    {
        if (targetGraphic == null)
            return;

        if (isOn)
        {
            targetGraphic.material.color = colors.pressedColor;
        }
        else
        {
            targetGraphic.material.color = colors.normalColor;
        }
    }
}
