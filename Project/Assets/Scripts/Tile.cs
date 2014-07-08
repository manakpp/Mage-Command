//  
//  File Name   :   Tile.cs
//


// Namespaces
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Tile : MonoBehaviour
{

	// Member Types


	// Member Delegates & Events


	// Member Properties


	// Member Fields

	Color m_defaultColor;
	Color m_startColor;
	Material m_material;
	float m_duration;
	float m_timeAccum;
	

	// Member Methods

	public void Highlight(Color _color, float _duration)
	{
		m_startColor = _color;
		m_material.color = _color;
		m_duration = _duration;
		m_timeAccum = 0.0f;	
	}


	public void UnHighlight()
	{
		m_material.color = m_defaultColor;
	}


	void Awake()
	{
		m_material = renderer.material;
		m_defaultColor = m_material.color;
	}


	void Update()
	{
		if (m_timeAccum == m_duration)
			return;

		m_timeAccum += Time.deltaTime;

		if (m_timeAccum > m_duration)
			m_timeAccum = m_duration;

		m_material.color = Color.Lerp(m_startColor, m_defaultColor, m_timeAccum / m_duration);
	}
};
