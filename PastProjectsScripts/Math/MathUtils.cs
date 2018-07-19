
using UnityEngine;
using System;

public static class MathUtils {

    public static float CompareEpsilon = 0.00001f;

    //Eases from the start to the end.  This is meant to be called over many frames.  The
    //values will change fast at first and gradually slow down.
    public static float LerpTo(float easeSpeed, float start, float end, float dt)
    {
        float diff = end - start;

        diff *= Mathf.Clamp(dt * easeSpeed, 0.0f, 1.0f);

        return diff + start;
    }

    //Eases from the start to the end.  This is meant to be called over many frames.  The
    //values will change fast at first and gradually slow down.
    public static Vector3 LerpTo(float easeSpeed, Vector3 start, Vector3 end, float dt)
    {
        Vector3 diff = end - start;

        diff *= Mathf.Clamp(dt * easeSpeed, 0.0f, 1.0f);

        return diff + start;
    }



    public static float SmoothLerp(float aStart, float aEnd, float aT)
    {
        float t = SmoothT(aT);

        return Mathf.Lerp(aStart, aEnd, t);
    }

    public static Color SmoothLerp(Color aStart, Color aEnd, float aT)
    {
        float t = SmoothT(aT);

        return Color.Lerp(aStart, aEnd, t);
    }

    public static Vector3 SmoothLerp(Vector3 aStart, Vector3 aEnd, float aT)
    {
        float t = SmoothT(aT);

        return Vector3.Lerp(aStart, aEnd, t);
    }

    public static Quaternion SmoothLerp(Quaternion aStart, Quaternion aEnd, float aT)
    {
        float t = SmoothT(aT);

        return Quaternion.Lerp(aStart, aEnd, t);
    }

    //for linear interpolation, eases in and out
    public static float SmoothT(float aT)
    {
        return aT * aT * aT * (aT * (6f * aT - 15f) + 10f);
    }

    public static float Sinerp(float aStart, float aEnd, float aT)
    {
        float t = EaseOutT(aT);

        return Mathf.Lerp(aStart, aEnd, t);
    }

    public static Vector3 Sinerp(Vector3 aStart, Vector3 aEnd, float aT)
    {
        float newT = EaseOutT(aT);

        return Vector3.Lerp(aStart, aEnd, newT);
    }

    public static Quaternion Sinerp(Quaternion aStart, Quaternion aEnd, float aT)
    {
        float t = SmoothT(aT);

        return Quaternion.Lerp(aStart, aEnd, t);
    }

        //for linear interpolation, eases out
        public static float EaseOutT(float aT)
    {
        return Mathf.Sin(aT * Mathf.PI * 0.5f);
    }

    public static float EasInT(float aT)
    {
        return 1f - Mathf.Cos(aT * Mathf.PI * 0.5f);
    }

    public static float Coserp(float aStart, float aEnd, float aT)
    {
        float t = EasInT(aT);

        return Mathf.Lerp(aStart, aEnd, t);
    }

	public static float SuperT(float aT, float aPowerOf)
	{
		float t = EasInT(aT);

		return Mathf.Pow(t, aPowerOf);
	}

    public static float SuperCoserp(float aStart, float aEnd, float aPowerOf, float aT)
    {
		float t = SuperT (aT, aPowerOf);

        return Mathf.Lerp(aStart, aEnd, t);
    }

    //Eases from the start to the end.  This is meant to be called over many frames.  The
    //values will change fast at first and gradually slow down.
    public static Vector3 SlerpTo(float easeSpeed, Vector3 start, Vector3 end, float dt)
    {
        float percent = Mathf.Clamp(dt * easeSpeed, 0.0f, 1.0f);

        return Vector3.Slerp(start, end, percent);
    }

    //Eases from the start to the end.  This is meant to be called over many frames.  The
    //values will change fast at first and gradually slow down.
    public static Vector3 SlerpTo(float easeSpeed, Vector3 start, Vector3 end, Vector3 slerpCenter, float dt)
    {
        Vector3 startOffset = start - slerpCenter;
        Vector3 endOffset = end - slerpCenter;

        float percent = Mathf.Clamp(dt * easeSpeed, 0.0f, 1.0f);

        return Vector3.Slerp(startOffset, endOffset, percent) + slerpCenter;
    }

    //Eases from the start to the end.  This is meant to be called over many frames.  The
    //values will change fast at first and gradually slow down.
    public static Quaternion LerpTo(float easeSpeed, Quaternion start, Quaternion end, float dt)
    {
        float percent = Mathf.Clamp(dt * easeSpeed, 0.0f, 1.0f);

        return Quaternion.Slerp(start, end, percent);
    }

    //Use this to compare floating point numbers, when you want to allow for a small degree of error
    public static bool AlmostEquals(float v1, float v2, float epsilon)
    {
        return Mathf.Abs(v2 - v1) <= epsilon;
    }

    //Use this to compare floating point numbers, when you want to allow for a small degree of error
    public static bool AlmostEquals(float v1, float v2)
    {
        return AlmostEquals(v1, v2, CompareEpsilon);
    }

    public static bool AlmostEquals(Vector3 v1, Vector3 v2, float epsilon)
    {
        bool almostEqualsX = false;
        bool almostEqualsY = false;
        bool almostEqualsZ = false;

        almostEqualsX = AlmostEquals(v1.x, v2.x, epsilon);
        almostEqualsY = AlmostEquals(v1.y, v2.y, epsilon);
        almostEqualsZ = AlmostEquals(v1.z, v2.z, epsilon);

        return almostEqualsX && almostEqualsY && almostEqualsZ;
    }

  

    //Clamps a vector along the x-z plane
    public static Vector3 HorizontalClamp(Vector3 v, float maxLength)
    {
        float horizLengthSqrd = v.x * v.x + v.z * v.z;

        if (horizLengthSqrd <= maxLength * maxLength)
        {
            return v;
        }

        float horizLength = Mathf.Sqrt(horizLengthSqrd);

        v.x *= maxLength / horizLength;
        v.z *= maxLength / horizLength;

        return v;
    }

    //Eases from the start to the end.  This is meant to be called over many frames.  The
    //values will change fast at first and gradually slow down.
    public static Vector3 SlerpToHoriz(float easeSpeed, Vector3 start, Vector3 end, Vector3 slerpCenter, float dt)
    {
        Vector3 startOffset = start - slerpCenter;
        Vector3 endOffset = end - slerpCenter;

        startOffset.y = 0.0f;
        endOffset.y = 0.0f;

        float percent = Mathf.Clamp(dt * easeSpeed, 0.0f, 1.0f);

        Vector3 result = Vector3.Slerp(startOffset, endOffset, percent) + slerpCenter;
        result.y = start.y;

        return result;
    }


    //This function will project a point inside a capsule to the bottom of the capsule. 
    //The capsule is assumed to be oriented along the y-axis.
    public static Vector3 ProjectToBottomOfCapsule(
        Vector3 ptToProject,
        Vector3 capsuleCenter,
        float capsuleHeight,
        float capsuleRadius
        )
    {
        //Calculating the length of the line segment part of the capsule
        float lineSegmentLength = capsuleHeight - 2.0f * capsuleRadius;

        //Clamp line segment length
        lineSegmentLength = Math.Max(lineSegmentLength, 0.0f);

        //Calculate the line segment that goes along the capsules "Height"
        Vector3 bottomLineSegPt = capsuleCenter;
        bottomLineSegPt.y -= lineSegmentLength * 0.5f;

        //Get displacement from bottom of line segment
        Vector3 ptDisplacement = ptToProject - bottomLineSegPt;

        //Calculate needed distances
        float horizDistSqrd = ptDisplacement.x * ptDisplacement.x + ptDisplacement.z * ptDisplacement.z;

        float radiusSqrd = capsuleRadius * capsuleRadius;

        //The answer will be undefined if the pt is horizontally outside of the capsule
        if (horizDistSqrd > radiusSqrd)
        {
            return ptToProject;
        }

        //Calc projected pt
        float heightFromSegPt = -Mathf.Sqrt(radiusSqrd - horizDistSqrd);

        Vector3 projectedPt = ptToProject;
        projectedPt.y = bottomLineSegPt.y + heightFromSegPt;

        return projectedPt;
    }

    //Returns the angle of the surface in degrees
    public static float CalcSurfaceAngle(Vector3 normal)
    {
        //                 /|
        //                / |
        //               /  |
        //            h /   |
        //             /    | o
        //            /     |
        //           /ang   |
        //          /_______|
        //
        //sin(ang) = o/h, but since the normal is a unit vector h = 1
        //The angle will be: Asin(o)
        return Mathf.Rad2Deg * Mathf.Asin(normal.y);
    }

    //does not work for angles less than -360
    public static float WrapAngleAround360(float aAngle)
    {
        float newAngle = aAngle;

        if (newAngle < 0.0f)
        {
            newAngle += 360.0f;
            return newAngle;
        }

        if (newAngle > 360.0f)
            newAngle %= 360.0f;

        return newAngle;
    }

    //Calculate where a target will be in the future. Does not account for acceleration.
    //returns the direction vector that you would want for your "projectile" to hit (given the target stays at the exact same velocity the whole time)
    public static Vector3 CalculateFutureTargetLocation(Vector3 aProjectilePos, Vector3 aTargetPos, Vector3 aTargetVelocity, float aProjectileSpeed)
    {
        //cos of theta (angle between targets dirvec and target to projectile dirvec)
        float dot = Vector3.Dot(Vector3.Normalize(aProjectilePos - aTargetPos), aTargetVelocity.normalized);

        //distance from projectilie to target at start
        float startDist = Vector3.Distance(aTargetPos, aProjectilePos);

        float targetSpeed = aTargetVelocity.magnitude;

        //quadratic formula
        float a = (aProjectileSpeed * aProjectileSpeed) - (targetSpeed * targetSpeed);
        float b = 2f * startDist * targetSpeed * dot;
        float c = -(startDist * startDist);

        float t1 = (-b + Mathf.Sqrt((b * b) + (4f * a * -c))) / (2f * a);
        float t2 = (-b - Mathf.Sqrt((b * b) + (4f * a * -c))) / (2f * a);


        //ignore negatives
        if (t1 < 0f)
            t1 = float.MaxValue;
        if (t2 < 0f)
            t2 = float.MaxValue;

        //ignore the negative value, or choose the t that will hit faster
        float realT = Mathf.Min(t1, t2);

        //get the projectile velocity
        return aTargetVelocity + ((aTargetPos - aProjectilePos) / realT);
    }

    #region WaterHelpers
    //Converts 2D coordinates into 1D coordinate.  This can be used for 1D arrays that are
    //holding 2D grids of values.
    public static int Calc1DIndex(int x, int y, int gridDimensionY)
    {
        return x * gridDimensionY + y;
    }

    public static Vector2 RotateVec2(Vector2 v, float degrees)
    {
        float rads = Mathf.Deg2Rad * degrees;

        float sinAngle = Mathf.Sin(rads);
        float cosAngle = Mathf.Cos(rads);

        return new Vector2(
            v.x * cosAngle - v.y * sinAngle,
            v.x * sinAngle + v.y * cosAngle
            );
    }

    //This will get the closest point on a sphere. 
    public static bool IsPointInSphere(
        Vector3 samplePt,
        Vector3 sphereCenter,
        float sphereRadius
        )
    {
        //Calculate the projection direction to the sample point
        Vector3 displacementDir = samplePt - sphereCenter;

        float distToSphereSqrd = displacementDir.sqrMagnitude;

        return distToSphereSqrd <= sphereRadius * sphereRadius;
    }

    //This will get the closest point on a capsule. 
    public static bool IsPointInCapsule(
        Vector3 samplePt,
        Vector3 capsuleCenter,
        float capsuleHeight,
        float capsuleRadius
        )
    {
        //Calculating the length of the line segment part of the capsule
        float lineSegmentLength = capsuleHeight - 2.0f * capsuleRadius;

        //if the linesegment lenght is less than or equal to zero just treat it like a sphere
        if (lineSegmentLength <= 0.0f)
        {
            return IsPointInSphere(samplePt, capsuleCenter, capsuleRadius);
        }

        //Calculate the line segment that goes along the capsules "Height"
        Vector3 lineSegPt1 = capsuleCenter;
        Vector3 lineSegPt2 = capsuleCenter;

        lineSegPt1.y += lineSegmentLength * 0.5f;
        lineSegPt2.y -= lineSegmentLength * 0.5f;

        Vector3 closestLineSegPt = GetClosestPtOnLineSegment(samplePt, lineSegPt1, lineSegPt2);

        //Calculate the projection direction to the sample point
        Vector3 displacementDir = samplePt - closestLineSegPt;

        float sampleLineSegDistSqrd = displacementDir.sqrMagnitude;

        return sampleLineSegDistSqrd <= capsuleRadius * capsuleRadius;
    }

    public static Vector3 GetClosestPtOnLineSegment(
        Vector3 samplePt,
        Vector3 lineSegPt1,
        Vector3 lineSegPt2
        )
    {
        Vector3 lineSegPtDiff = lineSegPt2 - lineSegPt1;

        //This formula will give the projected percent along the line segment.
        //If the number is between 0 and 1 the point is on the line segment, otherwise it will be
        //a point collinear to the line segment.  Because of this we need to clamp the value bettween 0
        //and 1
        float sampleProjectedT = Vector3.Dot(lineSegPtDiff, samplePt - lineSegPt1) / lineSegPtDiff.sqrMagnitude;
        sampleProjectedT = Mathf.Clamp01(sampleProjectedT);

        //Calculate the closest pt on the line segment to our sample point.  This is based on the 
        //projected percent we calculated above.
        Vector3 closestLineSegPt = lineSegPtDiff * sampleProjectedT + lineSegPt1;

        return closestLineSegPt;
    }

    //This formula was taken from here: http://en.wikipedia.org/wiki/Spherical_cap
    public static float CalcSphereCapVolume(float sphereRadius, float capHeight)
    {
        return (Mathf.PI * capHeight * capHeight / 3) * (3 * sphereRadius - capHeight);
    }

    public static float CalcSphereVolume(float sphereRadius)
    {
        return (Mathf.PI * 4.0f / 3.0f) * sphereRadius * sphereRadius * sphereRadius;
    }

    public static float CalcCylinderVolume(float radius, float height)
    {
        return Mathf.PI * radius * radius * height;
    }

    public static float CalcCapsuleBaseVolume(float radius, float capsuleHeight, float baseHeight)
    {
        float volume = 0;
        float cylinderHeight = capsuleHeight - radius * 2;

        //are we over capsule Height?
        if(baseHeight >= capsuleHeight)
        {
            volume = CalcSphereVolume(radius);
            volume += CalcCylinderVolume(radius, cylinderHeight);
        }
        // are we over Cylinder
        else if(baseHeight >= capsuleHeight - radius)
        {
            volume = CalcSphereVolume(radius);
            volume -= CalcSphereCapVolume(radius, capsuleHeight - baseHeight);
            volume += CalcCylinderVolume(radius, cylinderHeight);
        }
        // are we over bottomCap
        else if(baseHeight > radius)
        {
            volume = CalcSphereVolume(radius) * 0.5f;
            volume += CalcCylinderVolume(radius, baseHeight - radius);
        }
        //are we on bottomCap
        else if(baseHeight > 0)
        {
            volume = CalcSphereCapVolume(radius, baseHeight);
        }    

        return volume;
    }

    public static Vector2 Mult(this Vector2 v1, Vector2 v2)
    {
        return new Vector2(v1.x * v2.x, v1.y * v2.y);
    }

    #endregion

    public static Vector3 GetPointOnBezierCurve(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        float u = 1f - t;
        float t2 = t * t;
        float u2 = u * u;
        float u3 = u2 * u;
        float t3 = t2 * t;

        Vector3 result =
            (u3) * p0 +
            (3f * u2 * t) * p1 +
            (3f * u * t2) * p2 +
            (t3) * p3;

        return result;
    }

    public static float GetPointOnBezierCurve(float p0, float p1, float p2, float p3, float t)
    {
        float u = 1f - t;
        float t2 = t * t;
        float u2 = u * u;
        float u3 = u2 * u;
        float t3 = t2 * t;

        float result =
            (u3) * p0 +
            (3f * u2 * t) * p1 +
            (3f * u * t2) * p2 +
            (t3) * p3;

        return result;
    }



    // returns a normalized value based on n within -r -> r 
    public static float NormalizeRange(float n, int r)
    {
        r = Mathf.Max(1, r);
       return (n + r) / (r * 2);
    }

    public static float InverseNormalizeRange(float n, ushort r)
    {
        return 1 - NormalizeRange(n, r);
    }
}
