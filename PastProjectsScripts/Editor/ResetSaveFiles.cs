using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ResetSaveFiles : ScriptableWizard
{


    [MenuItem("My Tools/ResetSaveFiles")]
    static void ResetFiles()
    {
        SaveManager.ResetFiles();
    }
}
