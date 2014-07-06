//  
//  File Name   :   ParticleTrail.cs
//


// Namespaces
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ParticleTrail : MonoBehaviour
{

	// Member Types


	// Member Delegates & Events


	// Member Properties


	// Member Fields

	private ParticleSystem m_particles;
	private float m_lifespanAfterDetach;
	private bool m_isAttached;

	// Member Methods

	public void Initialise(Transform _target, float _lifespan)
	{
		//m_particles.Simulate(m_particles.startLifetime, true, true);
		m_particles.Play(true);
		m_lifespanAfterDetach = _lifespan;

		transform.parent = _target;
		m_isAttached = true;
	}


	public void Detach()
	{
		m_isAttached = false;
		this.EnterActiveGroup();
	}


	void Awake()
	{
		m_particles = GetComponent<ParticleSystem>();
		m_particles.Stop(true);
		gameObject.SetActive(false);
	}


	void Update()
	{
		if (m_lifespanAfterDetach > 0.0f && !m_isAttached)
		{
			m_lifespanAfterDetach -= Time.deltaTime;

			if (m_lifespanAfterDetach <= 0.0f)
			{
				m_particles.Clear(true);
				m_particles.Stop(true);
				this.Recycle();
			}
		}
	}
};
