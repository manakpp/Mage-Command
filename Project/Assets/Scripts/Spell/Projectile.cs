//  
//  File Name   :   Projectile.cs
//


// Namespaces
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Projectile : MonoBehaviour
{
// Member Properties


// Member Fields

	public Projectile m_projectile;
	public Projectile m_explosionPrefab;
	public AudioClip m_castSound;
	public AudioClip m_travelSound;
	public float m_lifespan = 1.5f;

	protected ParticleSystem m_particles;
	protected Vector3 m_target;
	protected Vector3 m_start;
	protected float m_timeAccum;
	protected float m_childLifespan;
	protected bool m_reachedTarget;

// Member Methods


	/// <summary>
	/// Initialisation of the projectile
	/// </summary>
	/// <param name="_start"></param>
	/// <param name="_destination"></param>
	public virtual void Shoot(Vector3 _start, Vector3 _destination)
	{
		transform.position = _start;
		m_target = _destination;
		m_start = _start;
		
		// Reset timer for projectile lifetime
		m_timeAccum = 0.0f;

		// Enable the particles particles
		if(m_particles != null)
			m_particles.Play(true);

		m_reachedTarget = false;
	}


	public virtual void Explode(Vector3 _position)
	{
		if (m_explosionPrefab)
		{
			var explosion = ObjectPool.Spawn(m_explosionPrefab.gameObject, _position);
			explosion.GetComponent<Explosion>().Explode();
		}

		if (m_particles != null)
		{
			m_particles.Clear(true);
			m_particles.Stop(true);
		}

		m_reachedTarget = true;
	}


	protected virtual void SubUpdate()
	{
		// To be overidden
	}


	protected virtual void Awake()
	{
		m_particles = GetComponentInChildren<ParticleSystem>();
		gameObject.SetActive(false);

		if(m_explosionPrefab == null)
			return;

		var projectile = m_explosionPrefab.GetComponent<Projectile>();

		m_childLifespan = m_lifespan;

		while (projectile != null)
		{
			m_childLifespan += projectile.m_lifespan;

			if(projectile.m_explosionPrefab == null)
				break;

			projectile = projectile.m_explosionPrefab.GetComponent<Projectile>();
		}
	}


	void Update()
	{
		if (m_reachedTarget)
		{
			m_timeAccum += Time.deltaTime;
			if (m_timeAccum >= m_childLifespan)
			{
				ObjectPool.Recycle(gameObject);
			}
			return;
		}

		m_timeAccum += Time.deltaTime;
		if (m_timeAccum > m_lifespan)
		{
			m_timeAccum = m_lifespan;
		}

		SubUpdate();
		
		if (m_timeAccum == m_lifespan)
			Explode(transform.position);
	}
};
