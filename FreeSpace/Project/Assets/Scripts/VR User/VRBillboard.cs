using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

enum CorrectionStates
{
    Playing,
    Stopped
}

public class VRBillboard : MonoBehaviour
{
    private Transform m_Camera;

    [SerializeField]
    UnityEvent onStartBillBoardCorrection = new UnityEvent();

    [SerializeField]
    UnityEvent onEndBillBoardCorrection = new UnityEvent();

    [SerializeField]
    [Range(0.001f, 100f)]
    float m_lerpSpeed = 20f;

    Vector3 lookRotLerpedTo;

    CorrectionStates correctionState = CorrectionStates.Stopped;

    void Start ()
    {
        m_Camera = Camera.main.transform;
	}
	
	void Update ()
    {
        if ((lookRotLerpedTo - (transform.position - m_Camera.position)).sqrMagnitude > 0.1f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(transform.position - m_Camera.position, Vector3.up), m_lerpSpeed * Time.deltaTime);

            if(correctionState == CorrectionStates.Stopped)
                onStartBillBoardCorrection.Invoke();

            correctionState = CorrectionStates.Playing;
        }
        else
        {
            if (correctionState == CorrectionStates.Playing)
                onEndBillBoardCorrection.Invoke();

            correctionState = CorrectionStates.Stopped;
        }
	}
}
