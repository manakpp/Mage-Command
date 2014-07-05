//  
//  File Name   :   Player.cs
//


// Namespaces
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(PlayerHealth))]
[RequireComponent(typeof(PlayerController))]
public class Player : Singleton<Player>
{

	// Member Types


	// Member Delegates & Events


	// Member Properties

	public PlayerController Controller
	{
		get { return m_controller; }
		private set { m_controller = value; }
	}

	public PlayerHealth Health
	{
		get { return m_health; }
		private set { m_health = value; }
	}


	// Member Fields

	PlayerController m_controller;
	PlayerHealth m_health;


	// Member Methods


	void Awake()
	{
		Controller = GetComponent<PlayerController>();
		Health = GetComponent<PlayerHealth>();
	}


	void Start()
	{
		// Empty
	}



	void Update()
	{
		// Empty
	}


};
