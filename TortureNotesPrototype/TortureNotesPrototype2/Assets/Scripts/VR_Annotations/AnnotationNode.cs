using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NS_Annotation.NS_Data
{
    public class Tag
    {
        string title;
        string description;
        Vector3 localPos;
        Vector3 localOrigin;

        List<AnnotationNode> annotationNodes = new List<AnnotationNode>();

        public int AnnotationCount { get { return annotationNodes.Count; } }
    }

    public struct Comment
    {
        public string author;
        public string date;
        public string content;
    }

    public class AnnotationNode
    {
        Comment mainComment;
        List<Comment> comments = new List<Comment>();
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
}