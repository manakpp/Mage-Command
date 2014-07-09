//  
//  File Name   :   Projectile.cs
//


// Namespaces
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Projectile : MonoBehaviour
{
// Member Properties


// Member Fields

	public Projectile m_projectile;
	public Explosion m_explosionPrefab;
	public ParticleTrail m_particleTrailPrefab;
	public AudioClip m_castSound;

	protected ParticleTrail m_trail;
	protected float m_speed = 10.0f;
	protected float m_timeAccum;
	protected Vector3 m_target;
	protected Vector3 m_start;

	protected Vector3 m_initialVelocity;
	protected float m_baseDuration = 2.5f;
	protected float m_duration = 1.5f;

	protected const float k_projectileGravity = 15.8f;


// Member Methods


	/// <summary>
	/// Initialisation of the projectile
	/// </summary>
	/// <param name="_start"></param>
	/// <param name="_destination"></param>
	public void Shoot(Vector3 _start, Vector3 _destination)
	{
		transform.position = _start;
		m_target = _destination;
		m_start = _start;
		
		// Reset timer for projectile lifetime
		m_timeAccum = 0.0f;

		// Attach particles
		m_trail = ObjectPool.Spawn(m_particleTrailPrefab.gameObject, _start).GetComponent<ParticleTrail>();

		float lifeSpanAfterParentDeath = 0.5f;
		m_trail.Initialise(transform, lifeSpanAfterParentDeath);

		// Set the path to follow
		// Reference :http://gamedev.stackexchange.com/questions/17467/calculating-velocity-needed-to-hit-target-in-parabolic-arc

		// The arc and duration is dependent of the distance.
		// ie close tiles spawn faster projectiles.
		float distance = (_destination - _start).sqrMagnitude;
		const float minDistance = 100.0f;
		const float distanceToFurtherestTile = 300.0f;
		m_duration = m_baseDuration * Mathf.Clamp((distance + minDistance) / distanceToFurtherestTile, 0.25f, m_baseDuration);

		// Initial velocity can later be used to calculate the position of the projectile 
		m_initialVelocity = CalculateInitialVelocity(m_start, m_target, m_duration);
	}


	void Awake()
	{
		gameObject.SetActive(false);
	}


	void Update()
	{
		m_timeAccum += Time.deltaTime;
		if (m_timeAccum > m_duration)
		{
			m_timeAccum = m_duration;
		}

		Vector3 newPos = ParabolaEvaluate(m_timeAccum);
		transform.position = newPos;

		if (m_timeAccum == m_duration)
			Explode(transform.position);
	}


	public void Explode(Vector3 _position)
	{
		//Ray ray = new Ray(_position, Vector3.down);
		//RaycastHit hitInfo;
		//if (Physics.Raycast(ray, out hitInfo, 100.0f, 1 << LayerMask.NameToLayer("Ground")))
		//{
		//    _position = hitInfo.point;
		//    _position.y += 0.1f;
		//}

		var explosion = ObjectPool.Spawn(m_explosionPrefab.gameObject, _position);
		explosion.GetComponent<Explosion>().Explode();

		m_trail.Detach();
		m_trail = null;

		ObjectPool.Recycle(gameObject);
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
