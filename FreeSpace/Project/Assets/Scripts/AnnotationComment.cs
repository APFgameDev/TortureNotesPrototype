using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnnotationComment : MonoBehaviour
{
    public System.Action<GameObject> DeleteCommentCallback;

    public void OnDeleteComment()
    {
        DeleteCommentCallback(gameObject);
    }
}
