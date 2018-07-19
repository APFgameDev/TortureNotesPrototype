using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using NS_Handlers.NS_EventHandling;

namespace NS_Managers.NS_Input
{
    public enum GamePlayButtons
    {
        Shoot = 0x0001,
        Jump = 0x0002,
        Escape = 0x0004,
        END_GPD       = 0x0008,

        //move5       = 0x0010,
        //move6       = 0x0020,
        //move7       = 0x0040,
        //move8       = 0x0080,

        //move9       = 0x0100,
        //move10      = 0x0200,
        //move11      = 0x0400,
        //move12      = 0x0800,

        //move13      = 0x1000,
        //move14      = 0x2000,
        //move15      = 0x4000,
        //move16      = 0x8000
    }
    public enum GamePlayAxis
    {
        LookX = 0x0001,
        LookY = 0x0002,
        FwdMove = 0x0004,
        SideMove = 0x0008,
        //move5       = 0x0010,
        //move6       = 0x0020,
        //move7       = 0x0040,
        //move8       = 0x0080,

        //move9       = 0x0100,
        //move10      = 0x0200,
        //move11      = 0x0400,
        //move12      = 0x0800,

        //move13      = 0x1000,
        //move14      = 0x2000,
        //move15      = 0x4000,
        //move16      = 0x8000
    }

    public enum InputEventType
    {
        OnPressed,
        OnReleased,
        OnHeld,
        FixedOnPressed,
        FixedOnReleased,
        FixedOnHeld,
        END_IET
    }

    public interface InputDevice
    {
        void UpdateInputs();
        void FixedUpdateInputs();

        float GetAxisValue(GamePlayAxis axis);
        bool GetIsButtonPressed(GamePlayButtons gameButton);
        bool GetFixedIsButtonPressed(GamePlayButtons gameButton);

        void SubToButtonEvent(GamePlayButtons gameButton, InputEventType inputEventType, Action aCallBack);
        void UnSubToButtonEvent(GamePlayButtons gameButton, InputEventType inputEventType, Action aCallBack);
    }

    //GameInputDevice 
    //templates button type template axis type 
    //used when quering inputs
    public abstract class GameInputDevice<B, A> : InputDevice
    {
        Dictionary<GamePlayButtons, B[]> m_gamePlayButtons = new Dictionary<GamePlayButtons, B[]>();
        Dictionary<GamePlayAxis, A[]> m_gamePlayAxis = new Dictionary<GamePlayAxis, A[]>();

        int m_buttonUpdateFlags = 0;
        int m_buttonFixedUpdateFlags = 0;

        Dictionary<GamePlayAxis, float> m_axisValues = new Dictionary<GamePlayAxis, float>();
        Dictionary<InputEventType, Dictionary<GamePlayButtons, GamePlayEvent>> m_eventDictionary = new Dictionary<InputEventType, Dictionary<GamePlayButtons, GamePlayEvent>>();

        public GameInputDevice()
        {
            // setup event dictionaries
            for (int i = 0; i < (int)InputEventType.END_IET; i++)
                SetUpEventDictionaries((InputEventType)i);
        }

        void SetUpEventDictionaries(InputEventType inputEventType)
        {
            m_eventDictionary[inputEventType] = new Dictionary<GamePlayButtons, GamePlayEvent>();

            for (int i = 1; i < (int)GamePlayButtons.END_GPD; i *= 2)
                m_eventDictionary[inputEventType][(GamePlayButtons)i] = new GamePlayEvent();
        }

        //these functions must be defined as ways to query Input for the device
        protected abstract bool CheckButtonInput(B input);
        protected abstract float GetAxisValue(A input);

        // call to update inputs
        public void UpdateInputs()
        {
            //Update Input flags and Fire Update for Input Events
            ProcessButtons();
            ProcessAxis();
        }
        public void FixedUpdateInputs()
        {
            bool gameButtonNowPressed;
            bool gameButtonWasPressed;

            //copy m_buttonUpdateFlags and Fire Fixed Update for Input Events
            for (int i = 1; i < (int)GamePlayButtons.END_GPD; i *= 2)
            {
                gameButtonNowPressed = (m_buttonUpdateFlags & i) == 0;
                gameButtonWasPressed = (m_buttonFixedUpdateFlags & i) == 0;

                if (gameButtonNowPressed)
                    m_eventDictionary[InputEventType.FixedOnHeld][(GamePlayButtons)i].FireEvent();

                if (gameButtonWasPressed != gameButtonNowPressed)
                {
                    if (gameButtonNowPressed && gameButtonWasPressed == false)
                    {
                        m_eventDictionary[InputEventType.FixedOnPressed][(GamePlayButtons)i].FireEvent();
                        m_buttonFixedUpdateFlags |= i;
                    }
                    else if (gameButtonNowPressed == false && gameButtonWasPressed)
                    {
                        m_eventDictionary[InputEventType.FixedOnReleased][(GamePlayButtons)i].FireEvent();
                        m_buttonFixedUpdateFlags ^= i;
                    }
                }
            }
        }

        //Access Inputs Values
        public float GetAxisValue(GamePlayAxis axis)
        {
            return m_axisValues[axis];
        }
        public bool GetIsButtonPressed(GamePlayButtons gameButton)
        {
            return (m_buttonUpdateFlags & (int)gameButton) != 0;
        }
        public bool GetFixedIsButtonPressed(GamePlayButtons gameButton)
        {
            return (m_buttonFixedUpdateFlags & (int)gameButton) != 0;
        }

        //call these functions to subscribe to events
        public void SubToButtonEvent(GamePlayButtons gameButton, InputEventType inputEventType, Action aCallBack)
        {
            m_eventDictionary[inputEventType][gameButton].AddCallBack(aCallBack);
        }
        public void UnSubToButtonEvent(GamePlayButtons gameButton, InputEventType inputEventType, Action aCallBack)
        {
            m_eventDictionary[inputEventType][gameButton].RemoveCallBack(aCallBack);
        }

        //these functions are overridable to allow for additional processes
        protected virtual void ProcessButtons()
        {
            foreach (var gameButton in m_gamePlayButtons)
                QueryButtonInput(gameButton.Key, gameButton.Value);
        }
        protected virtual void ProcessAxis()
        {
            foreach (var gameAxis in m_gamePlayAxis)
            {
                m_axisValues[gameAxis.Key] = 0;

                for (int i = 0; i < gameAxis.Value.Length; i++)
                    AddValueToAxis(gameAxis.Key, GetAxisValue(gameAxis.Value[i]));
            }
        }

        //safe way to add value to axis
        protected void AddValueToAxis(GamePlayAxis gamePlayAxis, float value)
        {
            m_axisValues[gamePlayAxis] = Mathf.Clamp(m_axisValues[gamePlayAxis] + value, -1, 1);
        }

        //This is where buttons are queried and events are sent off
        void QueryButtonInput(GamePlayButtons gamePlayButton, B[] inputs)
        {
            bool keyWasPressed = (m_buttonUpdateFlags & (int)gamePlayButton) == 0;
            bool keyNowPressed = false;

            for (int i = 0; i < inputs.Length; i++)
            {
                if (CheckButtonInput(inputs[i]))
                {
                    m_eventDictionary[InputEventType.OnHeld][gamePlayButton].FireEvent();

                    if (keyWasPressed == false)
                    {
                        m_eventDictionary[InputEventType.OnPressed][gamePlayButton].FireEvent();
                        m_buttonUpdateFlags |= (int)gamePlayButton;
                    }

                    keyNowPressed = true;
                    break;
                }
            }

            if (keyWasPressed && keyNowPressed == false)
            {
                m_eventDictionary[InputEventType.OnReleased][gamePlayButton].FireEvent();
                m_buttonUpdateFlags ^= (int)gamePlayButton;
            }
        }

        //use this function to initialize the inputs for GamePlayButtons
        protected void SetUpGamePlayInputsOnButton(GamePlayButtons gamePlayButton, B[] inputs)
        {
            m_gamePlayButtons.Add(gamePlayButton, inputs);
        }
        //use this function to initialize the inputs for GamePlayAxis
        protected void SetUpGamePlayInputsOnAxis(GamePlayAxis gamePlayAxis, A[] Inputs)
        {
            m_gamePlayAxis.Add(gamePlayAxis, Inputs);
        }

 
    }
}