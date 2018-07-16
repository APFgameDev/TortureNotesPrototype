using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEnableSpawnInRelativePositionToPlayerWithOffset : MonoBehaviour
{
    [SerializeField]
    Vector3 offset;

    private void OnEnable()
    {
        transform.position = Camera.main.transform.position + Camera.main.transform.rotation * offset;
    }
}
