using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Selectable))]
public class ToggleColorMeshChange : MonoBehaviour {
    Selectable m_selectable;
    [SerializeField]
    MeshRenderer[] m_meshes;

    [SerializeField]
    [Range(0.001f,5f)]
    float m_transitionTime = 0.2f;

    private void Awake()
    {
        m_selectable = GetComponent<Selectable>();
        UpdateColorToNormal();
    }

    public void UpdateColorToHighlighted()
    {
        StopAllCoroutines();

        StartCoroutine(LerpToColor(m_selectable.colors.highlightedColor));
    }

    public void UpdateColorToNormal()
    {
        StopAllCoroutines();

        StartCoroutine(LerpToColor(m_selectable.colors.normalColor));
    }


    public void UpdateColorToDisabled()
    {
        StopAllCoroutines();

        StartCoroutine(LerpToColor(m_selectable.colors.disabledColor));
    }

    IEnumerator LerpToColor(Color colorToLerpTo)
    {
        Color[] ogColors = new Color[m_meshes.Length];

        for (int i = 0; i < m_meshes.Length; i++)
            ogColors[i] = m_meshes[i].material.color;

        float time = 0;


        while(time < m_transitionTime)
        {
            time += Time.deltaTime;

            for (int i = 0; i < m_meshes.Length; i++)
                m_meshes[i].material.color = Color.Lerp(ogColors[i], colorToLerpTo,time/ m_transitionTime);

            yield return null;
        }

    }
}
