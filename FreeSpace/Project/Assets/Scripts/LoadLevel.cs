using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{
    bool loadFirstScene = true;


    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }


    // Update is called once per frame
    void Update () {

        if(loadFirstScene)
        {
            SceneManager.LoadScene(1);
            loadFirstScene = false;
        }

		if(Input.GetKey(KeyCode.Alpha1))
        {
            SceneManager.LoadScene(1);
        }

        if (Input.GetKey(KeyCode.Alpha2))
        {
            SceneManager.LoadScene(2);
        }

        if (Input.GetKey(KeyCode.Alpha3))
        {
            SceneManager.LoadScene(3);
        }
    }
}
