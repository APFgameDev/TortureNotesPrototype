using UnityEngine;

namespace Annotation.SO
{
    [ExecuteInEditMode]
    public class VoidEventMultiListener : MonoBehaviour
    {
        public VoidEvent[] m_Events;
        public UnityEngine.Events.UnityEvent m_Respones;
        
        private void OnEnable()
        {
            if (m_Events != null)
            {
                foreach (VoidEvent Event in m_Events)
                {
                    Event.SubscribeListener(this);
                }
            }
        }

        private void OnDisable()
        {
            if (m_Events != null)
            {
                foreach (VoidEvent Event in m_Events)
                {
                    Event.UnSubscribeListener(this);
                }
            }
        }

        public void OnEventPublished()
        {

        }
    }
}