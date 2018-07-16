using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

struct InputAxis
{
    public const string LeftIndexTrigger = "LeftIndexTrigger";
    public const string RightIndexTrigger = "RightIndexTrigger";
    public const string LeftGripTrigger = "LeftGripTrigger";
    public const string RightGripTrigger = "RightGripTrigger";
}


public class VRSetterUpper : MonoBehaviour {
    [SerializeField]
    Transform leftHand;
    [SerializeField]
    Transform rightHand;
    [SerializeField]
    Vector3 posOffSet = new Vector3(0,1,0);
	
	// Update is called once per frame
	void Update () {
        leftHand.localPosition = InputTracking.GetLocalPosition(XRNode.LeftHand);
        leftHand.localRotation = InputTracking.GetLocalRotation(XRNode.LeftHand);
        rightHand.localPosition =  InputTracking.GetLocalPosition(XRNode.RightHand);
        rightHand.localRotation = InputTracking.GetLocalRotation(XRNode.RightHand);
    }

}
