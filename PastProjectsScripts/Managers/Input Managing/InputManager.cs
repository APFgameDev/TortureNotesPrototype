using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NS_Managers.NS_GameManagement;


namespace NS_Managers.NS_Input
{
    public class InputManager : SingletonBehaviour<InputManager>
    {
        InputDevice m_inputDevice;

        private void Awake()
        {
            //sets s_instance
            InitSingleton(this);
            //set InputDevice to keyboard for now
            m_inputDevice = new KeyBoardMouseInputDevice();
        }

        void Update()
        {
            m_inputDevice.UpdateInputs();
        }

        void FixedUpdate()
        {
            m_inputDevice.FixedUpdateInputs();
        }

        static public void SubToButtonEvent(GamePlayButtons gameButton, InputEventType inputEventType, System.Action aCallBack)
        {
            Instance.m_inputDevice.SubToButtonEvent(gameButton, inputEventType, aCallBack);
        }
        
        static public bool GetIsButtonPressed(GamePlayButtons gameplayButton)
        {
            return Instance.m_inputDevice.GetIsButtonPressed(gameplayButton);
        }

        static public float GetAxisValue(GamePlayAxis gamePlayAxis)
        {
            return Instance.m_inputDevice.GetAxisValue(gamePlayAxis);
        }
    }
}