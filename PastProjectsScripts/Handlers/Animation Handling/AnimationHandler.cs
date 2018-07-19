using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NS_ArrayExtensions;



namespace NS_AnimationHandler
{
    //	Class Definition: TheAnimationHandler grabs all AnimationStateEventHandler from gameobjects Animator,
    //Animation Handler sets up callbacks to update the member variable m_currentAnimation
    public class AnimationHandler : MonoBehaviour
    {
        [SerializeField]
        string[] m_currentAnimations;
        Dictionary<string, AnimationStateEventHandler> m_animStateHandlers = new Dictionary<string, AnimationStateEventHandler>();

        [SerializeField]
        Animator m_animator;

        [SerializeField]
        float curSpeed = 1;

        [SerializeField]
        NS_Sound.SoundType m_soundTypeForSoundEvent;

        void Awake()
        {

            m_animator = GetComponent<Animator>();
            AnimationStateEventHandler[] animStateHandlers = m_animator.GetBehaviours<AnimationStateEventHandler>();

            UnityExtensionMethods.AddArrayToDictionary(ref m_animStateHandlers, animStateHandlers, (a) =>
            {
                a.AddReportStateEnterCallback(SetCurrentAnimName);
                return a.GetName();
            });

          
            m_currentAnimations = new string[m_animator.layerCount];
        }

        public void SetAnimatorSpeed(float speed)
        {
            m_animator.speed = curSpeed = speed;
        }

        public bool GetIsStateAtEnd(int layerIndex, string stateName)
        {
            if (GetCurrentAnimName(0) == stateName)
            {
                bool returnVal = m_animator.GetCurrentAnimatorStateInfo(layerIndex).normalizedTime >= 1 && m_animator.IsInTransition(0) == false;
                return returnVal;
            }
            else
            {
                //Debug.Log("Requested is state ended for non-current animation WWstate.");
                return true;
            }
        }

        public float GetStatesExpiredTime(int layerIndex, string stateName)
        {
            if (GetCurrentAnimName(0) == stateName && m_animator.IsInTransition(0) == false)
            {
                float length = m_animator.GetCurrentAnimatorStateInfo(layerIndex).length;

                return Mathf.Max(0, m_animator.GetCurrentAnimatorStateInfo(layerIndex).normalizedTime * length - length);
            }
            else
            {
                //Debug.Log("Not InState animation state.");
                return 0;
            }
        }


        public void SetAnimatorParam(string paramName, bool paramValue)
        {
            m_animator.SetBool(paramName, paramValue);
        }

        public void SetAnimatorParam(string paramName, float paramValue)
        {
            m_animator.SetFloat(paramName, paramValue);
        }

        public void SetAnimatorParam(string paramName, int paramValue)
        {
            m_animator.SetInteger(paramName, paramValue);
        }

        public void GetAnimatorParam(string paramName, ref int paramValue)
        {
            paramValue = m_animator.GetInteger(paramName);
        }

        public void GetAnimatorParam(string paramName, ref float paramValue)
        {
            paramValue = m_animator.GetFloat(paramName);
        }

        public void GetAnimatorParam(string paramName, ref bool paramValue)
        {
               paramValue = m_animator.GetBool(paramName);
        }

        //Function Definition: returns m_currentAnimation
        public string GetCurrentAnimName(int aLayer)
        {
            if (aLayer >= m_currentAnimations.Length)
                return string.Empty;
            if (m_currentAnimations[aLayer] == null)
                return string.Empty;

            return m_currentAnimations[aLayer];
        }

        public bool GetIsInTransition(int layer)
        {
           return m_animator.IsInTransition(0);
        }
        public string GetCurrentAnimName(string aLayer)
        {
            return GetCurrentAnimName(m_animator.GetLayerIndex(aLayer));
        }

        //Function Definition: adds callback to a state
        public void AddStateEnterCallBack(string stateName, System.Action aCallBack)
        {

            if (DebugTestLogger.TestHasKey(m_animStateHandlers, stateName))
            {
                m_animStateHandlers[stateName].AddCallBackOnEnter(aCallBack);
            }
        }

        public void AddStateEnterCallBack(string[] stateNames, System.Action aCallBack)
        {
            for (int i = 0; i < stateNames.Length; i++)
            {
                if (DebugTestLogger.TestHasKey(m_animStateHandlers, stateNames[i]))
                    m_animStateHandlers[stateNames[i]].AddCallBackOnEnter(aCallBack);
            }
        }

        //Function Definition: adds callback to a state
        public void AddStateExitCallBack(string stateName, System.Action aCallBack)
        {
            if (DebugTestLogger.TestHasKey(m_animStateHandlers, stateName))
                m_animStateHandlers[stateName].AddCallBackOnExit(aCallBack);
        }

        public void AddStateExitCallBack(string[] stateNames, System.Action aCallBack)
        {
            for (int i = 0; i < stateNames.Length; i++)
            {
                if (DebugTestLogger.TestHasKey(m_animStateHandlers, stateNames[i]))
                    m_animStateHandlers[stateNames[i]].AddCallBackOnExit(aCallBack);
            }
        }

        //Function Definition: adds callback to a state
        public void AddStateUpdateCallBack(string stateName, System.Action aCallBack)
        {
            if (DebugTestLogger.TestHasKey(m_animStateHandlers, stateName))
                m_animStateHandlers[stateName].AddCallBackOnUpdate(aCallBack);
        }

        public void AddStateUpdateCallBack(string[] stateNames, System.Action aCallBack)
        {
            for (int i = 0; i < stateNames.Length; i++)
            {
                if (DebugTestLogger.TestHasKey(m_animStateHandlers, stateNames[i]))
                    m_animStateHandlers[stateNames[i]].AddCallBackOnUpdate(aCallBack);
            }
        }


        public void SetLayerWeight(int layerIndex, float weight)
        {
            m_animator.SetLayerWeight(layerIndex, weight);
        }

        public void SetLayerWeight(string layerName, float weight)
        {
            int index = m_animator.GetLayerIndex(layerName);
            SetLayerWeight(index, weight);
        }

        public float GetLayerWeight(string layerName)
        {
            return m_animator.GetLayerWeight(m_animator.GetLayerIndex(layerName));
        }


        //Function Definition: call back used by AnimationStateEventHandler
        void SetCurrentAnimName(string stateName, int aLayer)
        {
            if (aLayer >= m_currentAnimations.Length)
                return;

            m_currentAnimations[aLayer] = stateName;
        }

        public void SetAnimatorActive(bool isActive)
        {
            m_animator.enabled = isActive;
        }

        public void AttachAndPlaySound(string clipName)
        {
            NS_Sound.SoundController.AttachAndPlaySound(m_soundTypeForSoundEvent, transform, clipName);
        }
    }

}