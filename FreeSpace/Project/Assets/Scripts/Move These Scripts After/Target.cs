using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Annotation.SO;

public class Target : MonoBehaviour
{
    public MatrixVariable m_MyMatrix;
    
	private void Update ()
    {
		if(m_MyMatrix != null)
        {
            m_MyMatrix.Value = transform.localToWorldMatrix;
        }
	}
}
