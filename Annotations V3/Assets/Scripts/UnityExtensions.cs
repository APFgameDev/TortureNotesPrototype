using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnityExtensions
{
    public static float InverseLerp(this Vector3 a, Vector3 b, Vector3 value)
    {
        Vector3 AB = b - a;
        Vector3 AV = value - a;
        return Vector3.Dot(AV, AB) / Vector3.Dot(AB, AB);
    }

    public static void SafeSetParentNull(this Transform transform)
    {
        transform.parent = null;
    }
}
