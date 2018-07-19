using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NS_Sound;

namespace NS_Sound
{
    public class PlayMusicAtLoad : MonoBehaviour
    {

        [SerializeField]
        int m_musicID = -1;
        [SerializeField]
        float m_fadingForStopMusic = 0.2f;
        [SerializeField]
        float m_fadingForStartMusic = 0.2f;
        // Use this for initialization
        void Start()
        {
            SoundController.StopSound(SoundType.Music, m_fadingForStopMusic);

            if (m_musicID == -1)
            {
                Debug.Log("PlayMusicAtLoad playing nothing. Stopped music");
            }
            else if (m_musicID >= 0)
            {
                SoundController.PlaySoundAtLocation(SoundType.Music, Vector3.zero, m_musicID, m_fadingForStartMusic);
            }
            else
            {
                Debug.LogError("PlayMusicAtLoad Could Not Find Requested music to play");
                return;
            }
        }

    }
}