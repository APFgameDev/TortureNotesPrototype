using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NS_AnimationHandler
{

    struct AnimIDs
    {
        public const string DefaultAnimID = "AnimID";
    }

    public delegate void CallBackDel(string stateName); //– Delegate type used for this class events callback 
    //	Class Definition: TheAnimationStateEventHandler allows for configurable callbacks
    //when a animation state is entered in the animator
    public class AnimationStateEventHandler : StateMachineBehaviour
    {
        [SerializeField]
        string m_stateName = string.Empty; // – animation state name 
    
        event System.Action OnStateEnterEvent; // Event 
        event System.Action OnStateExitEvent; // Event
        event System.Action OnStateUpdateEvent; // Event

        event System.Action<string, int> OnReportStateEvent;

        //returns m_ stateName
        public string GetName()
        {
            return m_stateName;
        }

        public  void AddReportStateEnterCallback(System.Action<string, int> aReportState)
        {
            OnReportStateEvent += aReportState;
        }


        //addsCallback to OnStateEnter
        public void AddCallBackOnEnter(System.Action aCallBack)
        {

            OnStateEnterEvent += aCallBack;
        }
        //addsCallback to OnStateExit
        public void AddCallBackOnExit(System.Action aCallBack)
        {
            OnStateExitEvent += aCallBack;
        }

        public void AddCallBackOnUpdate(System.Action aCallback)
        {
            OnStateUpdateEvent += aCallback;
        }

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (OnReportStateEvent != null)
                OnReportStateEvent(m_stateName, layerIndex);
            
            if (OnStateEnterEvent != null)
                OnStateEnterEvent();

        }
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (OnStateUpdateEvent != null)
                OnStateUpdateEvent();
            base.OnStateUpdate(animator, stateInfo, layerIndex);
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (OnStateExitEvent != null)
                OnStateExitEvent();
        }


    }
}
