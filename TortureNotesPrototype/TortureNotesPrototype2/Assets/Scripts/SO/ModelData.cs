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

    public TagHandler CreateGameObject(GameObject tagHandlerPrefab, ToolBox toolBox, Transform spawnPlace, System.Action<TagHandler, bool> aCallBackOnToggleAnnotationView)
    {
        GameObject newGameObject = new GameObject();
        newGameObject.transform.position = spawnPlace.position;
        newGameObject.transform.rotation = spawnPlace.rotation;

        MeshRenderer meshRenderer = newGameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = material;

        MeshFilter meshFilter = newGameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        MeshCollider meshCollider = newGameObject.AddComponent<MeshCollider>();
        meshCollider.inflateMesh = true;
        meshCollider.skinWidth = 0.1f;
        meshCollider.convex = true;
   
        meshCollider.isTrigger = true;
        VRSelectableObject vrSelectable = newGameObject.AddComponent<VRSelectableObject>();
        vrSelectable.SetSpeeds(100, 0.1f, 0.1f);
        vrSelectable.SetToolBox(toolBox);


        GameObject newTagHandler = GameObject.Instantiate(tagHandlerPrefab);
        TagHandler tagHandler = newTagHandler.GetComponent<TagHandler>();

        vrSelectable.SetTagHandler(tagHandler);

        tagHandler.PlaceTag(newGameObject.transform, tag, aCallBackOnToggleAnnotationView);

        return tagHandler;
    }
}
