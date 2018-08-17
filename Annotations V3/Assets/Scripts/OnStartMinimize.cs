using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnStartMinimize : MonoBehaviour {

    [SerializeField]
    ScaleRect[] rectsToSetMinimize;


    [SerializeField]
    ScaleRect[] rectsToStartMaximize;

    [SerializeField]
    ScaleRect[] rectsToSetMaximize;


    [SerializeField]
    ScaleRect[] rectsToStartMinimize;

    private void Start()
    {
        for (int i = 0; i < rectsToSetMinimize.Length; i++)
        {
            rectsToSetMinimize[i].SetToMinScale();
        }

        for (int i = 0; i < rectsToSetMaximize.Length; i++)
        {
            rectsToSetMaximize[i].SetToMaxScale();
        }

        for (int i = 0; i < rectsToStartMaximize.Length; i++)
        {
            rectsToStartMaximize[i].ScaleRectToMax();
        }
       
        for (int i = 0; i < rectsToStartMinimize.Length; i++)
        {
            rectsToStartMinimize[i].ScaleRectToMin();
        }
    }
}
