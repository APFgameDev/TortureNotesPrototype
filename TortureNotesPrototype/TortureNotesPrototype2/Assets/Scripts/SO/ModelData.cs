using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Model Data", menuName = "Scriptable Objects/Model Data")]
public class ModelData : ScriptableObject
{
    [SerializeField]
    Mesh mesh;

    [SerializeField]
    Material material;

    public NS_Annotation.NS_Data.Tag tag;

    public GameObject CreateGameObject(GameObject tagHandlerPrefab, Transform spawnPlace)
    {
        GameObject newGameObject = new GameObject();
        newGameObject.transform.position = spawnPlace.position;
        newGameObject.transform.rotation = spawnPlace.rotation;

        MeshRenderer meshRenderer = newGameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = material;

        MeshFilter meshFilter = newGameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        MeshCollider meshCollider = newGameObject.AddComponent<MeshCollider>();
        meshCollider.convex = true;
        meshCollider.isTrigger = true;
        VRGrabbable vrGrabbable = newGameObject.AddComponent<VRGrabbable>();
        vrGrabbable.SetSpeeds(100, 0.1f, 0.1f);

        GameObject newTagHandler = GameObject.Instantiate(tagHandlerPrefab);

        newTagHandler.GetComponent<TagHandler>().PlaceTag(newGameObject.transform, tag);

        return newGameObject;
    }
}
