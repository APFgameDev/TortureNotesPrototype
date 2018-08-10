using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathHelpers
{
    public static bool CompareFloats(float valueA, float valueB)
    {
        if (Mathf.Abs(valueA - valueB) < Mathf.Epsilon)
            return true;

        return false;
    }
}
