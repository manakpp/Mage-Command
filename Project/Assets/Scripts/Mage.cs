//  
//  File Name   :   Mage.cs
//


// Namespaces
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Mage : MonoBehaviour
{

	// Member Types


	// Member Delegates & Events

	public delegate void OnNotEnoughAmmoHandler(Mage _sender);
	public event OnNotEnoughAmmoHandler EventNotEnoughAmmo;


	// Member Properties


	// Member Fields


	public Projectile m_projectilePrefab;

	const int k_maxProjectiles = 20;


	// Member Methods


	void Awake()
	{
		// Create a bunch of projectiles
		ObjectPool.CreatePool(m_projectilePrefab, k_maxProjectiles);
		ObjectPool.CreatePool(m_projectilePrefab.m_explosionPrefab, k_maxProjectiles);
	}


	void Start()
	{
		// Listen to player input
		Player.Instance.Controller.EventOnShoot += Shoot;	
	}

	void OnDisable()
	{
	}


	void OnDestroy()
	{
		
	}


	void Update()
	{
		// Empty
	}


	void Shoot(PlayerController _sender, Vector3 _destination)
	{
		Vector3 startPosition = transform.position + (_destination - transform.position).normalized;

		var newProjectile = m_projectilePrefab.Spawn(startPosition);

		if (newProjectile == null)
		{
			if (EventNotEnoughAmmo != null)
				EventNotEnoughAmmo(this);

			return;
		}
		
		newProjectile.Shoot(startPosition, _destination);
	}


};
