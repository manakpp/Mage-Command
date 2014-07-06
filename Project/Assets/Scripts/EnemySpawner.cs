﻿//  
//  File Name   :   EnemySpawner.cs
//


// Namespaces
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class EnemySpawner : MonoBehaviour
{

	// Member Types


	// Member Delegates & Events


	// Member Properties


	// Member Fields

	public Enemy m_enemyPrefab;

	const int k_maxEnemies = 20;

	float m_spawnTimer;
	float m_spawnTimerMax = 3.0f;
	float m_spawnTimerMin = 1.5f;


	// Member Methods


	void Start()
	{
		// Create a bunch of Explosions
		ObjectPool.CreatePool(m_enemyPrefab, k_maxEnemies);

		RandomiseTimer();

		Game.Instance.EventRestart += OnRestart;
	}


	void OnRestart(Game _sender)
	{
		DestroyAllActiveEnemies();
		RandomiseTimer();
	}


	void OnDestroy()
	{
		Game.Instance.EventRestart -= OnRestart;
	}


	void DestroyAllActiveEnemies()
	{
		
	}


	void Update()
	{
		if (m_spawnTimer > 0.0f)
		{
			m_spawnTimer -= Time.deltaTime;

			if (m_spawnTimer <= 0.0f)
			{
				SpawnEnemy();
				return;
			}
		}
	}


	void SpawnEnemy()
	{
		Vector3 extents = collider.bounds.extents;
		Vector3 position = collider.bounds.center;

		Vector3 startPosition = Vector3.zero;
		startPosition.x = Random.Range(position.x - extents.x / 2.0f, position.x + extents.x / 2.0f);
		startPosition.z = Random.Range(position.z - extents.z / 2.0f, position.z + extents.z / 2.0f);

		var enemy = m_enemyPrefab.Spawn(startPosition);

		if (enemy != null)
		{
			var target = GameObject.FindGameObjectWithTag("Target");
			enemy.transform.LookAt(target.transform);

			enemy.gameObject.SetActive(true);

			enemy.Spawn();
		}

		RandomiseTimer();
	}


	void RandomiseTimer()
	{
		m_spawnTimer = Random.Range(m_spawnTimerMin, m_spawnTimerMax);
	}

};
