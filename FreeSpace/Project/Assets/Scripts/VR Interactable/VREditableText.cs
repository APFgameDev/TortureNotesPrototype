﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Annotation
{
    [RequireComponent(typeof(Text), typeof(RectTransform))]
    public class VREditableText : VR_UI_Interactable
    {
        TextMeshProUGUI text;

        private void Awake()
        {
            text = GetComponent<TextMeshProUGUI>();
            InitVR_UI_Interactable();
        }

        override public void OnHoverEnter(VRInteractionData vrInteraction)
        {
            vrInteraction.ChangeColor(Color.green);
        }

        override public void OnHoverExit(VRInteractionData vrInteraction)
        {
            vrInteraction.ChangeColor(Laser.DEFAULT_LASER_COLOR);
        }

        public override void OnClick(VRInteractionData vrInteraction)
        { 
        }
    }
}
