using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NS_ObjectPooling;



namespace NS_Sound
{
    [System.Serializable]
    public class Clip
    {
        public AudioClip m_audio;
        public string m_name;
        [Range(0, 1)]
        public float m_startVolume = 1f;
    }

    [System.Serializable]
    public class SoundPool
    {
        [SerializeField]
        Clip[] m_clips;
        [SerializeField]
        ObjectPool m_audioPool;

        SoundType m_soundType;

        public void Start(SoundType soundType)
        {
            m_soundType = soundType;
            m_audioPool.Start();
        }

        //Function Definition: Attaches SoundObject to aObj and plays a Random clip.
        public SoundObject AttachRandSound(Transform aObj, float fadingSpeed)
        {
            GameObject aGo = m_audioPool.GetObjectFromPool();
            if (aGo != null)
            {
                aGo.transform.parent = aObj;
                aGo.transform.localPosition = Vector3.zero;

                SoundObject sndObj = aGo.GetComponent<SoundObject>();

                PlayRandomClipOnSoundObject(sndObj, fadingSpeed);
                return sndObj;
            }
            return null;
        }

        //Function Definition: Attaches SoundObject to aObj and plays a Requested clip
        public SoundObject AttachSound(Transform aObj, int id, float fadingSpeed)
        {
            GameObject aGo = m_audioPool.GetObjectFromPool();
            if (aGo != null)
            {
                aGo.transform.parent = aObj;
                aGo.transform.localPosition = Vector3.zero;

                SoundObject sndObj = aGo.GetComponent<SoundObject>();

                PlayClipOnSoundObject(sndObj, id, fadingSpeed);
                return sndObj;
            }
            return null;
        }

        //Function Definition: Attaches SoundObject to aObj and plays a Requested clip
        public SoundObject AttachSound(Transform aObj, string name, float fadingSpeed)
        {
            GameObject aGo = m_audioPool.GetObjectFromPool();
            if (aGo != null)
            {
                aGo.transform.parent = aObj;
                aGo.transform.localPosition = Vector3.zero;

                SoundObject sndObj = aGo.GetComponent<SoundObject>();

                PlayClipOnSoundObject(sndObj, name, fadingSpeed);
                return sndObj;
            }

            return null;
        }

        //	Function Definition: Plays AudioClip at position.
        public SoundObject PlayRandSoundAtPos(Vector3 aPos, float fadingSpeed)
        {
            GameObject aGo = m_audioPool.GetObjectFromPool();
            if (aGo != null)
            {
                aGo.transform.position = aPos;

                SoundObject sndObj = aGo.GetComponent<SoundObject>();

                PlayRandomClipOnSoundObject(sndObj, fadingSpeed);
                return sndObj;
            }
            return null;
        }


        public SoundObject PlaySoundAtPos(Vector3 aPos, int id, float fadingSpeed)
        {
            GameObject aGo = m_audioPool.GetObjectFromPool();
            if (aGo != null)
            {
                aGo.transform.position = aPos;

                SoundObject sndObj = aGo.GetComponent<SoundObject>();

                PlayClipOnSoundObject(sndObj, id, fadingSpeed);
                return sndObj;
            }
            return null;
        }
        public SoundObject PlaySoundAtPos(Vector3 aPos, string name, float fadingSpeed)
        {
            GameObject aGo = m_audioPool.GetObjectFromPool();
            if (aGo != null)
            {
                aGo.transform.position = aPos;

                SoundObject sndObj = aGo.GetComponent<SoundObject>();

                PlayClipOnSoundObject(sndObj, name, fadingSpeed);

                return sndObj;
            }

            return null;
        }
        //	Function Definition: Plays Rand AudioClip on aSndObj.
        void PlayRandomClipOnSoundObject(SoundObject aSndObj, float fadingSpeed)
        {
            if (m_clips.Length == 0)
                return;
            int randIndex = Random.Range(0, m_clips.Length);
            aSndObj.PlayAudio(m_clips[randIndex], m_soundType, randIndex);
        }

        void PlayClipOnSoundObject(SoundObject aSndObj, int index, float fadingSpeed)
        {
            if (index < m_clips.Length)
            {
                if (fadingSpeed < 0)
                    aSndObj.PlayAudio(m_clips[index], m_soundType, index);
                else
                    aSndObj.FadeInPlay(m_clips[index], fadingSpeed, m_soundType, index);
            }
            else
            {
                Debug.LogWarning("SoundPool.PlayClipOnSoundObject could not find clip with index " + index);
            }
        }

        void PlayClipOnSoundObject(SoundObject aSndObj, string name, float fadingSpeed)
        {
            for (int i = 0; i < m_clips.Length; i++)
            {
                if (m_clips[i].m_name == name)
                {
                    if (fadingSpeed < 0)
                        aSndObj.PlayAudio(m_clips[i], m_soundType, i);
                    else
                        aSndObj.FadeInPlay(m_clips[i], fadingSpeed, m_soundType, i);
                    return;
                }
            }

            Debug.LogWarning("SoundPool.PlayClipOnSoundObject could not find clip with name " + name);
        }

        //	Function Definition: getter for m_soundType
        public SoundType GetSoundType()
        {
            return m_soundType;
        }
    }
}
