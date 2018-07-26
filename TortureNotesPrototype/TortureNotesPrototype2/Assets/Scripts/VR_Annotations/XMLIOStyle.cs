using System.IO;
using System.Xml;
using NS_Annotation.NS_Data;
using UnityEngine;
using UnityEngine.SceneManagement;

public class XMLIOStyle : IOStyle
{
    public override void LoadData(string a_file)
    {
        FileStream file = null;

        if (File.Exists(a_file))
        {
            File.Create(a_file);
        }

        file = new FileStream(a_file, FileMode.Open);

        TagHandler[] objectsInScene = Resources.FindObjectsOfTypeAll<TagHandler>();

        using (XmlReader reader = XmlReader.Create(file))
        {
            //Read the basic tag data and save it into the tag            
            reader.ReadToFollowing("Tags");

            foreach (TagHandler tagHandler in objectsInScene)
            {            
                reader.ReadToFollowing("uniqueID");
                int uniqueID = reader.ReadContentAsInt();

                reader.ReadToFollowing("title");
                string title = reader.ReadContentAsString();

                reader.ReadToFollowing("deescription");
                string description = reader.ReadContentAsString();

                reader.ReadToFollowing("posX");
                float x = reader.ReadContentAsFloat();

                reader.ReadToFollowing("posY");
                float y = reader.ReadContentAsFloat();

                reader.ReadToFollowing("posZ");
                float z = reader.ReadContentAsFloat();

                tagHandler.TagData.title = title;
                tagHandler.TagData.description = description;
                tagHandler.TagData.localPos = new Vector3(x, y, z);

                //next we do the annotations

                reader.ReadToFollowing("count");
                int numAnnotations = reader.ReadContentAsInt();

                for (int i = 0; i < numAnnotations; i++)
                {
                    //Create the first thread and save the content in the main thread node
                    reader.ReadToFollowing("mainThread");

                    reader.ReadToFollowing("mainThreadCreator");
                    string mainThreadCreator = reader.ReadContentAsString();

                    reader.ReadToFollowing("mainThreadCreationDate");
                    string mainThreadCreationDate = reader.ReadContentAsString();

                    reader.ReadToFollowing("mainThreadContent");
                    string mainthreadContent = reader.ReadContentAsString();

                    reader.ReadToFollowing("mainThreadPriority");
                    Priority mainThreadPriority = (Priority)reader.ReadContentAsInt();

                    AnnotationNode node = new AnnotationNode(mainThreadCreator, mainThreadCreationDate, mainthreadContent, mainThreadPriority);

                    reader.ReadToFollowing("count");
                    int numReplies = reader.ReadContentAsInt();

                    for (int j = 0; j < numReplies; j++)
                    {
                        reader.ReadToFollowing("commentOwner");
                        string commentOwner = reader.ReadContentAsString();

                        reader.ReadToFollowing("commentCreationDate");
                        string commentCreationDate = reader.ReadContentAsString();

                        reader.ReadToFollowing("commentContent");
                        string commentContent = reader.ReadContentAsString();

                        reader.ReadToFollowing("commentPriority");
                        Priority commentPriority = (Priority)reader.ReadContentAsInt();

                        Comment reply = new Comment(commentOwner, commentCreationDate, commentContent, commentPriority);

                        node.AddComment(reply);
                    }

                    tagHandler.TagData.annotationNodes.Add(node);
                }
            }           
        }
    }

    public override void SaveData(string a_file)
    {
        FileStream file = null;

        if (File.Exists(a_file))
        {
            File.Create(a_file);
        }

        file = new FileStream(a_file, FileMode.Open);

        TagHandler[] sceneTags = Resources.FindObjectsOfTypeAll<TagHandler>();

        XmlWriter writer = XmlWriter.Create(a_file);

        writer.WriteStartDocument();

        writer.WriteStartElement("Scene");
        writer.WriteAttributeString("id", UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);

        writer.WriteStartElement("Tags");
        writer.WriteAttributeString("count", sceneTags.Length.ToString());

        foreach (TagHandler tag in sceneTags)
        {
            writer.WriteStartElement("uniqueID");
            writer.WriteString(tag.TagData.objectID.ToString());
            writer.WriteEndElement();

            writer.WriteStartElement("title");
            writer.WriteString(tag.TagData.title);
            writer.WriteEndElement();

            writer.WriteStartElement("description");
            writer.WriteString(tag.TagData.description);
            writer.WriteEndElement();

            writer.WriteStartElement("posX");
            writer.WriteString(tag.TagData.localPos.x.ToString());
            writer.WriteEndElement();

            writer.WriteStartElement("posY");
            writer.WriteString(tag.TagData.localPos.y.ToString());
            writer.WriteEndElement();

            writer.WriteStartElement("posZ");
            writer.WriteString(tag.TagData.localPos.z.ToString());
            writer.WriteEndElement();

            writer.WriteStartElement("Annotations");

            foreach (AnnotationNode node in tag.TagData.annotationNodes)
            {
                writer.WriteStartElement("count");
                writer.WriteString(node.ThreadCount.ToString());
                writer.WriteEndElement();

                writer.WriteStartElement("mainThread");

                writer.WriteStartElement("mainThreadCreator");
                writer.WriteString(node.MainThread.author);
                writer.WriteEndElement();

                writer.WriteStartElement("mainThreadCreationDate");
                writer.WriteString(node.MainThread.date);
                writer.WriteEndElement();

                writer.WriteStartElement("mainThreadContent");
                writer.WriteString(node.MainThread.content);
                writer.WriteEndElement();

                writer.WriteStartElement("mainThreadPriority");
                writer.WriteString(((int)node.MainThread.priority).ToString());
                writer.WriteEndElement();

                writer.WriteEndElement();

                writer.WriteStartElement("replies");
                writer.WriteStartElement("count");
                writer.WriteString(node.ThreadCount.ToString());
                writer.WriteEndElement();

                foreach (Comment comment in node.Replies)
                {
                    writer.WriteStartElement("commentOwner");
                    writer.WriteString(comment.author);
                    writer.WriteEndElement();

                    writer.WriteStartElement("commentCreationDate");
                    writer.WriteString(comment.date);
                    writer.WriteEndElement();

                    writer.WriteStartElement("commentContent");
                    writer.WriteString(comment.content);
                    writer.WriteEndElement();

                    writer.WriteStartElement("commentPriority");
                    writer.WriteString(((int)comment.priority).ToString());
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();           //End replies
            }

            writer.WriteEndElement();               //End Annotations
        }
        writer.WriteEndElement();                   //End Tags
        writer.WriteEndElement();                   //End Scene

        writer.WriteEndDocument();
        writer.Close();
    }
}
