using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEnableSpawnRelative2D : MonoBehaviour
{
    [SerializeField]
    Vector3 posOffset;

    [SerializeField]
    Vector3 rotOffset;

    private void OnEnable()
    {
        Transform camTransform = Camera.main.transform;
        Vector3 Fwd2D = camTransform.forward;
        Fwd2D.y = 0;
        Quaternion Fwd2DRot = Quaternion.LookRotation(Fwd2D, Vector3Int.up);

        transform.position = camTransform.position + Fwd2DRot * posOffset;
        transform.rotation = Fwd2DRot * Quaternion.Euler(rotOffset);
    }
}
