using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Annotater : MonoBehaviour
{
    List<AnnotationNode> annotationNodes = new List<AnnotationNode>();
    [SerializeField]
    IIOStyle iOStyle;
    [SerializeField]
    GameObject prefabNode;

	// Use this for initialization
	void Awake ()
    {
        annotationNodes = iOStyle.LoadData();
	}
	
    public void SaveData()
    {
        iOStyle.SaveData(annotationNodes);
    }
}
