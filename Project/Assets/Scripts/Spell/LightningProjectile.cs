//  
//  File Name   :   LightningProjectile.cs
//


// Namespaces
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class LightningProjectile : Projectile
{
	// Member Methods

	public override void Shoot(Vector3 _start, Vector3 _destination)
	{
		_start.y = 0.5f;
		_destination.y = 0.5f;

		base.Shoot(_start, _destination);
	}


	protected override void SubUpdate()
	{
		Vector3 newPos = Vector3.Lerp(m_start, m_target, m_timeAccum);
		transform.position = newPos;
	}
};
