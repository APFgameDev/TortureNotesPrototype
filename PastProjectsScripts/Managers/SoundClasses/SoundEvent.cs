using System.Collections.Generic;
using UnityEngine;
namespace NS_Sound
{
    [System.Serializable]
    public class SoundEventDef
    {
        public SoundType m_soundType;
        public int m_soundID = -1;
        public int m_uniqueSoundID = int.MinValue;
        public string m_soundName = string.Empty;
    }


    public abstract class SoundEvent
    {
        protected SoundEventDef m_sndEventDef;

        bool m_spawnedInPool = false;

        public bool GetIsSpawnedInPool()
        {
            return m_spawnedInPool;
        }

        public SoundEvent(SoundEventDef sndDef)
        {
            m_sndEventDef = sndDef;
        }

        public static SoundEvent CreateEvent(SoundEventDef sndDef, System.Type type)
        {
            SoundEvent sndE = null;          

            if (type == typeof(SoundEventChangePlayBackSpeed))
                sndE = new SoundEventChangePlayBackSpeed(sndDef);

            if (type == typeof(SoundEventChangeVolume))
                sndE = new SoundEventChangeVolume(sndDef);

            if (type == typeof(SoundEventChangeIsLooping))
                sndE = new SoundEventChangeIsLooping(sndDef);

            if (type == typeof(SoundEventAttachAndPlaySound))
                sndE = new SoundEventAttachAndPlaySound(sndDef);

            if (type == typeof(SoundEventPlaySoundAtLocation))
                sndE = new SoundEventPlaySoundAtLocation(sndDef);

            if (type == typeof(SoundEventStopSound))
                sndE = new SoundEventStopSound(sndDef);

            if (type == typeof(SoundEventPause))
                sndE = new SoundEventPause(sndDef);

            if (type == typeof(SoundEventResume))
                sndE = new SoundEventResume(sndDef);

            if (sndE == null)
                Debug.LogError("SoundEvent CreateEvent could not find type to create in list");
            else
                sndE.m_spawnedInPool = true;

            return sndE;
        }

        //Allows for batch updates to sound objects
        protected List<SoundObject> ReturnSoundObjects()
        {

            List<SoundObject> sndObj = new List<SoundObject>();

            if (m_sndEventDef.m_soundID >= 0)
                sndObj.InsertRange(sndObj.Count, SoundController.FindLiveSoundObjects(m_sndEventDef.m_soundType, m_sndEventDef.m_soundID));
            else if (m_sndEventDef.m_soundName != string.Empty)
                sndObj.InsertRange(sndObj.Count, SoundController.FindLiveSoundObjects(m_sndEventDef.m_soundType, m_sndEventDef.m_soundName));
            else if (m_sndEventDef.m_uniqueSoundID != int.MinValue)
                sndObj.Add(SoundController.LocateLiveSoundObjectWithUniqueID(m_sndEventDef.m_soundType, m_sndEventDef.m_uniqueSoundID));
            else
                sndObj.InsertRange(sndObj.Count, SoundController.FindLiveSoundObjects(m_sndEventDef.m_soundType));

            return sndObj;
        }
            
        public virtual void ProcessEvent()
        {
            List<SoundObject> sndObj = ReturnSoundObjects();

            for(int i = 0; i < sndObj.Count;i++)
            {
                ApplyEvent(sndObj[i]);
            }
        }

        protected abstract void ApplyEvent(SoundObject sndObj);
    }


    public class SoundEventChangePlayBackSpeed : SoundEvent
    {
        public float m_playBackSpeed;

        public SoundEventChangePlayBackSpeed(SoundEventDef sndDef, float playBackSpeed = 1) : base(sndDef)
        {
            m_playBackSpeed = playBackSpeed;
        }

        override protected void ApplyEvent(SoundObject sndObj)
        {
            SoundController.ChangePlayBackSpeed(sndObj, m_playBackSpeed);
        }
    }

    public class SoundEventChangeVolume : SoundEvent
    {
        public float m_volume;
        public float m_easeSpeed;

        public SoundEventChangeVolume( SoundEventDef sndDef, float volume = 1, float easeSpeed = -1) : base(sndDef)
        {
            m_volume = volume;
            m_easeSpeed = easeSpeed;
        }

        override protected void ApplyEvent(SoundObject sndObj)
        {
            SoundController.ChangeSoundVolume(sndObj, m_volume, m_easeSpeed);
        }
    }

    public class SoundEventStopSound : SoundEvent
    {
        float m_fadingSpeed;

        public SoundEventStopSound(SoundEventDef sndDef, float fadingSpeed = -1) : base(sndDef)
        {
            m_fadingSpeed = fadingSpeed;
        }

        override protected void ApplyEvent(SoundObject sndObj)
        {
            SoundController.StopSound(sndObj, m_fadingSpeed);
        }
    }

    public class SoundEventPlaySoundAtLocation : SoundEvent
    {
        public float m_fadingSpeed;
        public Vector3 m_pos;
        public delegate void SetUniqueIDDel(int id);
        public SetUniqueIDDel m_SetUniqueIDDel;


        public SoundEventPlaySoundAtLocation(SoundEventDef sndDef, float fadingSpeed = -1, SetUniqueIDDel aSetUniqueIDDel = null) : base(sndDef)
        {
            m_fadingSpeed = fadingSpeed;
            m_pos = UnityEngine.Vector3.zero;
            m_SetUniqueIDDel = aSetUniqueIDDel;
        }

        public SoundEventPlaySoundAtLocation(SoundEventDef sndDef, Vector3 pos, float fadingSpeed = -1, SetUniqueIDDel aSetUniqueIDDel = null) : base(sndDef)
        {
            m_fadingSpeed = fadingSpeed;
            m_pos = pos;
            m_SetUniqueIDDel = aSetUniqueIDDel;
        }

        override protected void ApplyEvent(SoundObject sndObj)
        {
        }

        override public void ProcessEvent()
        {
            int uniqueID;

            if (m_sndEventDef.m_soundID >= 0)
                uniqueID = SoundController.PlaySoundAtLocation(m_sndEventDef.m_soundType, m_pos, m_sndEventDef.m_soundID, m_fadingSpeed);
            else if (m_sndEventDef.m_soundName != string.Empty)
                uniqueID = SoundController.PlaySoundAtLocation(m_sndEventDef.m_soundType, m_pos, m_sndEventDef.m_soundName, m_fadingSpeed);
            else
                uniqueID = SoundController.PlayRandSoundAtLocation(m_sndEventDef.m_soundType, m_pos, m_fadingSpeed);

            if (m_SetUniqueIDDel != null)
                m_SetUniqueIDDel(uniqueID);
        }
    }

    public class SoundEventAttachAndPlaySound : SoundEvent
    {
        public float m_fadingSpeed;
        public Transform m_obj;
        public delegate void SetUniqueIDDel(int id);
        SetUniqueIDDel m_SetUniqueIDDel;

        public SoundEventAttachAndPlaySound(SoundEventDef sndEventDef, UnityEngine.Transform obj = null, float fadingSpeed = -1 , SetUniqueIDDel aSetUniqueIDDel = null) : base(sndEventDef)
        {
            m_fadingSpeed = fadingSpeed;
            m_obj = obj;
            m_SetUniqueIDDel = aSetUniqueIDDel;
        }

        override protected void ApplyEvent(SoundObject sndObj)
        {
        }

        override public void ProcessEvent()
        {
            int uniqueID;

            if (m_sndEventDef.m_soundID >= 0)
                uniqueID = SoundController.AttachAndPlaySound(m_sndEventDef.m_soundType, m_obj, m_sndEventDef.m_soundID, m_fadingSpeed);
            else if (m_sndEventDef.m_soundName != string.Empty)
                uniqueID = SoundController.AttachAndPlaySound(m_sndEventDef.m_soundType, m_obj, m_sndEventDef.m_soundName, m_fadingSpeed);
            else
                uniqueID = SoundController.AttachAndPlayRandSound(m_sndEventDef.m_soundType, m_obj, m_fadingSpeed);

            if (m_SetUniqueIDDel != null)
                m_SetUniqueIDDel(uniqueID);
        }
    }


    public class SoundEventChangeIsLooping : SoundEvent
    {
        public bool m_isLooping;

        public SoundEventChangeIsLooping(SoundEventDef sndEventDef, bool isLooping = true) : base(sndEventDef)
        {
            m_isLooping = isLooping;
        }

        override protected void ApplyEvent(SoundObject sndObj)
        {
            SoundController.ChangeSoundIsLooping(sndObj, m_isLooping);
        }
    }

    public class SoundEventPause: SoundEvent
    {
        public SoundEventPause(SoundEventDef sndEventDef) : base(sndEventDef)
        {
        }

        override protected void ApplyEvent(SoundObject sndObj)
        {
            SoundController.PauseSound(sndObj);
        }
    }

    public class SoundEventResume : SoundEvent
    {
        public SoundEventResume(SoundEventDef sndEventDef) : base(sndEventDef)
        {
        }

        override protected void ApplyEvent(SoundObject sndObj)
        {
            SoundController.ResumePlay(sndObj);
        }
    }
}