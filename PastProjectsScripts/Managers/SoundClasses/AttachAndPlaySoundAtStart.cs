using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NS_Sound;

public class AttachAndPlaySoundAtStart : MonoBehaviour
{
    [SerializeField]
    SoundType m_soundType;

    [SerializeField]
    string m_clipName;

    [SerializeField]
    float m_fadingSpeed = -1;

    [SerializeField]
    float m_startVolume = 1;

    [SerializeField]
    bool m_isLooping = false;

    int m_uniqueId;

	// Use this for initialization
	void Start () {
        m_uniqueId = SoundController.AttachAndPlaySound(m_soundType, transform, m_clipName, m_fadingSpeed);

        SoundObject temp = SoundController.LocateLiveSoundObjectWithUniqueID(m_soundType, m_uniqueId);
        if (temp != null)
        {
            SoundController.ChangeSoundVolume(temp, m_startVolume);

            SoundController.ChangeSoundIsLooping(temp, m_isLooping);
        }
    }
}
