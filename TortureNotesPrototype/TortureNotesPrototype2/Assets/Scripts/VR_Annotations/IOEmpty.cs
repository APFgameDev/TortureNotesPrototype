using System.Collections;
using System.Collections.Generic;
using NS_Annotation.NS_Data;
using UnityEngine;

public class IOEmpty : IIOStyle
{
    public Tag LoadData()
    {
        return new Tag();
    }

    public void SaveData(Tag annotationNode)
    {
    }
}
