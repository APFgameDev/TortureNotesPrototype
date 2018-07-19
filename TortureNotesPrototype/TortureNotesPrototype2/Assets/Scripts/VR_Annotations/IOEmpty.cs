using System.Collections;
using System.Collections.Generic;
using NS_Annotation.NS_Data;
using UnityEngine;

public class IOEmpty : IIOStyle {
    public List<AnnotationNode> LoadData()
    {
        return new List<AnnotationNode>();
    }

    public void SaveData(List<AnnotationNode> annotationNode)
    {
    }
}
