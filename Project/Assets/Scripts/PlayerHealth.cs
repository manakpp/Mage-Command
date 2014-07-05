//  
//  File Name   :   PlayerHealth.cs
//


// Namespaces
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PlayerHealth : MonoBehaviour
{

	// Member Types


	// Member Delegates & Events

	public delegate void OnHealthChangeHandler(PlayerHealth _sender, int _oldHealthValue);
	public event OnHealthChangeHandler EventOnHealthChange;

	public delegate void OnHealthEqualToZero(PlayerHealth _sender);
	public event OnHealthEqualToZero EventOnHealthEqualToZero;


	// Member Properties

	public int Value
	{
		get { return m_health; }
		set 
		{
			if(m_health != value)
				return;

			int oldHealth = m_health;

			m_health = value;
			m_health = Mathf.Clamp(m_health, 0, m_maxHealth);

			if (EventOnHealthChange != null)
				EventOnHealthChange.Invoke(this, oldHealth);

			if(m_health == 0)
			{
				if (EventOnHealthEqualToZero != null)
					EventOnHealthEqualToZero.Invoke(this);
			}
		}
	}


	// Member Fields

	public int m_maxHealth = 3;

	int m_health;


	// Member Methods

	void Awake()
	{
		m_health = m_maxHealth;
	}


	void Start()
	{
		// Empty
	}


	void OnDestroy()
	{
		// Empty
	}


	void Update()
	{
		// Empty
	}


};
