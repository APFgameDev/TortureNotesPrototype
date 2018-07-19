using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NS_CustomInspector
{
    [System.Serializable]
    public class ButtonData
    {
        public System.Action m_action;
        public string m_label;

        public ButtonData(System.Action action, string label)
        {
            m_action = action;
            m_label = label;
        }
    }
#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(EditorButtons), true)]
    [ExecuteInEditMode]
    public class EditorButton : UnityEditor.Editor
    {
        ButtonData[] buttonData;


        void OnEnable()
        {
            Debug.Log("Editor Button  Reassigned");
            EditorButtons myScript = (EditorButtons)target;
            buttonData = myScript.GetButtonData();
        }


        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (buttonData != null)
                for (int i = 0; i < buttonData.Length; i++)
                    if (GUILayout.Button(buttonData[i].m_label))
                        buttonData[i].m_action();
        }
    }
#endif

    public abstract class EditorButtons : MonoBehaviour
    {
        abstract public ButtonData[] GetButtonData();
    }
}
