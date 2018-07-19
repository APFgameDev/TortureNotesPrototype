using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NS_Handlers.NS_EventHandling
{
    public class GamePlayEvent<I>
    {
        List<System.Action<I>> m_subscribers = new List<System.Action<I>>();

        public void AddCallBack(System.Action<I> callBack)
        {
            m_subscribers.Add(callBack);
        }

        public void RemoveCallBack(System.Action<I> callBack)
        {
            m_subscribers.Remove(callBack);
        }

        void RemoveCallBack(int index)
        {
            m_subscribers.RemoveAt(index);
        }

        public void FireEvent(I input)
        {
            for (int i = 0; i < m_subscribers.Count; i++)
            {
                if (m_subscribers[i] != null)
                    m_subscribers[i](input);
                else
                {
                    RemoveCallBack(i);
                    i--;
                }
            }
        }
    }

    public class GamePlayEvent
    {
        List<System.Action> m_subscribers = new List<System.Action>();

        public void AddCallBack(System.Action callBack)
        {
            m_subscribers.Add(callBack);
        }

        public void RemoveCallBack(System.Action callBack)
        {
            m_subscribers.Remove(callBack);
        }

        void RemoveCallBack(int index)
        {
            m_subscribers.RemoveAt(index);
        }

        public void FireEvent()
        {
            for (int i = 0; i < m_subscribers.Count; i++)
            {
                if (m_subscribers[i] != null)
                    m_subscribers[i]();
                else
                {
                    RemoveCallBack(i);
                    i--;
                }
            }
        }
    }
}