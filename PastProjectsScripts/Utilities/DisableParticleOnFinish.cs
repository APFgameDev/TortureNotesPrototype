using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableParticleOnFinish : MonoBehaviour
{
    ParticleSystem m_ParticleSystem;

    [SerializeField]
    float m_DurationOffset = 0.0f;

	// Use this for initialization
	void Awake ()
    {
        m_ParticleSystem = GetComponent<ParticleSystem>();
	}
    
    private void OnEnable()
    {
        StartCoroutine(DisableParticleWhenDone());
    }

    private IEnumerator DisableParticleWhenDone()
    {
        yield return new WaitForSeconds(m_ParticleSystem.main.duration + m_DurationOffset);
        gameObject.SetActive(false);
    }
}
