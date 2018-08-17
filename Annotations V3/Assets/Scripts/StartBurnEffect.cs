using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StartBurnEffect : MonoBehaviour {


    [SerializeField]
    Material m_burnMaterial;

    [SerializeField]
    MeshRenderer[] m_objectsToBurn;

    [SerializeField]
    float m_burnTSpeed = 0.1f;

    float cutOffValue = -0.5f;

    [SerializeField]
    UnityEvent onDoneBurning;

    private void Start()
    {
        for (int i = 0; i < m_objectsToBurn.Length; i++)
        {
            if (m_objectsToBurn[i] != null)
            {
                Color color = m_objectsToBurn[i].material.color;

                m_objectsToBurn[i].material = m_burnMaterial;
                m_objectsToBurn[i].material.SetColor("_Color", color);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < m_objectsToBurn.Length; i++)
        {
            if(m_objectsToBurn[i] != null)
            m_objectsToBurn[i].material.SetFloat("_CutOffAmount", cutOffValue);
        }

        cutOffValue += Time.deltaTime;

        if(cutOffValue > 1.5f)
        {
            onDoneBurning.Invoke();
            gameObject.SetActive(false);
        }
    }
}
