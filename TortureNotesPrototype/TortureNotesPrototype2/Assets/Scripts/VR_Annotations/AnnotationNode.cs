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

        public static bool operator ==(Comment a, Comment b)
        {
            return  (a.author.ToLower()     == b.author.ToLower())  &&
                    (a.date.ToLower()       == b.date.ToLower())    &&
                    (a.content.ToLower()    == b.content.ToLower())     ? true : false;
        }

        public static bool operator !=(Comment a, Comment b)
        {
            return  (a.author.ToLower()     != b.author.ToLower())  &&
                    (a.date.ToLower()       != b.date.ToLower())    &&
                    (a.content.ToLower()    != b.content.ToLower())     ? true : false;
        }
    }

    public class AnnotationNode
    {
        private Comment m_MainComment = new Comment();
        private List<Comment> m_Replies = new List<Comment>();

        public AnnotationNode(string author, string content)
        {
            m_MainComment.author = author;
            m_MainComment.content = content;
        }

        //Adds a comment to this annotation thread
        public void AddComment(Comment comment)
        {
            if (m_Replies.Contains(comment))
            {
                Debug.LogWarning("The main comment thread already contains this reply by author " + comment.author);
            }

            m_Replies.Add(comment);
        }

        //Removes a comment from this annotation thread
        public void RemoveComment(Comment comment)
        {
            if (!m_Replies.Contains(comment))
            {
                Debug.LogWarning("The main comment thread doesn't contain this reply by author " + comment.author);
            }

            m_Replies.Remove(comment);
        }

        //Clears the list of annotations that are held within this annotation node        
        public void DeleteAnnotation()
        {
            m_Replies.Clear();
        }

        #region Properties

        public Comment MainThread { get { return m_MainComment; } }
        public List<Comment> Replies { get { return m_Replies; } }
        public int ThreadCount { get { return m_Replies.Count + 1; } }
        public Vector3 AnnotationPos { get; set; }

        #endregion
    }
}