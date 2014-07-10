//  
//  File Name   :   Explosion.cs
//


// Namespaces
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Explosion : Projectile
{

	// Member Types


	// Member Delegates & Events


	// Member Properties


	// Member Fields

	public AudioClip m_explosionSound;
	

	// Member Methods

	public virtual void Explode()
	{
		base.Shoot(transform.position, transform.position);

		if(m_explosionSound != null)
			AudioSource.PlayClipAtPoint(m_explosionSound, transform.position);
	}


	protected override void SubUpdate()
	{
		// To be derived
	}


	protected override void Awake()
	{
		base.Awake();
	}


	void OnTriggerEnter(Collider _col)
	{
		// I want to handle it here incase I want to spawn particles :P
		if (LayerMask.LayerToName(_col.gameObject.layer) == "Enemy")
		{
			
		}
	}


};
