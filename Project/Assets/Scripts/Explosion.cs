//  
//  File Name   :   Explosion.cs
//


// Namespaces
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Explosion : MonoBehaviour
{

	// Member Types


	// Member Delegates & Events


	// Member Properties


	// Member Fields
	
	public ParticleSystem m_particles;
	public AudioClip m_audio;

	private float m_lifeSpan = 1.0f;
	private float m_timeElapsed = 0.0f;
	

	// Member Methods

	public void Explode()
	{
		m_timeElapsed = m_lifeSpan;
		m_particles.Play(true);
		AudioSource.PlayClipAtPoint(m_audio, transform.position);
	}


	void Awake()
	{
		m_particles.Stop(true);
		gameObject.SetActive(false);
	}


	void Update()
	{
		m_timeElapsed -= Time.deltaTime;

		if(m_timeElapsed < 0.0f)
		{
			m_particles.Clear(true);
			m_particles.Stop(true);
			ObjectPool.Recycle(gameObject);
		}
	}


};
