using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NS_ObjectPooling;
using NS_Managers.NS_GameManagement;

namespace NS_Timer
{
    //	Class Definition: The Create Timer class is a singleton service that will have a pool of available timers to use 
    public class TimerPool : SingletonBehaviour<TimerPool> 
    {
        ObjectPool m_timerPool = new ObjectPool();

        // Use this for initialization
        void Start()
        {
            m_timerPool.m_cloneObject = new GameObject("Timer");
            m_timerPool.m_cloneObject.SetActive(false);
            m_timerPool.m_cloneObject.AddComponent<Timer>();
            m_timerPool.m_cloneObject.transform.parent = transform;
            m_timerPool.m_expandAmount = 1;
            m_timerPool.m_autoExpand = true;
          
            m_timerPool.Start();
        }

        static public Timer GetATimer(bool returnToPoolWhenDone)
        {

            GameObject gO = Instance.m_timerPool.GetObjectFromPool();
            if (gO == null)
            {
                Debug.LogError("TimerPool Pool Is Empty!!"); // this should never happen
                return null;
            }
            if (returnToPoolWhenDone == false)
            {

                ReturnBackToPool returnBackToPool = gO.GetComponent<ReturnBackToPool>();
                returnBackToPool.m_returnPool = null;
                Destroy(returnBackToPool);
            }
            else
                gO.transform.parent = Instance.transform;

            gO.SetActive(true);
            return gO.GetComponent<Timer>();
        }

        //Function Definition: Grabs available timer from timer pool and starts it up
        static public Timer CreateTimer(System.Action aCallBack, float seconds)
        {
            GameObject gO = Instance.m_timerPool.GetObjectFromPool();
            if (gO != null)
            {
                Timer timer = gO.GetComponent<Timer>();
                timer.SetUpTimer(aCallBack, seconds);
                return timer;
            }
            else
                Debug.LogError("TimerPool Pool Is Empty!!"); // this should never happen
            return null;
        }
    }
}
