using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(KeyboardKeySetup))]
public class KeyboardKeySetupEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        KeyboardKeySetup myScript = (KeyboardKeySetup)target;

        if (GUILayout.Button("Place Keys"))
        {
            myScript.SetKeys();
        }
    }
}