using UnityEngine.XR;
using UnityEngine;
using Annotation.SO;

public class VRUser : MonoBehaviour
{
    [SerializeField]
    private KeyboardSO m_KeyboardSO;
    [SerializeField]
    private Transform m_LeftHand;
    [SerializeField]
    private Transform m_RightHand;

    private void Update()
    {
        m_LeftHand.localPosition = InputTracking.GetLocalPosition(XRNode.LeftHand);
        m_LeftHand.localRotation = InputTracking.GetLocalRotation(XRNode.LeftHand);

        m_RightHand.localPosition = InputTracking.GetLocalPosition(XRNode.RightHand);
        m_RightHand.localRotation = InputTracking.GetLocalRotation(XRNode.RightHand);
    }

    
}
