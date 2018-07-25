using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class IOStyle : MonoBehaviour
{
    [SerializeField]
    private string directory;

    public string Directory { get { return directory; } }

    public void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    public void OnApplicationQuit()
    {
        SaveData(Directory);
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        LoadData(Directory);
    }

    public void OnSceneUnloaded(Scene scene)
    {
        SaveData(Directory);
    }

    public abstract void SaveData(string a_directory);
    public abstract void LoadData(string a_directory);
}
