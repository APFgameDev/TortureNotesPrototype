using System.Collections;
using UnityEngine;

public class VRAnnotation : VRGrabbable
{
    [SerializeField]
    private LineRenderer m_LineRenderer;
    [SerializeField]
    Transform m_highlightNode;

    [SerializeField]
    Transform m_lineStartPos;

    private void Start()
    {
        m_LineRenderer.positionCount = 2;
    }


    public void StartUp(Vector3 clickPoint)
    {
        m_highlightNode.position = clickPoint;
    }

    public void Remove()
    {
        Destroy(transform.parent.gameObject);
    }

    public void StartAnnotationUpdate()
    {
        StartCoroutine(AnnotationUpdate());
    }

    public void EndAnnotationUpdate()
    {
        StopCoroutine(AnnotationUpdate());
    }

    IEnumerator AnnotationUpdate()
    {
        while (true)
        {
            m_LineRenderer.SetPositions(new Vector3[] { m_lineStartPos.position, m_highlightNode.position });
            yield return null;
        }
    }
}
