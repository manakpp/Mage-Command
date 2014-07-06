//  
//  File Name   :   Mage.cs
//


// Namespaces
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Mage : MonoBehaviour
{

	// Member Delegates & Events

	public delegate void OnNotEnoughManaHandler(Mage _sender);
	public event OnNotEnoughManaHandler EventNotEnoughMana;

	public delegate void OnManaChangedHandler(Mage _sender, float _currentMana, float _maxMana, float _oldMana);
	public event OnManaChangedHandler EventManaChanged;

	public delegate void OnHealthChangedHandler(Mage _sender, int _currentHealth, int _maxHealth, int _oldHealth);
	public event OnHealthChangedHandler EventHealthChanged;


	// Member Properties

	public int Health
	{
		get { return m_health; }
		set
		{
			if (m_health == value)
				return;

			int prevHealth = m_health;

			m_health = value;
			m_health = Mathf.Clamp(m_health, 0, m_maxHealth);

			if (EventHealthChanged != null)
			{
				EventHealthChanged.Invoke(this, m_health, m_maxHealth, prevHealth);
			}
		}
	}

	public float Mana
	{
		get { return m_mana; }
		set
		{
			if (m_mana == value)
				return;

			float prevMana = m_mana;

			m_mana = value;
			m_mana = Mathf.Clamp(m_mana, 0, m_maxMana);

			if (EventManaChanged != null)
			{
				EventManaChanged.Invoke(this, m_mana, m_maxMana, prevMana);
			}
		}
	}


	// Member Fields

	public Projectile m_projectilePrefab;
	public int m_maxHealth = 3;
	public float m_maxMana = 20.0f;
	public float m_manaRegenPerSecond = 5.0f;
	public const int k_maxProjectiles = 20;
	
	int m_health;
	float m_mana;


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
		Game.Instance.Controller.EventOnShoot += OnShoot;
		Game.Instance.EventRestart += OnRestart;

		Initialise();
	}


	void Initialise()
	{
		// Set up stats
		m_health = m_maxHealth;
		m_mana = m_maxMana;
	}


	void OnRestart(Game _sender)
	{
		Initialise();
	}


	void OnDestroy()
	{
		Game.Instance.Controller.EventOnShoot -= OnShoot;
		Game.Instance.EventRestart -= OnRestart;
	}


	void Update()
	{
		if (Mana < m_maxMana)
		{
			Mana += m_manaRegenPerSecond * Time.deltaTime;
		}
	}


	void OnShoot(MageController _sender, Vector3 _destination)
	{
		// Check for enough Mana
		if (Mana > 0) // TODO: Check for enough Mana to cast this spell
		{
			Shoot(_destination);
			Mana -= 1;
		}
		else
		{
			if (EventNotEnoughMana != null)
				EventNotEnoughMana.Invoke(this);
		}
	}


	void Shoot(Vector3 _destination)
	{
		Vector3 startPosition = transform.position + (_destination - transform.position).normalized;

		var newProjectile = m_projectilePrefab.Spawn(startPosition);
		
		newProjectile.Shoot(startPosition, _destination);
	}


};
