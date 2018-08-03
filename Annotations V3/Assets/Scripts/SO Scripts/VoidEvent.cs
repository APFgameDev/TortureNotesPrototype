using System.Collections.Generic;
using UnityEngine;

namespace Brinx.SO
{
    /// <summary>ScriptableObject containing an event that several listeners respond to when published</summary>
    [CreateAssetMenu(fileName = "VoidEvent", menuName = "SO Events/Void", order = 0)]
    public class VoidEvent : ScriptableObject
    {
        List<VoidEventListener> m_Listeners = new List<VoidEventListener>();

        public void Publish()
        {
            for (int i = m_Listeners.Count - 1; i >= 0; i--)
            {
                m_Listeners[i].OnEventPublished();
            }
        }

        /// <summary>Adds argument to list of listeners invoked when this GameEvent is published</summary>
        /// <param name="a_ToSubscribe">Listener to invoke</param>
        public void SubscribeListener(VoidEventListener a_ToSubscribe)
        {
            if (!m_Listeners.Contains(a_ToSubscribe))
            {
                m_Listeners.Add(a_ToSubscribe);
            }
        }

        /// <summary>Removes argument listener from list so it no longer listens to event</summary>
        /// <param name="a_ToUnSubscribe">Listener to remove</param>
        public void UnSubscribeListener(VoidEventListener a_ToUnSubscribe)
        {
            if (m_Listeners.Contains(a_ToUnSubscribe))
            {
                m_Listeners.Remove(a_ToUnSubscribe);
            }
        }
    }
}