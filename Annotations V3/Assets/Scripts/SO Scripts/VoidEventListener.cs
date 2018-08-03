using UnityEngine;

namespace NS_Annotation.NS_SO
{
    /// <summary>Listens to the linked VoidEvent and invokes a response</summary>
    [ExecuteInEditMode]
    public class VoidEventListener : MonoBehaviour
    {
        public VoidEvent m_Event;
        public UnityEngine.Events.UnityEvent m_Response;

        private void OnEnable()
        {
            if (m_Event != null)
            {
                m_Event.SubscribeListener(this);
            }
        }

        private void OnDisable()
        {
            if (m_Event != null)
            {
                m_Event.UnSubscribeListener(this);
            }
        }

        public void OnEventPublished()
        {
            m_Response.Invoke();
        }
    }
}