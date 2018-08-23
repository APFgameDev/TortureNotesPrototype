using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundLib : MonoBehaviour
{
    public SoundGroup[] SoundGroups;

    private Dictionary<string, AudioClip[]> groupDictionary = new Dictionary<string, AudioClip[]>();

    private void Awake()
    {
        foreach (SoundGroup soundGroup in SoundGroups)
        {
            groupDictionary.Add(soundGroup.GroupID, soundGroup.Group);
        }
    }

    public AudioClip GetClipFromName(string name)
    {
        if(groupDictionary.ContainsKey(name) == true)
        {
            AudioClip[] sounds = groupDictionary[name];
            return sounds[Random.Range(0, sounds.Length)];
        }
        return null;
    }

    [System.Serializable]
    public class SoundGroup
    {
        public string GroupID;
        public AudioClip[] Group;

    }

}
