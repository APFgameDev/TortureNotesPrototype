using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Annotation.SO;

public class Follower : MonoBehaviour
{
    [SerializeField]
    private MatrixVariable m_TargetMatrix;

    [Range(0.0f, 1.0f)]
    public float m_SpeedCurve = 0.25f;

    [SerializeField]
    private float m_MinimumDistance = 0.01f;

    /// <summary>
    /// Callback for when the object has reached its destination
    /// </summary>
    public event System.Action OnFinishedMoving = delegate { };

    private void Update()
    {
        if (m_TargetMatrix != null && m_TargetMatrix.Value.ValidTRS())
        {
            Vector3 targetPosition = m_TargetMatrix.Value.MultiplyPoint(Vector3.zero);
            Quaternion targetRotation = m_TargetMatrix.Value.rotation;
            Vector3 targetScale = m_TargetMatrix.Value.lossyScale;

            Vector3 currentPosition = transform.position;
            Quaternion currentRotation = transform.rotation;
            Vector3 currentScale = transform.localScale;

            float lerpAmount = Mathf.Pow(Time.deltaTime, m_SpeedCurve);

            transform.position = Vector3.Lerp(currentPosition, targetPosition, lerpAmount);
            transform.rotation = Quaternion.Lerp(currentRotation, targetRotation, lerpAmount);
            transform.localScale = Vector3.Lerp(currentScale, targetScale, lerpAmount);

            //Check if we have arrived at our target destination
            float distanceBetweenPoints = Vector3.Distance(targetPosition, currentPosition);
            if (distanceBetweenPoints <= m_MinimumDistance)
            {
                OnFinishedMoving();
            }
        }
    }
}
