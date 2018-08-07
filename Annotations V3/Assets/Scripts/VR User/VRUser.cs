using UnityEngine.XR;
using UnityEngine;

public class VRUser : MonoBehaviour
{
    [SerializeField]
    private Transform m_LeftHand;
    [SerializeField]
    private Transform m_RightHand;

    [SerializeField]
    private TransformSO m_RightHandTransformSO;
    [SerializeField]
    private TransformSO m_LeftHandTransformSO;

    private void Start()
    {
        if (m_LeftHand == null || m_LeftHandTransformSO == null || m_RightHand == null || m_RightHandTransformSO == null)
            Debug.LogError("Either the Hand's transform isn't present or the TransformSO isn't set on this object");

        m_RightHandTransformSO .Value = m_RightHand;
        m_LeftHandTransformSO.Value = m_LeftHand;
    }

    void Update()
    {
        m_LeftHand.localPosition = InputTracking.GetLocalPosition(XRNode.LeftHand);
        m_LeftHand.localRotation = InputTracking.GetLocalRotation(XRNode.LeftHand);

        m_RightHand.localPosition = InputTracking.GetLocalPosition(XRNode.RightHand);
        m_RightHand.localRotation = InputTracking.GetLocalRotation(XRNode.RightHand);
    }
}
