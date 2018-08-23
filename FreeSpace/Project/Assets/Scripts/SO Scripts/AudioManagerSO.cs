using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Annotation.SO
{
    [CreateAssetMenu(fileName = "AudioManagerSO", menuName = "Misc SO/Audio Manager", order = 0)]
    public class AudioManagerSO : ScriptableObject
    {
        public AudioManager m_AudioManager;

        private void OnEnable()
        {
            if (m_AudioManager == null)
            {
                m_AudioManager = FindObjectOfType<AudioManager>();
            }
        }

    }
}

