using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToPoolOnDisabled : MonoBehaviour
{
    System.Action<ReturnToPoolOnDisabled> returnToPoolFunc;

    public void SetReturnFunction(System.Action<ReturnToPoolOnDisabled> aReturnToPoolFunc)
    {
        returnToPoolFunc = aReturnToPoolFunc;
    }

    private void OnDisable()
    {
        returnToPoolFunc(this);  
    }
}
