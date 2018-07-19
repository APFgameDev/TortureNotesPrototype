using NS_CustomInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadLevelImmediatly : EditorVariableUnion<string, NS_Game.GameConsts.SceneBuildIndex>
{
    private void Update()
    {
        if (m_displayA)
            UnityEngine.SceneManagement.SceneManager.LoadScene(m_varA);
        else
            UnityEngine.SceneManagement.SceneManager.LoadScene((int)m_varB);
    }
}
