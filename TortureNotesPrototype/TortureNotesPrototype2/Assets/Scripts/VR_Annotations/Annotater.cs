using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NS_Annotation.NS_Data;

public class Annotater : MonoBehaviour
{
    [SerializeField]
    IIOStyle iOStyle;
    [SerializeField]
    GameObject tagPreFab;

    Tag tag;
    TagHandler tagHandler;

    // Use this for initialization
    //void Awake ()
    //{       
    //    tag = iOStyle.LoadData();
    //    tagPreFab = Instantiate(tagPreFab);
    //    tagHandler = tagPreFab.GetComponent<TagHandler>();
    //    tagHandler.PlaceTag(transform.position + tag.localPos, transform);
    //}
	
    public void SaveData()
    {
        iOStyle.SaveData(tag);
    }
}
