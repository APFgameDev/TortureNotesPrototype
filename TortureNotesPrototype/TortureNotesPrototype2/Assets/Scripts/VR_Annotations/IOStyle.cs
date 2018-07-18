using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IIOStyle
{
    void SaveData(List<AnnotationNode> annotationNode);
    List<AnnotationNode> LoadData();
}
