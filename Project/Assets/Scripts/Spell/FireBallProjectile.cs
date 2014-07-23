//  
//  File Name   :   FireBall.cs
//


// Namespaces
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class FireBallProjectile : Projectile
{
// Member Fields

	protected Vector3 m_initialVelocity;
	protected float m_baseDuration = 2.5f;

	protected const float k_projectileGravity = 15.8f;


// Member Methods

	public override void Shoot(Vector3 _start, Vector3 _destination)
	{
		base.Shoot(_start, _destination);

		// Set the path to follow
		// Reference :http://gamedev.stackexchange.com/questions/17467/calculating-velocity-needed-to-hit-target-in-parabolic-arc

		// The arc and duration is dependent of the distance.
		// ie close tiles spawn faster projectiles.
		float distance = (_destination - _start).sqrMagnitude;
		const float minDistance = 100.0f;
		const float distanceToFurtherestTile = 300.0f;
		m_lifespan = m_baseDuration * Mathf.Clamp((distance + minDistance) / distanceToFurtherestTile, 0.25f, m_baseDuration);

		// Initial velocity can later be used to calculate the position of the projectile 
		m_initialVelocity = CalculateInitialVelocity(m_start, m_target, m_lifespan);
	}


	protected override void SubUpdate()
	{
		Vector3 newPos = ParabolaEvaluate(m_timeAccum);
		transform.position = newPos;
	}


	Vector3 CalculateInitialVelocity(Vector3 _start, Vector3 _end, float _duration)
	{
		Vector3 initialVelocity;

		initialVelocity.x = (m_target.x - _start.x) / _duration;
		initialVelocity.y = (m_target.y + 0.5f * k_projectileGravity * _duration * _duration - _start.y) / _duration;
		initialVelocity.z = (m_target.z - _start.z) / _duration;

		return initialVelocity;
	}


	Vector3 ParabolaEvaluate(float _t)
	{
		Vector3 position;
		position.x = m_initialVelocity.x * _t + m_start.x;
		position.y = -0.5f * k_projectileGravity * _t * _t + m_initialVelocity.y * _t + m_start.y;
		position.z = m_initialVelocity.z * _t + m_start.z;

		return position;
	}
};
