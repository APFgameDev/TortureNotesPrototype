using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalleryLoader : MonoBehaviour
{
    [SerializeField]
    ModelData[] modelData;

    [SerializeField]
    Transform[] modelPositions;

    [SerializeField]
    GameObject tagHandlerPrefab;

    private void Awake()
    {
        for(int i = 0; i < modelData.Length && i < modelPositions.Length; i++)
        {
            GameObject gameObject = modelData[i].CreateGameObject(tagHandlerPrefab, modelPositions[i]);
        }
    }
}
