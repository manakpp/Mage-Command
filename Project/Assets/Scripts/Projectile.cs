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
	public Explosion m_explosionPrefab;
	public ParticleTrail m_particleTrailPrefab;

	protected Vector3 m_velocity;
	protected Vector3 m_direction;
	protected ParticleTrail m_trail;
	protected float m_speed = 10.0f;
	protected bool m_initialised;


// Member Methods


	public void Shoot(Vector3 _start, Vector3 _destination)
	{
		transform.position = _start;
		m_direction = (_destination - _start).normalized;

		m_trail = ObjectPool.Spawn(m_particleTrailPrefab.gameObject, _start).GetComponent<ParticleTrail>();
		m_trail.Initialise(transform, 0.5f);
	}

	public void Explode(Vector3 _position)
	{
		Ray ray = new Ray(_position, Vector3.down);
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, 100.0f, 1 << LayerMask.NameToLayer("Ground")))
		{
			_position = hitInfo.point;
			_position.y += 0.1f;
		}

		var explosion = ObjectPool.Spawn(m_explosionPrefab.gameObject, _position);
		explosion.GetComponent<Explosion>().Explode();

		m_trail.Detach();
		m_trail = null;

		ObjectPool.Recycle(gameObject);
	}


	void Awake()
	{
		gameObject.SetActive(false);
	}


	void OnDestroy()
	{
		// Empty
	}


	void Update()
	{
		transform.position += m_direction * m_speed * Time.deltaTime;
	}


	void OnCollisionEnter(Collision _collision)
	{
		if(gameObject.activeSelf)
			Explode(transform.position);
	}
};
