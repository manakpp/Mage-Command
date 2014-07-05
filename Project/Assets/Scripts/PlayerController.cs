﻿//  
//  File Name   :   PlayerController.cs
//


// Namespaces
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PlayerController : MonoBehaviour
{

	// Member Types


	// Member Delegates & Events
	public delegate void OnShootHandler(PlayerController _sender, Vector3 _destination);
	public event OnShootHandler EventOnShoot;


	// Member Properties

	public LayerMask m_touchLayerMask;


	// Member Fields


	// Member Methods


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
		if (Input.GetMouseButtonUp(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hitInfo;
			if (Physics.Raycast(ray, out hitInfo, 10000.0f, m_touchLayerMask.value))
			{
				if (EventOnShoot != null)
					EventOnShoot(this, hitInfo.point);
			}
		}
	}


};
