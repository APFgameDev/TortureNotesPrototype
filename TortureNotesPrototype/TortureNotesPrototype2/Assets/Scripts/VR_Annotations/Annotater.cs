using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NS_Annotation.NS_Data;

public class Annotater : MonoBehaviour
{
    Tag tag;
    [SerializeField]
    IIOStyle iOStyle;
    [SerializeField]
    GameObject prefabNode;

	// Use this for initialization
	void Awake ()
    {
        tag = iOStyle.LoadData();
	}
	
    public void SaveData()
    {
        iOStyle.SaveData(tag);
    }
}
