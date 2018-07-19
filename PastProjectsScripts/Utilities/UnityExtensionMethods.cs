using UnityEngine;
using System.Collections.Generic;
using System;
public static class UnityExtensionMethods
{
    //https://answers.unity.com/questions/458207/copy-a-component-at-runtime.html
    public static T CopyComponent<T>(this GameObject destination, T original) where T : Component
    {
        System.Type type = original.GetType();
        var dst = destination.GetComponent(type) as T;
        if (!dst) dst = destination.AddComponent(type) as T;
        var fields = type.GetFields();
        foreach (var field in fields)
        {
            if (field.IsStatic) continue;
            field.SetValue(dst, field.GetValue(original));
        }
        var props = type.GetProperties();
        foreach (var prop in props)
        {
            if (!prop.CanWrite || !prop.CanWrite || prop.Name == "name") continue;
            prop.SetValue(dst, prop.GetValue(original, null), null);
        }
        return dst as T;
    }

    public static Vector3 Multiply(this Vector3 vectorA, Vector3 vectorB)
    {
        Vector3 multipliedVector = Vector3.zero;
        multipliedVector.x = vectorA.x * vectorB.x;
        multipliedVector.y = vectorA.y * vectorB.y;
        multipliedVector.z = vectorA.z * vectorB.z;

        return multipliedVector;
    }

    public static Vector3 Multiply(this Vector3 vectorA,float x, float y, float z)
    {
        Vector3 multipliedVector = Vector3.zero;
        multipliedVector.x = vectorA.x * x;
        multipliedVector.y = vectorA.y * y;
        multipliedVector.z = vectorA.z * z;

        return multipliedVector;
    }

    public static void AddArrayToDictionary<I,T>(ref Dictionary<I,T> dictionary, T[] array, Func<T, I> aDelegate)
    {
        for (int i = 0; i < array.Length; i++)
            dictionary.Add(aDelegate(array[i]), array[i]);
    }


    public static void ResetTransformation(this Transform trans)
    {
        trans.position = Vector3.zero;
        trans.localRotation = Quaternion.identity;
        trans.localScale = new Vector3(1, 1, 1);
    }

    /// <summary>
    /// transform Local to World rot
    /// </summary>
    /// <param name="trans"></param>
    /// <param name="rot"></param>
    /// <returns></returns>
    public static Quaternion TransformRot(this Transform trans, Quaternion rot)
    {
        return Quaternion.Inverse(trans.rotation) * rot;
    }

    /// <summary>
    /// transform World to Local rot
    /// </summary>
    /// <param name="trans"></param>
    /// <param name="rot"></param>
    /// <returns></returns>
    public static Quaternion InverseTransformRot(this Transform trans, Quaternion rot)
    {
        return trans.rotation * rot;
    }

    public static T[] GetComponentsWithTag<T>(string aTag) where T : Component
    {
        GameObject[] m_gos = GameObject.FindGameObjectsWithTag(aTag);
        List<T> m_comps = new List<T>();

        for (int i = 0; i < m_gos.Length; i++)
        {
            T comp = m_gos[i].GetComponent<T>();
            if (comp != null)
                m_comps.Add(comp);
        }

        return m_comps.ToArray();
    }


    public static void ResetSetParent(this Transform trans,Transform other)
    {
        trans.position = other.position;
        trans.parent = other;
        trans.localRotation = Quaternion.identity;
        trans.localScale = new Vector3(1, 1, 1);
    }

    //https://answers.unity.com/questions/893966/how-to-find-child-with-tag.html
    public static T[] FindComponentsInChildrenWithTag<T>(this GameObject parent, string tag, bool forceActive = false) where T : Component
    {
        if (parent == null) { throw new System.ArgumentNullException(); }
        if (string.IsNullOrEmpty(tag) == true) { throw new System.ArgumentNullException(); }
        List<T> list = new List<T>(parent.GetComponentsInChildren<T>(forceActive));
        if (list.Count == 0) { return null; }

        for (int i = list.Count - 1; i >= 0; i--)
        {
            if (list[i].CompareTag(tag) == false)
            {
                list.RemoveAt(i);
            }
        }
        return list.ToArray();
    }

    public static T FindComponentInChildWithTag<T>(this GameObject parent, string tag, bool forceActive = false) where T : Component
    {
        if (parent == null) { throw new System.ArgumentNullException(); }
        if (string.IsNullOrEmpty(tag) == true) { throw new System.ArgumentNullException(); }

        T[] list = parent.GetComponentsInChildren<T>(forceActive);
        foreach (T t in list)
        {
            if (t.CompareTag(tag) == true)
            {
                return t;
            }
        }
        return null;
    }

    public static T[] FindComponentsWithTag<T>(string tag) where T : Component
    {
        if (string.IsNullOrEmpty(tag) == true) { throw new System.ArgumentNullException(); }

        List<GameObject> gOList = new List<GameObject>(GameObject.FindGameObjectsWithTag(tag));
        List<T> CompList = new List<T>(gOList.Count);

        for (int i = 0; i < gOList.Count; i++)
        {
            T comp = gOList[i].GetComponent<T>();
            if (comp != null)
                CompList.Add(comp);
        }

        if (CompList.Count == 0) { return null; }

        return CompList.ToArray();
    }



    public static T[] FindComponentsWithTags<T>(string[] tags) where T : Component
    {
        if (tags == null) { throw new System.ArgumentNullException(); }

        List<GameObject> gOList = new List<GameObject>();

        for (int i = 0; i < tags.Length; i++)
        {
            if (string.IsNullOrEmpty(tags[i]) == true) { throw new System.ArgumentNullException(); }

            gOList.AddRange(GameObject.FindGameObjectsWithTag(tags[i]));
        }

   
        List<T> CompList = new List<T>(gOList.Count);

        for (int i = 0; i < gOList.Count; i++)
        {
            T comp = gOList[i].GetComponent<T>();
            if (comp != null)
                CompList.Add(comp);
        }

        if (CompList.Count == 0) { return null; }

        return CompList.ToArray();
    }
}
