using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NS_Annotation.NS_Data
{
    public enum Priority
    {
        Low = 1,
        Medium = 2,
        High = 3
    }

    [System.Serializable]
    public class Tag
    {
        public int objectID;
        public string title;
        public string description;
        public Vector3 localPos;
        public List<AnnotationNode> annotationNodes = new List<AnnotationNode>();
        public int AnnotationCount { get { return annotationNodes.Count; } }
    }
    [System.Serializable]
    public struct Comment
    {
        public string author;
        public string date;
        public string content;
        public Priority priority;

        public Comment(string a_author, string a_date, string a_content, Priority a_priority = Priority.Low)
        {
            author = a_author;
            date = a_date;
            content = a_content;
            priority = a_priority;
        }

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

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
    }
    [System.Serializable]
    public class AnnotationNode
    {
        [SerializeField]
        private Comment m_MainComment;
        [SerializeField]
        private List<Comment> m_Replies = new List<Comment>();

        public Vector3 AnnotationEndPos;
        public Vector3 AnnotationStartPos;
        public float AnnotationScale;

        public AnnotationNode(string a_author, string a_date, string a_content, Priority a_priority = Priority.Low)
        {
            m_MainComment = new Comment(a_author, a_date, a_content, a_priority);
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
        #endregion
    }
}