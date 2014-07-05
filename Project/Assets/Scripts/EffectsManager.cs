//  
//  File Name   :   EffectsManager.cs
//


// Namespaces
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class EffectsManager : Singleton<EffectsManager>
{

	// Member Types


	// Member Delegates & Events


	// Member Properties


	// Member Fields

	const int k_maxExplosions = 20;

	public Explosion m_explosionPrefab;

	List<Explosion> m_explosions;

	Dictionary<System.Type, List<MonoBehaviour>> m_effects; 

	static EffectsManager s_instance;


	// Member Methods

	public Explosion CreateExplosion(Vector3 _startPosition)
	{
		foreach (Explosion obj in m_explosions)
		{
			if (!obj.gameObject.activeSelf)
			{
				obj.transform.position = _startPosition;
				obj.Explode();

				return obj.GetComponent<Explosion>();
			}
		}

		return null;
	}


	void Start()
	{
		// Create a bunch of Explosions
		var projectiles = new GameObject("Explosions");
		var projectilesTransform = projectiles.transform;

		m_explosions = new List<Explosion>(k_maxExplosions);

		for (int i = 0; i < k_maxExplosions; ++i)
		{
			// Create offscreen
			var newProjectile = Instantiate(m_explosionPrefab.gameObject, Vector3.left * 1000.0f, Quaternion.identity) as GameObject;
			newProjectile.transform.parent = projectilesTransform;

			m_explosions.Add(newProjectile.GetComponent<Explosion>());
		}
	}


};
