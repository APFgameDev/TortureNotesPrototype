using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct Comment
{
    public string author;
    public string date;
    public string content;
    public List<Comment> m_replies;
}

public class AnnotationNode
{
    Comment mainComment;
    int numReplies = 0;
    Vector3 localPos;

    void IncrementReplies()
    {
        numReplies++;
    }

    void DecrementReplies()
    {
        numReplies--;
    }

    void SetLocalPos(Vector3 aLocalPos)
    {
        localPos = aLocalPos;
    }
}
