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


    public enum EEnemyType
    {
        INVALID     = -1,
        Grunt,
        Archer,
        Warrior,
        Caster,
        Boss,
        MAX
    }


// Member Delegates & Events


// Member Properties


// Member Fields


    const int k_maxGrunts   = 20;
    const int k_maxArchers  = 10;
    const int k_maxWarriors = 10;
    const int k_maxCasters  = 10;
    const int k_maxBosses   = 4;

    public GameObject m_skeletonGrunt   = null;
    public GameObject m_skeletonArcher  = null;
    public GameObject m_skeletonWarrior = null;
    public GameObject m_skeletonCaster  = null;
    public GameObject m_skeletonBoss    = null;

	float m_spawnTimer      = 0.0f;
	float m_spawnTimerMax   = 3.0f;
	float m_spawnTimerMin   = 1.5f;


// Member Methods


	void Start()
	{
        ObjectPool.CreatePool(m_skeletonGrunt, k_maxGrunts);
        ObjectPool.CreatePool(m_skeletonArcher, k_maxArchers);
        ObjectPool.CreatePool(m_skeletonWarrior, k_maxWarriors);
        ObjectPool.CreatePool(m_skeletonCaster, k_maxCasters);
        ObjectPool.CreatePool(m_skeletonBoss, k_maxBosses);

		RandomiseTimer();

		Game.Instance.EventRestart += OnRestart;
	}


	void OnRestart(Game _sender)
	{
		RandomiseTimer();
	}


	void OnDestroy()
	{
		Game.Instance.EventRestart -= OnRestart;
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

		//var enemy = ObjectPool.Spawn(m_enemyPrefab.gameObject, startPosition);

        int enemyType = Random.Range(0, (int)EEnemyType.MAX);

        GameObject enemy = null;

        switch ((EEnemyType)enemyType)
        {
            case EEnemyType.Grunt:
                enemy = ObjectPool.Spawn(m_skeletonGrunt, startPosition);
                break;

            case EEnemyType.Archer:
                enemy = ObjectPool.Spawn(m_skeletonArcher, startPosition);
                break;

            case EEnemyType.Warrior:
                enemy = ObjectPool.Spawn(m_skeletonWarrior, startPosition);
                break;

            case EEnemyType.Caster:
                enemy = ObjectPool.Spawn(m_skeletonCaster, startPosition);
                break;

            case EEnemyType.Boss:
                enemy = ObjectPool.Spawn(m_skeletonBoss, startPosition);
                break;
        }

        enemy.transform.rotation = Quaternion.LookRotation(Vector3.left);


        /* 
		var enemy = 

		if (enemy != null)
		{
			var target = GameObject.FindGameObjectWithTag("Target");
			enemy.transform.LookAt(target.transform);

			enemy.gameObject.SetActive(true);

			enemy.GetComponent<EnemyBehaviour>().Spawn();
		}
         * */

		RandomiseTimer();
	}


	void RandomiseTimer()
	{
		m_spawnTimer = Random.Range(m_spawnTimerMin, m_spawnTimerMax);
	}


};
