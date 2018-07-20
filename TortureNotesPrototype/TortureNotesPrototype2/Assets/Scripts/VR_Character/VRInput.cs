using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum InputAxisIndex
{
    LeftStickHorizontal,
    LeftStickVertical,
    RightStickHorizontal,
    RightStickVertical,
    LeftIndexTrigger,
    RightIndexTrigger,
    LeftGripTrigger,
    RightGripTrigger
}

class InputAxis
{
    public const string LeftStickHorizontal = "LeftStickHorizontal";
    public const string LeftStickVertical = "LeftStickVertical";
    public const string RightStickHorizontal = "RightStickHorizontal";
    public const string RightStickVertical = "RightStickVertical";
    public const string LeftIndexTrigger = "LeftIndexTrigger";
    public const string RightIndexTrigger = "RightIndexTrigger";
    public const string LeftGripTrigger = "LeftGripTrigger";
    public const string RightGripTrigger = "RightGripTrigger";

    public static readonly string[] InputAxisArray = { LeftStickHorizontal, LeftStickVertical, RightStickHorizontal, RightStickVertical, LeftIndexTrigger, RightIndexTrigger, LeftGripTrigger, RightGripTrigger };
}

struct InputButton
{
    public const string Button3 = "Button3";
}