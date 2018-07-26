using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalleryLoader : MonoBehaviour
{
    [SerializeField]
    List<ModelData> modelData;

    [SerializeField]
    List<Transform> modelPositions;

    [SerializeField]
    GameObject tagHandlerPrefab;

    [SerializeField]
    GameObject toolBoxPrefab;

    TagHandler openedTagHandler;

    public static bool lockView;

    private void Awake()
    {
        GameObject toolBox = Instantiate(toolBoxPrefab);

        for (int i = 0; i < modelData.Count && i < modelPositions.Count; i++)
        {
            modelData[i].CreateGameObject(tagHandlerPrefab, toolBox.GetComponent<ToolBox>(), modelPositions[i], OnTagHandlerViewAnnotations);
        }
    }

    void OnTagHandlerViewAnnotations(TagHandler tagHandler,bool expanding)
    {
        if (lockView == true)
            return;

        if(expanding && tagHandler != openedTagHandler)
        {
            if (openedTagHandler != null)
                openedTagHandler.MinimizeAnnotationView(() => {  tagHandler.MaximizeAnnotationView(); openedTagHandler = tagHandler; });
            else
            {
                tagHandler.MaximizeAnnotationView();
                openedTagHandler = tagHandler;
            }

        }
        else if(expanding == false)
        {
            tagHandler.MinimizeAnnotationView(null);
            openedTagHandler = null;
        }
    }
}
