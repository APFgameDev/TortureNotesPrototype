using UnityEngine;
using UnityEngine.Events;
using Annotation.SO;

namespace Annotation.SO
{
    [CreateAssetMenu(fileName = "WhiteboardSO", menuName = "Whiteboard SO/Whiteboard", order = 0)]
    public class WhiteboardSO : ScriptableObject
    {
        [SerializeField]
        private Whiteboard m_CurrentWhiteboard;

        public void SetCurrentWhiteboard(Whiteboard whiteboard)
        {
            if (m_CurrentWhiteboard != null)
            {
                m_CurrentWhiteboard.gameObject.SetActive(false);
            }

            m_CurrentWhiteboard = whiteboard;
        }

        private void OnEnable()
        {
            m_CurrentWhiteboard = null;
        }

        private void OnDisable()
        {
            m_CurrentWhiteboard = null;
        }
    }
}