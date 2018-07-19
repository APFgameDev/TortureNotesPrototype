using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NS_Managers.NS_Input
{
    public enum SwipeEventType
    {
        Left_Tap,
        Left_HoldStart,
        Left_HoldEnd,
        Left_Fwd,
        Left_FwdRight,
        Left_Right,
        Left_Back,
        Left_BackRight,
   

        Right_Tap,
        Right_HoldStart,
        Right_HoldEnd,
        Right_Fwd,
        Right_FwdLeft,
        Right_Left,
        Right_Back,
        Right_BackLeft
    }

    public class SwipeInputManager : NS_GameManagement.SingletonBehaviour<SwipeInputManager>
    {
        static Dictionary<SwipeEventType, System.Action<SwipeEventType>> m_InputSubscribers = new Dictionary<SwipeEventType, System.Action<SwipeEventType>>();
        static Dictionary<int, TouchInfo> m_touchInfo = new Dictionary<int, TouchInfo>();
        const float TIME_FOR_HOLD = 0.5f;
        const float HOLD_POS_TOLERANCEPercentage = 0.1f;

        [SerializeField]
        float SwipeMin = 0;

        private void Start()
        {
            SwipeMin = HOLD_POS_TOLERANCEPercentage * Mathf.Sqrt(Screen.width * Screen.width + Screen.height * Screen.height);
        }

        void Update()
        {
            Touch[] touches = Input.touches;

            for (int i = 0; i < touches.Length; i++)
            {
                int fingerId = touches[i].fingerId;

                switch (touches[i].phase)
                {
                    case TouchPhase.Began:
                        m_touchInfo[fingerId] = new TouchInfo(Time.unscaledTime, touches[i].position);
                        break;
                    case TouchPhase.Ended:
                        ProcessTouch(fingerId, touches[i].position);
                        break;
                    default:
                        ProcessHold(fingerId, touches[i].position);
                        break;
                }
            }
        }

        bool GetIsSwipe(int fingerId,Vector2 nowPos)
        {
            return Vector2.SqrMagnitude(m_touchInfo[fingerId].StartPos - nowPos) > SwipeMin;
        }

        public static void SubscribeToInput(SwipeEventType swipeEventType, System.Action<SwipeEventType> callback)
        {
            if (m_InputSubscribers.ContainsKey(swipeEventType))
                m_InputSubscribers[swipeEventType] += callback;
            else
                m_InputSubscribers.Add(swipeEventType, callback);
        }

        void ProcessHold(int fingerId,Vector2 endpos)
        {
            if (GetIsSwipe(fingerId, endpos) == false &&
                m_touchInfo[fingerId].m_heldHasStarted == false &&
                Time.unscaledTime - m_touchInfo[fingerId].StartTime > TIME_FOR_HOLD)
            {
                m_touchInfo[fingerId].m_heldHasStarted = true;

                if (IsPixelPosLeftSideOfScreen(m_touchInfo[fingerId].StartPos))
                    CallSubscribers(SwipeEventType.Left_HoldStart);
                else
                    CallSubscribers(SwipeEventType.Right_HoldStart);
            }
        }

        void ProcessTouch(int fingerId, Vector2 endPos)
        {
            if (m_touchInfo[fingerId].m_heldHasStarted == true)
            {
                if (IsPixelPosLeftSideOfScreen(m_touchInfo[fingerId].StartPos))
                    CallSubscribers(SwipeEventType.Left_HoldEnd);
                else
                    CallSubscribers(SwipeEventType.Right_HoldEnd);
            }
            else if (GetIsSwipe(fingerId,endPos))
            {
                Vector2 swipeDir = (endPos - m_touchInfo[fingerId].StartPos).normalized;

                float verticalAmount = Vector2.Dot(Vector2.up, swipeDir);
                float horizontalAmount = Vector2.Dot(Vector2.right, swipeDir);

                if (IsPixelPosLeftSideOfScreen(m_touchInfo[fingerId].StartPos))
                {
                    if (verticalAmount > 0.9f)
                        CallSubscribers(SwipeEventType.Left_Fwd);
                    else if (horizontalAmount > 0.9f)
                        CallSubscribers(SwipeEventType.Left_Right);
                    else if (verticalAmount < -0.9f)
                        CallSubscribers(SwipeEventType.Left_Back);
                    else if (verticalAmount > 0)
                        CallSubscribers(SwipeEventType.Left_FwdRight);
                    else
                        CallSubscribers(SwipeEventType.Left_BackRight);
                }
                else
                {
                    if (verticalAmount > 0.9f)
                        CallSubscribers(SwipeEventType.Right_Fwd);
                    else if (horizontalAmount < -0.9f)
                        CallSubscribers(SwipeEventType.Right_Left);
                    else if (verticalAmount < -0.9f)
                        CallSubscribers(SwipeEventType.Right_Back);
                    else if (verticalAmount > 0)
                        CallSubscribers(SwipeEventType.Right_FwdLeft);
                    else
                        CallSubscribers(SwipeEventType.Right_BackLeft);
                }
            }
            else
            {
                if (IsPixelPosLeftSideOfScreen(m_touchInfo[fingerId].StartPos))
                    CallSubscribers(SwipeEventType.Left_Tap);
                else
                    CallSubscribers(SwipeEventType.Right_Tap);
            }
        }

        void CallSubscribers(SwipeEventType swipeEventType)
        {
            if (m_InputSubscribers.ContainsKey(swipeEventType) && m_InputSubscribers[swipeEventType] != null)
                m_InputSubscribers[swipeEventType](swipeEventType);
        }

        bool IsPixelPosLeftSideOfScreen(Vector2 pos)
        {
            return pos.x < Camera.main.pixelWidth * 0.5f;
        }

        public static void ClearSubscribers()
        {
            m_InputSubscribers = new Dictionary<SwipeEventType, System.Action<SwipeEventType>>();
        }


        class TouchInfo
        {
            //Time.time
            public float StartTime { get; private set; }
            //pixel coords
            public Vector2 StartPos { get; private set; }

            public bool m_heldHasStarted;

            public TouchInfo(float startTime, Vector3 startPos)
            {
                StartTime = startTime;
                StartPos = startPos;
                m_heldHasStarted = false;
            }
        }
    }
}