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

	List<Projectile> m_projectiles;

	const int k_maxProjectiles = 20;


	// Member Methods


	void Start()
	{
		// Listen to player input
		Player.Instance.Controller.EventOnShoot += Shoot;


		// Create a bunch of projectiles
		var projectiles = new GameObject("Projectiles");
		var projectilesTransform = projectiles.transform;

		m_projectiles = new List<Projectile>(k_maxProjectiles);

		for (int i = 0; i < k_maxProjectiles; ++i)
		{
			// Create offscreen
			var newProjectile = Instantiate(m_projectilePrefab.gameObject, Vector3.left * 1000.0f, Quaternion.identity) as GameObject;
			newProjectile.transform.parent = projectilesTransform;

			m_projectiles.Add(newProjectile.GetComponent<Projectile>());
		}
	}


	void OnDestroy()
	{
		Player.Instance.Controller.EventOnShoot -= Shoot;
	}


	void Update()
	{
		// Empty
	}


	void Shoot(PlayerController _sender, Vector3 _destination)
	{
		var newProjectile = GetNextInactiveProjectile();

		if (newProjectile == null)
		{
			if(EventNotEnoughAmmo != null)
				EventNotEnoughAmmo(this);

			return;
		}


		Vector3 startPosition = transform.position + (_destination - transform.position).normalized;

		newProjectile.Shoot(startPosition, _destination);
	}


	Projectile GetNextInactiveProjectile()
	{
		foreach(Projectile p in m_projectiles)
		{
			if (!p.gameObject.activeSelf)
				return p;
		}

		return null;
	}

};
