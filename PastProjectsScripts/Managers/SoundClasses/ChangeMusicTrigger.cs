using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NS_Sound;


namespace NS_Sound
{
    public class ChangeMusicTrigger : MonoBehaviour
    {
        [SerializeField]
        string m_tagForTrigger = string.Empty;
        [SerializeField]
        string m_musicName = string.Empty;
        [SerializeField]
        int m_musicID = -1;
        [SerializeField]
        float m_fadingForStopMusic = 0.2f;
        [SerializeField]
        float m_fadingForStartMusic = 0.2f;


        void OnTriggerEnter(Collider col)
        {
            if (col.tag == m_tagForTrigger)
            {
                SoundEventDef sndEDef = new SoundEventDef();
                sndEDef.m_soundType = SoundType.Music;

                SoundController.AddSoundEvent(new SoundEventStopSound(sndEDef, m_fadingForStopMusic));

                if (m_musicName != string.Empty)
                {
                    SoundController.PlaySoundAtLocation(SoundType.Music, Vector3.zero, m_musicName, m_fadingForStartMusic);
                }
                else if (m_musicID >= 0)
                {
                    SoundController.PlaySoundAtLocation(SoundType.Music, Vector3.zero, m_musicID, m_fadingForStartMusic);

                }
                else
                {
                    Debug.LogError("ChangeMusicTrigger Could Not Find Requested music to play");
                    return;
                }
                gameObject.SetActive(false);

            }
        }
    }
}