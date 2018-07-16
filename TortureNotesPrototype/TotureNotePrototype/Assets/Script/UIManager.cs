using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public GameObject annotationPrefab;
    public Canvas AnnotationWindow;
    public ObjectSelector hand;

    private GameObject m_ObjectBeingAnnotated;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void OnObjectSelected(GameObject objBeingAnnotated)
    {
        m_ObjectBeingAnnotated = objBeingAnnotated;

        //AnnotationWindow.gameObject.SetActive(true);
        //AnnotationWindow.enabled = true;
    }


    public void AddAnotation()
    {
        //TODO: Activate the keyboard here and begin to allow the user to interact with the keyboard
    }

    public void DoneAnotation()
    {
        GameObject newAnnotation = Instantiate(annotationPrefab, m_ObjectBeingAnnotated.transform.position, m_ObjectBeingAnnotated.transform.rotation);
        

        hand.GrabObject(newAnnotation);
    }
}
