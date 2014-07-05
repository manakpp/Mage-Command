//  
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

	List<Enemy> m_enemies;

	const int k_maxEnemies = 20;

	float m_spawnTimer;
	float m_spawnTimerMax = 3.0f;
	float m_spawnTimerMin = 1.5f;


	// Member Methods


	void Start()
	{
		// Create a bunch of Explosions
		var enemies = new GameObject("Enemies");
		var enemiesTransform = enemies.transform;

		m_enemies = new List<Enemy>(k_maxEnemies);

		for (int i = 0; i < k_maxEnemies; ++i)
		{
			// Create offscreen
			var newProjectile = Instantiate(m_enemyPrefab.gameObject, Vector3.left * 1000.0f, Quaternion.identity) as GameObject;
			newProjectile.transform.parent = enemiesTransform;

			m_enemies.Add(newProjectile.GetComponent<Enemy>());
		}

		RandomiseTimer();
	}


	void OnDestroy()
	{
		// Empty
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
		foreach (Enemy enemy in m_enemies)
		{
			if (!enemy.gameObject.activeSelf)
			{
				Vector3 extents = collider.bounds.extents;
				Vector3 position = collider.bounds.center;

				Vector3 startPosition = Vector3.zero;
				startPosition.x = Random.Range(position.x - extents.x / 2.0f, position.x + extents.x / 2.0f);
				startPosition.z = Random.Range(position.z - extents.z / 2.0f, position.z + extents.z / 2.0f);

				enemy.transform.position = startPosition;

				var target = GameObject.FindGameObjectWithTag("Target");
				enemy.transform.LookAt(target.transform);

				enemy.gameObject.SetActive(true);

				enemy.Spawn();

				break;
			}
		}

		RandomiseTimer();
	}


	void RandomiseTimer()
	{
		m_spawnTimer = Random.Range(m_spawnTimerMin, m_spawnTimerMax);
	}

};
