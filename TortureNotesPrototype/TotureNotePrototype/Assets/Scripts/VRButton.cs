using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRButton : MonoBehaviour
{
    public Material SelectedMat;
    private Material OriginalMat;
    private MeshRenderer MRenderer;
    public UIManager UIMan;

    private void Start()
    {
        MRenderer = GetComponent<MeshRenderer>();
        OriginalMat = MRenderer.material;
        UIMan = FindObjectOfType<UIManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Right Hand")
        {
            UIMan.AddAnotation();
            MRenderer.material = SelectedMat;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Right Hand")
        {
            MRenderer.material = OriginalMat;
        }
    }

}
