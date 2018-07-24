using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class Tools
{
    [MenuItem("GameObject/BetterUI/CreateText", false, 0)]
    private static void GetHelp()
    {
        GameObject obj = new GameObject("Text");
        Text text = obj.AddComponent<Text>();
        //Outline outline = obj.AddComponent<Outline>();
        
        GameObject parent = Selection.activeGameObject;

        if(parent != null)
        {
            obj.transform.SetParent(parent.transform);
            obj.transform.localPosition = Vector3.zero;
        }

        text.rectTransform.sizeDelta = new Vector2(160.0f, 30.0f);
        text.text = "New Text Test";
        text.color = Color.black;

        Font ArialFont = (Font)Resources.Load("Fonts/Arial");
        text.font = ArialFont;
        text.material = ArialFont.material;

        //outline.effectColor = Color.white;
        //outline.effectDistance = new Vector2(0.1f, 0.1f);
    }
}