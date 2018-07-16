using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public GameObject annotationPrefab;
    public GameObject AnnotationWindow;
    public ObjectSelector hand;
    public GameObject Keyboard;
    public Text keyboardText;

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

        AnnotationWindow.SetActive(true);
    }


    public void AddAnotation()
    {
        Keyboard.SetActive(true);
    }

    public void DoneAnotation()
    {
        GameObject newAnnotation = Instantiate(annotationPrefab, m_ObjectBeingAnnotated.transform.position, m_ObjectBeingAnnotated.transform.rotation);
        newAnnotation.GetComponent<Annotation>().textComp.text = keyboardText.text;
        hand.GrabObject(newAnnotation);
    }
}
