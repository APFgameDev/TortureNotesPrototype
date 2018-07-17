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
        if(needsToSetPos)
        {
            needsToSetPos = false;

            Normal.UI.MoveAndScale moveAndScale = GetComponent<Normal.UI.MoveAndScale>();


            if (moveAndScale != null)
            {
                moveAndScale._targetPosition = Camera.main.transform.position + Camera.main.transform.rotation * offset;
                moveAndScale._targetRotation = Camera.main.transform.rotation * Quaternion.Euler(relativeRot);
            }
            else
            {
                transform.position = Camera.main.transform.position + Camera.main.transform.rotation * offset;
                transform.rotation = Camera.main.transform.rotation * Quaternion.Euler(relativeRot);
            }
        }
    }

}
