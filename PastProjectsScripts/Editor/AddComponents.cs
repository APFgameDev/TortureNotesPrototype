using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AddComponents : ScriptableWizard
{


    public GameObject m_ParentObject;
    public Component m_ComponentToCopy;



    [MenuItem("My Tools/ComponentAdder")]
    static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard<AddComponents>("Component Adder", "Add Components To Children", "Update selected");
    }

    void OnWizardCreate()
    {
        Transform[] children = m_ParentObject.GetComponentsInChildren<Transform>();

        Debug.Log("found " + children + " children");

        foreach (Transform trans in children)
        {
            UnityExtensionMethods.CopyComponent(trans.gameObject, m_ComponentToCopy);
        }
    }

    void OnWizardOtherButton()
    {
        if (Selection.activeTransform != null)
        {
        }
    }

    void OnWizardUpdate()
    {
    }

  
}
