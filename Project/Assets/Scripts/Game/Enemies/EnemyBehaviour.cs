﻿//  
//  File Name   :   EnemyBehaviour.cs
//


// Namespaces
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public abstract class EnemyBehaviour : MonoBehaviour
{

// Member Types


// Member Delegates & Events


// Member Properties


    public abstract float MovementSpeed { get; set; }


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


    protected virtual void Awake()
    {
        
    }


	protected virtual void Start()
    {
        gameObject.SetActive(false);
        m_initialised = true;
    }


	protected virtual void Update()
    {
        if (m_deathTimer > 0.0f)
        {
            m_deathTimer -= Time.deltaTime;

            if (m_deathTimer < 0.0f)
            {
                ObjectPool.Recycle(gameObject);
            }

            return;
        }

        //if (!m_hasReachedTarget)
         //   transform.position += transform.forward * 10.0f * Time.deltaTime;


        UpdateMovement();
    }


	protected virtual void OnDestroy()
	{
		// Empty
	}


    void UpdateMovement()
    {
        transform.position += Vector3.left * MovementSpeed * Time.deltaTime;
    }


	//void OnCollisionEnter(Collision _collision)
	//{
	//    if (!m_initialised)
	//        return;

	//    if (m_isDead)
	//        return;

	//    if(_collision.gameObject.layer == LayerMask.NameToLayer("Explosion"))
	//    {
	//        rigidbody.AddForce((transform.position - _collision.transform.position).normalized * 100.0f);
	//    }

	//    Die();
	//}


	public void Die()
	{
		if (m_isDead)
			return;

		m_isDead = true;
		m_deathTimer = 0.5f;
	}


	void OnTriggerEnter(Collider _col)
	{
		transform.LookAt(transform.position + Vector3.forward);

		Game.Instance.Mage.Health -= 1;

		Die();
	}


};
