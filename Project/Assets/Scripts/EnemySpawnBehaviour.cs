//  
//  File Name   :   EnemySpawner.cs
//


// Namespaces
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class EnemySpawnBehaviour : MonoBehaviour
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


    const int k_maxGrunts                   = 20;
    const int k_maxArchers                  = 10;
    const int k_maxWarriors                 = 10;
    const int k_maxCasters                  = 10;
    const int k_maxBosses                   = 4;

    public GameObject m_skeletonGrunt       = null;
    public GameObject m_skeletonArcher      = null;
    public GameObject m_skeletonWarrior     = null;
    public GameObject m_skeletonCaster      = null;
    public GameObject m_skeletonBoss        = null;

    public GameObject[] m_spawnPositions    = null;

	float m_spawnTimer                      = 0.0f;
	float m_spawnTimerMax                   = 3.0f;
	float m_spawnTimerMin                   = 1.5f;

    bool m_spawnEnabled                     = false;


// Member Methods


	void Start()
	{
        // Create object pools for enemy types
        ObjectPool.CreatePool(m_skeletonGrunt,   k_maxGrunts);
        ObjectPool.CreatePool(m_skeletonArcher,  k_maxArchers);
        ObjectPool.CreatePool(m_skeletonWarrior, k_maxWarriors);
        ObjectPool.CreatePool(m_skeletonCaster,  k_maxCasters);
        ObjectPool.CreatePool(m_skeletonBoss,    k_maxBosses);

        // Signup to game events
		Game.Instance.EventRestart += OnEventGameRestart;
        Game.Instance.EventStart   += OnEventGameStart;
	}


	void OnDestroy()
	{
        // Unsubscribe from game events
        Game.Instance.EventRestart -= OnEventGameRestart;
        Game.Instance.EventStart   -= OnEventGameStart;
	}


	void Update()
	{
        UpdateSpawning();
	}


    void UpdateSpawning()
    {
        if (!m_spawnEnabled)
            return;

        m_spawnTimer -= Time.deltaTime;

        // Spawn enemy
        if (m_spawnTimer <= 0.0f)
        {
            SpawnEnemy();

            // Randomise next spawn time
            RandomiseSpawnTimer();
        }
    }


	void SpawnEnemy()
	{
        // Randomise spawn location
        int spawnLocationIndex = Random.Range(0, m_spawnPositions.Length);

        Vector3 spawnLocation = m_spawnPositions[spawnLocationIndex].transform.position;

        // Create random enemy
        EEnemyType enemyType = (EEnemyType)Random.Range(0, (int)EEnemyType.MAX);

        GameObject enemy = null;

        switch ((EEnemyType)enemyType)
        {
            case EEnemyType.Grunt:
                enemy = ObjectPool.Spawn(m_skeletonGrunt, spawnLocation);
                break;

            case EEnemyType.Archer:
                enemy = ObjectPool.Spawn(m_skeletonArcher, spawnLocation);
                break;

            case EEnemyType.Warrior:
                enemy = ObjectPool.Spawn(m_skeletonWarrior, spawnLocation);
                break;

            case EEnemyType.Caster:
                enemy = ObjectPool.Spawn(m_skeletonCaster, spawnLocation);
                break;

            case EEnemyType.Boss:
                enemy = ObjectPool.Spawn(m_skeletonBoss, spawnLocation);
                break;
        }

        enemy.transform.rotation = Quaternion.LookRotation(Vector3.left);
	}


	void RandomiseSpawnTimer()
	{
		m_spawnTimer = Random.Range(m_spawnTimerMin, m_spawnTimerMax);
	}


    void OnEventGameRestart(Game _sender)
    {
        m_spawnEnabled = false;
    }


    void OnEventGameStart(Game _sender)
    {
        RandomiseSpawnTimer();

        m_spawnEnabled = true;
    }


};
