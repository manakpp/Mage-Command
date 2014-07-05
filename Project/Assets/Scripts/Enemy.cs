//  
//  File Name   :   Enemy.cs
//


// Namespaces
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Enemy : MonoBehaviour
{

	// Member Types


	// Member Delegates & Events


	// Member Properties


	// Member Fields

	private float m_deathTimer;
	private bool m_isDead;
	private bool m_initialised;


	// Member Methods


	public void Spawn()
	{
		m_isDead = false;
		rigidbody.velocity = Vector3.zero;
		rigidbody.angularVelocity = Vector3.zero;
	}


	void Start()
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
		if (m_deathTimer > 0.0f)
		{
			m_deathTimer -= Time.deltaTime;

			if (m_deathTimer < 0.0f)
			{
				gameObject.SetActive(false);
			}

			return;
		}

		transform.position += transform.forward * 10.0f * Time.deltaTime;
	}


	void OnCollisionEnter(Collision _collision)
	{
		if (!m_initialised)
			return;

		if (m_isDead)
			return;

		Die();
	}

	void Die()
	{
		m_isDead = true;
		m_deathTimer = 3.0f;
	}
};
