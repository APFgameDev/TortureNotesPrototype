using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

enum InputAxisIndex
{
    LeftIndexTrigger = 0,
    RightIndexTrigger,
    LeftGripTrigger,
    RightGripTrigger
}

class InputAxis
{
    public const string LeftIndexTrigger = "LeftIndexTrigger";
    public const string RightIndexTrigger = "RightIndexTrigger";
    public const string LeftGripTrigger = "LeftGripTrigger";
    public const string RightGripTrigger = "RightGripTrigger";

    public static readonly string[] InputAxisArray = { LeftIndexTrigger, RightIndexTrigger, LeftGripTrigger, RightGripTrigger};
}

struct InputButton
{
    public const string Button3 = "Button3";
}

public class VRSetterUpper : MonoBehaviour {
    [SerializeField]
    Transform leftHand;
    [SerializeField]
    Transform rightHand;

    [SerializeField]
    float leftGrip = 0;
    [SerializeField]
    float rightGrip = 0;
    [SerializeField]
    float leftTrig = 0;
    [SerializeField]
    float rightTrig = 0;

    // Update is called once per frame
    void Update () {
        leftHand.localPosition = InputTracking.GetLocalPosition(XRNode.LeftHand);
        leftHand.localRotation = InputTracking.GetLocalRotation(XRNode.LeftHand);
        rightHand.localPosition =  InputTracking.GetLocalPosition(XRNode.RightHand);
        rightHand.localRotation = InputTracking.GetLocalRotation(XRNode.RightHand);

        leftTrig = Input.GetAxis(InputAxis.LeftIndexTrigger);
        rightTrig = Input.GetAxis(InputAxis.RightIndexTrigger);
        leftGrip = Input.GetAxis(InputAxis.LeftGripTrigger);
        rightGrip = Input.GetAxis(InputAxis.RightGripTrigger);
    }

}
