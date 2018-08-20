using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource m_AudioSource;
    [SerializeField] private AudioClip m_KeyPress;

    [Range(0, 1.0f)] [SerializeField] private float m_Volume = 0.5f;

    private void Awake()
    {
        m_AudioSource = Camera.main.GetComponent<AudioSource>();
    }

    public void PlaySound()
    {
        m_AudioSource.PlayOneShot(m_KeyPress, m_Volume);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            PlaySound();
        }
    }
}
