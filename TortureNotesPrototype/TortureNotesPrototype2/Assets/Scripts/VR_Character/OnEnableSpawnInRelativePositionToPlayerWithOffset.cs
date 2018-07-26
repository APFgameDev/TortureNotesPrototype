using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEnableSpawnInRelativePositionToPlayerWithOffset : MonoBehaviour
{
    [SerializeField]
    Vector3 offset;
    [SerializeField]
    Vector3 relativeRot;
    bool needsToSetPos = false;

    private void OnEnable()
    {
        needsToSetPos = true;
    }

    private void Update()
    {
        if (needsToSetPos)
        {
            needsToSetPos = false;

            transform.position = Camera.main.transform.position + Camera.main.transform.rotation * offset;
            transform.rotation = Camera.main.transform.rotation * Quaternion.Euler(relativeRot);
        }
    }

    public void SetToReposition()
    {
        needsToSetPos = true;
    }

}
