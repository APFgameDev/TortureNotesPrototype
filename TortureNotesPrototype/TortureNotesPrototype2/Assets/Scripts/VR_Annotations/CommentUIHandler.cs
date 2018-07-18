using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CommentUIHandler : SingletonBehaviour<CommentUIHandler> {

    ObjectPool<CommentPanel> objectPool = new ObjectPool<CommentPanel>(5,5);

    public void Open()
    {

    }
    public void Close()
    {

    }

    public static CommentPanel GetCommentPanelFromPool()
    {
        return Instance.objectPool.GetObjectFromPool();
    }
}
