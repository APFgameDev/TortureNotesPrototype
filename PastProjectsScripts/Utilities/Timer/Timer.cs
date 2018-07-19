using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NS_ObjectPooling;


namespace NS_Timer
{
    //Class Definition: The Timer Class is Game Util to call back a Function 
    //when a set time has passed
    public class Timer : MonoBehaviour
    {
        //function to call when time is reached
        System.Action m_callBackWhenDone = null;
        System.Action<string> m_callBackWithName = null;
        public System.Action<float> m_callBackOnTick = null;
        //var to keep track if timer is finished // default to false
        bool m_done = true;
        float m_timeStartStamp = 0;
        float m_settedSeconds = 0;

        string m_name;


        //Set up and start the timer
        public void SetUpTimer(System.Action aCallBackWhenDone, float aSeconds, System.Action<float> aCallBackOnTick = null)
        {
            enabled = true;
            if (m_done == false)
                StopAllCoroutines();
            m_done = false;
            m_callBackWhenDone = aCallBackWhenDone;
            m_callBackOnTick = aCallBackOnTick;
            m_timeStartStamp = Time.time;
            StartCoroutine(StartTime(m_settedSeconds = aSeconds));
        }


        public void SetUpInterpolator<T>(float aSeconds, T startValue, T endValue, System.Action<T> callBackOfLerped, System.Func<T, T, float, T> lerpFunction, System.Action aCallBackWhenDone = null)
        {
            enabled = true;
            if (m_done == false)
                StopAllCoroutines();
            m_done = false;
            m_callBackWhenDone = aCallBackWhenDone;
            m_timeStartStamp = Time.time;
            StartCoroutine(StartLerper(m_settedSeconds = aSeconds, startValue, endValue, callBackOfLerped, lerpFunction));
        }

        public void SetUpInterpolator<T>(float aSeconds, T startValue, T endValue, System.Action<T> callBackOfLerped, System.Func<T, T, float, T> lerpFunction, string aName, System.Action<string> aCallWithNameWhenDone)
        {
            m_callBackWithName = aCallWithNameWhenDone;
            m_name = aName;
            SetUpInterpolator(aSeconds, startValue, endValue, callBackOfLerped, lerpFunction);
        }

        // Function Definition: Stop Timer // will not call calback
        public void Stop()
        {
            StopAllCoroutines();
            m_done = true;
        }

        void Update()
        {
            if (m_callBackOnTick != null && !m_done)
                m_callBackOnTick(GetTimeRemaining());
        }

        //Function Definition: returns if timer is done
        public bool GetIsDone()
        {
            return m_done;
        }

        public float GetTimeRemaining()
        {
            if (m_done)
                return 0f;

            return Mathf.Max(0, m_timeStartStamp - Time.time + m_settedSeconds);
        }


        //Function Definition: waits a number of seconds then calls our m_callBack
        IEnumerator StartTime(float seconds)
        {
            m_done = false;

            yield return new WaitForSeconds(seconds);

            m_done = true;

            if (m_callBackWhenDone != null)
                m_callBackWhenDone();
            if (m_callBackWithName != null)
                m_callBackWithName(m_name);

            if (gameObject.GetComponent<ReturnBackToPool>() != null)
                gameObject.SetActive(false);
        }

        IEnumerator StartLerper<T>(float seconds, T startValue, T endValue, System.Action<T> aValueToLerp, System.Func<T, T, float, T> lerpFunction)
        {
            m_done = false;

            float time = 0;

            while (m_done == false)
            {
                yield return new WaitForEndOfFrame();
                time += Time.deltaTime;
                aValueToLerp(lerpFunction(startValue, endValue, time / seconds));
                if (time >= seconds)
                {
                    aValueToLerp(lerpFunction(startValue, endValue, 1));
                    break;
                }

            }

            m_done = true;

            if (m_callBackWhenDone != null)
                m_callBackWhenDone();
            if (m_callBackWithName != null)
                m_callBackWithName(m_name);
        }
    }
}