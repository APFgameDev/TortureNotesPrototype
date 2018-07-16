using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public Canvas AnnotationWindow;

    void Start ()
    {
		if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
	}
	
	void Update ()
    {
		
	}

    public void OnObjectSelected()
    {
       
    }
}
