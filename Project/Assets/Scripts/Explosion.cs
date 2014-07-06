//  
//  File Name   :   Explosion.cs
//


// Namespaces
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Explosion : MonoBehaviour
{

	// Member Types


	// Member Delegates & Events


	// Member Properties


	// Member Fields

	private float m_lifeSpan = 2.0f;
	private float m_timeElapsed = 0.0f;


	// Member Methods

	public void Explode()
	{
		m_timeElapsed = m_lifeSpan;

	}


	void Awake()
	{
		gameObject.SetActive(false);
	}


	void Update()
	{
		m_timeElapsed -= Time.deltaTime;

		if(m_timeElapsed < 0.0f)
		{
			this.Recycle();
		}
	}


};
