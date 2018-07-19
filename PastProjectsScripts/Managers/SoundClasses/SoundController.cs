using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NS_Managers;
using NS_Managers.NS_GameManagement;
using UnityEngine.Audio;

namespace NS_Sound
{
    public enum SoundType
    {
        Music,
        PlayerLoops,
        PlayerSounds,
        SoundEffects,
        SoundEffectLoops,
        LoseSounds
    }

    public class MixerGroups
    {
        public const string MASTER = "Master";
        public const string MUSIC = "Music";
        public const string SOUNDEFFECT = "SoundEffect";
        public const string DIALOG = "Dialog";
    }
    public class MixerVars
    {
        public const string VOLUME = "Volume";
    }


    [System.Serializable]
    public class ManageSound : ManagedObject<SoundPool, SoundType>
    {
    }


    //	Class Definition: The Sound Controller class will store refs to all Sounds;
    //when a Sound needs to be played the Sound Controller.
    public class SoundController : Manager<SoundController, SoundPool, SoundType, ManageSound>
    {
        static SoundController s_instance = null;
        Dictionary<SoundType, List<SoundObject>> m_liveSoundObjects = new Dictionary<SoundType, List<SoundObject>>();

        List<SoundEvent> m_soundEvents = new List<SoundEvent>();

        [SerializeField]
        AudioMixer m_masterMixer;

        bool m_gameSoundsPaused = false;
        List<SoundObject> m_pausedSoundObjects;

        Dictionary<System.Type, List<SoundEvent>> m_soundEventPool = new Dictionary<System.Type, List<SoundEvent>>();
        void Awake()
        {
            // init Manager
            base.Initialize(this);

            //init dictionaries 
            foreach (KeyValuePair<SoundType, SoundPool> oP in m_objects)
            {
                oP.Value.Start(oP.Key);
                // create a new list in m_liveSoundObjects
                m_liveSoundObjects.Add(oP.Key, new List<SoundObject>());
            }
            // add existing SoundObjects
            SoundObject[] soundObjs = FindObjectsOfType<SoundObject>();
            foreach (SoundObject sndObj in soundObjs)
            {
                if (sndObj.isActiveAndEnabled)
                    m_liveSoundObjects[sndObj.GetSoundType()].Add(sndObj);
            }

            m_pausedSoundObjects = new List<SoundObject>();
        }

        void LateUpdate()
        {
            for (int i = 0; i < m_soundEvents.Count; i++)
            {
                m_soundEvents[i].ProcessEvent();
                if (m_soundEvents[i].GetIsSpawnedInPool())
                    AddSoundEventToPool(m_soundEvents[i]);
            }

            m_soundEvents.Clear();
        }

        static public float GetVolumeLevel(string mixerGroup)
        {
            float outVal;
            s_instance.m_masterMixer.GetFloat(mixerGroup + MixerVars.VOLUME, out outVal);
            return outVal;
        }

        static public void SetVolumeLevel(string mixerGroup, float level)
        {
            s_instance.m_masterMixer.SetFloat(mixerGroup + MixerVars.VOLUME, level);
        }

        static public AudioMixer GetMasterMixer()
        {
            return s_instance.m_masterMixer;
        }

        static public void AddSoundEvent<T>(SoundEventDef sndDef) where T : SoundEvent
        {
            AddSoundEvent(GetSoundEventFromPool<T>(sndDef));
        }

        static public T GetSoundEventFromPool<T>(SoundEventDef sndDef) where T : SoundEvent
        {
            List<SoundEvent> sndEvents;
            System.Type type = typeof(T);

            if (s_instance.m_soundEventPool.TryGetValue(type, out sndEvents) == false)
                s_instance.m_soundEventPool.Add(type, new List<SoundEvent>());

            if (s_instance.m_soundEventPool[type].Count <= 0)
                s_instance.m_soundEventPool[type].Add(SoundEvent.CreateEvent(sndDef, type));

            T sndEvent = (T)s_instance.m_soundEventPool[type][0];

            s_instance.m_soundEventPool[type].Remove(sndEvent);

            return sndEvent;
        }

        static public void AddSoundEventToPool(SoundEvent sndE)
        {
            List<SoundEvent> sndEvents;
            if (s_instance.m_soundEventPool.TryGetValue(sndE.GetType(), out sndEvents) == false)
            {
                s_instance.m_soundEventPool.Add(sndE.GetType(), new List<SoundEvent>());
            }

            s_instance.m_soundEventPool[sndE.GetType()].Add(sndE);
        }

        static public void AddSoundEvent(SoundEvent aSndEvent)
        {
            s_instance.m_soundEvents.Add(aSndEvent);
        }

        #region findIsCurrentlyPlaying
        static public bool IsSoundPlaying(SoundType aSndType, string aName)
        {
            SoundObject aSndObj = FindLiveSoundObject(aSndType, aName);

            if (aSndObj != null)
                return true;
            return false;
        }

        static public bool IsSoundPlaying(SoundType aSndType, int aID)
        {
            SoundObject aSndObj = FindLiveSoundObject(aSndType, aID);

            if (aSndObj != null)
                return true;
            return false;
        }

        static public bool IsSoundPlayingUnique(SoundType aSndType, int aID)
        {
            SoundObject aSndObj = LocateLiveSoundObjectWithUniqueID(aSndType, aID);

            if (aSndObj != null)
                return true;
            return false;
        }

        static public bool IsSoundTypePlaying(SoundType aSndType)
        {
            List<SoundObject> list = s_instance.m_liveSoundObjects[aSndType];

            if (list.Count > 0)
                return true;
            return false;
        }
        #endregion

        #region Pause/Resume
        static public void PauseSound(SoundObject sndObj)
        {
            if (sndObj != null)
                sndObj.PauseAudio();
        }

        static public void ResumePlay(SoundObject sndObj)
        {
            if (sndObj != null)
                sndObj.ResumeAudio();
        }

        #endregion

        #region finderFuncts
        public static SoundObject FindLiveSoundObject(SoundType aSndType, string aName)
        {
            List<SoundObject> list = s_instance.m_liveSoundObjects[aSndType];
            return list.Find(snd => snd.GetClipName() == aName);
        }

        public static SoundObject FindLiveSoundObject(SoundType aSndType, int aID)
        {
            List<SoundObject> list = s_instance.m_liveSoundObjects[aSndType];
            return list.Find(snd => snd.GetId() == aID);
        }

        public static List<SoundObject> FindLiveSoundObjects(SoundType aSndType, string aName)
        {
            List<SoundObject> list = s_instance.m_liveSoundObjects[aSndType];
            return list.FindAll(snd => snd.GetClipName() == aName);
        }

        public static List<SoundObject> FindLiveSoundObjects(SoundType aSndType, int aID)
        {
            List<SoundObject> list = s_instance.m_liveSoundObjects[aSndType];
            return list.FindAll(snd => snd.GetId() == aID);
        }

        public static SoundObject LocateLiveSoundObjectWithUniqueID(SoundType aSndType, int aID)
        {
            List<SoundObject> list = s_instance.m_liveSoundObjects[aSndType];
            return list.Find(snd => snd.GetInstanceID() == aID);
        }

        public static List<SoundObject> FindLiveSoundObjects(SoundType aSndType)
        {
            return s_instance.m_liveSoundObjects[aSndType];
        }
        #endregion

        #region ChangeIsLooping
        static public void ChangeSoundIsLooping(SoundObject sndObj, bool isLooping)
        {
            if (sndObj != null)
                sndObj.ChangeIsLooping(isLooping);
        }

        #endregion

        #region StopPlayfuncts

        public static void StopSound(SoundObject sndObj, float fadingSpeed)
        {
            if (fadingSpeed > 0)
                sndObj.FadeOutStop(fadingSpeed);
            else
                sndObj.Disable();
        }

        public static void StopSound(SoundType aSndType, float fadingSpeed)
        {
            List<SoundObject> sndObjs = FindLiveSoundObjects(SoundType.Music);

            for (int i = 0; i < sndObjs.Count; i++)
            {
                if (fadingSpeed > 0)
                    sndObjs[i].FadeOutStop(fadingSpeed);
                else
                    sndObjs[i].Disable();
            }

        }

        static public void RemoveSoundFromActiveSounds(SoundType aSndType, SoundObject aSndObj)
        {
            s_instance.m_liveSoundObjects[aSndType].Remove(aSndObj);
        }

        #endregion

        #region ChangeVolumeFuncts
        static public void ChangeSoundVolume(SoundObject sndObj, float volume, float easeSpeed = -1)
        {
            if (sndObj != null)
                sndObj.ChangeVolume(volume, easeSpeed);
            else
                Debug.LogWarning("SoundController.ChangeSoundVolumeWas Passed null sndObj");
        }

        #endregion

        #region ChangePlayBackSpeedFuncts

        static public void ChangePlayBackSpeed(SoundObject sndObj, float playBackSpeed)
        {
            if (sndObj != null)
                sndObj.ChangePlayBackSpeed(playBackSpeed);
        }


        #endregion

        #region PlaySoundFuncts

        static int AddToLiveSoundsAndReturnUniqueID(SoundType aSndType, SoundObject snd)
        {
            if (snd != null)
            {
                s_instance.m_liveSoundObjects[aSndType].Add(snd);
                return snd.GetInstanceID();
            }
            else
            {
                Debug.LogWarning("SoundController.AddToLiveSoundsAndReturnUniqueID was passed a null sound");
            }
            return int.MinValue;
        }

        //Function Definition: The AttachAndPlaySound will find a soundPool with aSndType
        // And Call AttachSound on soundPool
        static public int AttachAndPlayRandSound(SoundType aSndType, Transform aObject, float fadingSpeed = -1)
        {
            SoundPool soundPool = s_instance.FindObject(aSndType);
            int uniqueID = int.MinValue;

            if (soundPool != null)
            {
                SoundObject snd = soundPool.AttachRandSound(aObject, fadingSpeed);
                uniqueID = AddToLiveSoundsAndReturnUniqueID(aSndType, snd);
            }
            else
                Debug.LogWarning("SoundController.AttachAndPlaySound could not find soundPool of type " + aSndType.ToString());
            return uniqueID;
        }

        static public int AttachAndPlaySound(SoundType aSndType, Transform aObject, string name, float fadingSpeed = -1)
        {
            SoundPool soundPool = s_instance.FindObject(aSndType);
            int uniqueID = int.MinValue;

            if (soundPool != null)
            {
                SoundObject snd = soundPool.AttachSound(aObject, name, fadingSpeed);
                uniqueID = AddToLiveSoundsAndReturnUniqueID(aSndType, snd);
            }
            else
                Debug.LogWarning("SoundController.AttachAndPlaySound could not find soundPool of type " + aSndType.ToString());
            return uniqueID;
        }

        static public int AttachAndPlaySound(SoundType aSndType, Transform aObject, int id, float fadingSpeed = -1)
        {
            SoundPool soundPool = s_instance.FindObject(aSndType);
            int uniqueID = int.MinValue;

            if (soundPool != null)
            {
                SoundObject snd = soundPool.AttachSound(aObject, id, fadingSpeed);
                uniqueID = AddToLiveSoundsAndReturnUniqueID(aSndType, snd);
            }
            else
                Debug.LogWarning("SoundController.AttachAndPlaySound could not find soundPool of type " + aSndType.ToString());
            return uniqueID;
        }

        //Function Definition: The AttachAndPlaySound will find a soundPool with aSndType
        //And Call PlaySoundAtPos on soundPool
        static public int PlayRandSoundAtLocation(SoundType aSndType, Vector3 aPos, float fadingSpeed = -1)
        {
            SoundPool soundPool = s_instance.FindObject(aSndType);
            int uniqueID = int.MinValue;

            if (soundPool != null)
            {
                SoundObject snd = soundPool.PlayRandSoundAtPos(aPos, fadingSpeed);
                uniqueID = AddToLiveSoundsAndReturnUniqueID(aSndType, snd);
            }
            else
                Debug.LogWarning("SoundController.PlaySoundAtLocation could not find soundPool of type " + aSndType.ToString());
            return uniqueID;
        }

        static public int PlaySoundAtLocation(SoundType aSndType, Vector3 aPos, string name, float fadingSpeed = -1)
        {
            SoundPool soundPool = s_instance.FindObject(aSndType);
            int uniqueID = int.MinValue;

            if (soundPool != null)
            {
                SoundObject snd = soundPool.PlaySoundAtPos(aPos, name, fadingSpeed);
                uniqueID = AddToLiveSoundsAndReturnUniqueID(aSndType, snd);
            }
            else
                Debug.LogWarning("SoundController.PlaySoundAtLocation could not find soundPool of type " + aSndType.ToString());

            return uniqueID;
        }

        static public int PlaySoundAtLocation(SoundType aSndType, Vector3 aPos, int id, float fadingSpeed = -1)
        {
            SoundPool soundPool = s_instance.FindObject(aSndType);
            int uniqueID = int.MinValue;

            if (soundPool != null)
            {
                SoundObject snd = soundPool.PlaySoundAtPos(aPos, id, fadingSpeed);
                uniqueID = AddToLiveSoundsAndReturnUniqueID(aSndType, snd);
            }
            else
                Debug.LogWarning("SoundController.PlaySoundAtLocation could not find soundPool of type " + aSndType.ToString());

            return uniqueID;
        }
        #endregion
    }
}
