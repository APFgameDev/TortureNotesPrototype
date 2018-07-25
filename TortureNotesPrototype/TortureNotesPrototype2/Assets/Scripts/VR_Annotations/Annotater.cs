using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NS_Annotation.NS_Data;

public class Annotater : MonoBehaviour
{ 
    [SerializeField]
    GameObject tagPreFab;

    Tag tag;
    TagHandler tagHandler;

    // Use this for initialization
    void Awake()
    {
        tagPreFab = Instantiate(tagPreFab);
        tagHandler = tagPreFab.GetComponent<TagHandler>();
        //tagHandler.PlaceTag(transform.position + tag.localPos, tag);
    }

    //public void SaveData()
    //{
    //    iOStyle.SaveData(tag);
    //}
}
