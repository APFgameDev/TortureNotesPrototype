using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEngine;

namespace NS_CustomInspector
{
#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(EditorUnionVariable), true)]
    public class UnionVariable : Editor
    {
        SerializedProperty m_serializedA;
        SerializedProperty m_serializedB;

        void OnEnable()
        {
            m_serializedA = serializedObject.FindProperty("m_varA");
            m_serializedB = serializedObject.FindProperty("m_varB");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();
            if (((EditorUnionVariable)target).m_displayA)
                EditorGUILayout.PropertyField(m_serializedA);
            else
                EditorGUILayout.PropertyField(m_serializedB);

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
    // dont use it 
    public class EditorUnionVariable :MonoBehaviour
    {
        public object m_varA;
        public object m_varB;
        public bool m_displayA;
    }
    //use this
    public class EditorVariableUnion<A, B> : EditorUnionVariable
    {
        [HideInInspector]
        public new A m_varA;
        [HideInInspector]
        public new B m_varB;
    }
}