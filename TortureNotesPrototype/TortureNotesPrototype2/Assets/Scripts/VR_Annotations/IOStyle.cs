using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NS_Annotation.NS_Data;


public interface IIOStyle
{
    void SaveData(List<AnnotationNode> annotationNode);
    List<AnnotationNode> LoadData();
}
