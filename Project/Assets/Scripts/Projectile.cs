//  
//  File Name   :   Projectile.cs
//


// Namespaces
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Projectile : MonoBehaviour
{

	// Member Types


	// Member Delegates & Events


	// Member Properties

	public Explosion m_explosionPrefab;

	private Vector3 m_velocity;
	private Vector3 m_direction;
	private float m_speed = 10.0f;
	private bool m_initialised;


	// Member Fields


	// Member Methods


	public void Shoot(Vector3 _start, Vector3 _destination)
	{
		transform.position = _start;
		m_direction = (_destination - _start).normalized;
	}

	public void Explode(Vector3 _position)
	{
		this.Recycle();
		var explosion = m_explosionPrefab.Spawn(_position);
		explosion.Explode();
	}


	void Awake()
	{
		gameObject.SetActive(false);
		m_initialised = true;
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
		if (!m_initialised)
			return;

		Explode(transform.position);
	}
};
