using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NS_Sound
{
    //	Class Definition: The SoundObject Class is a component that attaches to a GO with a Audio Source attached. 
    //SoundObject will be the controller handling of Audio Source.
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(NS_Timer.Timer))]
    public class SoundObject : MonoBehaviour
    {
        AudioSource m_audioSource;
        NS_Timer.Timer m_timer;
        string m_clipName;
        SoundType m_soundType;
        int m_id;

        [SerializeField]
        float m_volumeTarget = 1;
        float m_volumeLerpSpeed = 0.1f;
        bool m_stopping = false;

        // Use this for initialization
        void Start()
        {
            m_timer = GetComponent<NS_Timer.Timer>();
            m_timer.enabled = false;
            m_audioSource = GetComponent<AudioSource>();
        }
        public void PauseAudio()
        {
            m_audioSource.Pause();
        }
        
        public void ResumeAudio()
        {
            m_audioSource.UnPause();
        }

        void Update()
        {
            float volumeTarget = m_stopping ? 0 : m_volumeTarget;

            if (MathUtils.AlmostEquals(m_audioSource.volume, volumeTarget) == false)
            {
                if (volumeTarget > m_audioSource.volume)
                {
                    m_audioSource.volume += Time.deltaTime * m_volumeLerpSpeed;
                    if (volumeTarget < m_audioSource.volume)
                        m_audioSource.volume = volumeTarget;
                }
                else
                {
                    m_audioSource.volume -= Time.deltaTime * m_volumeLerpSpeed;
                    if (volumeTarget > m_audioSource.volume)
                        m_audioSource.volume = volumeTarget;
                }
            }
            if (m_stopping == true && m_audioSource.volume <= 0)
                Disable();
        }  

        //Function Definition: Plays AudioClip.
        public void PlayAudio(Clip clip, SoundType aSndType, int id)
        {
            if (m_audioSource == null)
                Start();


            m_audioSource.Stop();
            m_clipName = clip.m_name;
            m_soundType = aSndType;
            m_audioSource.clip = clip.m_audio;
            m_audioSource.time = 0;
            m_audioSource.volume = clip.m_startVolume;
            m_audioSource.Play();
            m_id = id;
            m_stopping = false;
            //if audioSourece does not loop we set up the timer to disable our GO 
            //and return it back to pool when clip ends
            if (m_audioSource.loop == false)
                m_timer.SetUpTimer(Disable, clip.m_audio.length);
        }

        public void FadeInPlay(Clip clip, float fadeSpeed, SoundType aSndType, int id)
        {
            PlayAudio(clip, aSndType, id);
            m_audioSource.volume = 0;
            m_volumeLerpSpeed = fadeSpeed;
        }

        public void FadeOutStop(float fadeSpeed)
        {
            m_volumeLerpSpeed = fadeSpeed;
            m_stopping = true;
        }

        public void ChangePlayBackSpeed(float playBackSpeed)
        {
            m_audioSource.pitch = playBackSpeed;
        }

        public void ChangeVolume(float volume, float easeSpeed)
        {
            if (easeSpeed > 0)
            {
                m_volumeLerpSpeed = easeSpeed;
                m_volumeTarget = volume;
            }
            else
            {
                m_volumeTarget = m_audioSource.volume = volume;
            }
        }

        public void ChangeIsLooping(bool isLooping)
        {
            m_audioSource.loop = isLooping;

            if (m_audioSource.loop == false)
                m_timer.SetUpTimer(Disable, m_audioSource.clip.length);
            else
                m_timer.Stop();
        }

        public void Disable()
        {
            m_timer.enabled = false;
            gameObject.SetActive(false);
            SoundController.RemoveSoundFromActiveSounds(m_soundType, this);
        }

        public bool GetIsPlaying()
        {
            return m_audioSource.isPlaying;
        }

        public string GetClipName()
        {
            return m_clipName;
        }

        public AudioClip GetClip()
        {
            return m_audioSource.clip;
        }

        public SoundType GetSoundType()
        {
            return m_soundType;
        }

        public int GetId()
        {
            return m_id;
        }

        public AudioSource GetAudioSource()
        {
            return m_audioSource;
        }

    }
}
