using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Annotation.SO;

[CustomEditor(typeof(VoidEvent))]
public class VoidEventEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUI.enabled = Application.isPlaying;

        VoidEvent e = target as VoidEvent;
        if (GUILayout.Button("Publish"))
            e.Publish();
    }
}
