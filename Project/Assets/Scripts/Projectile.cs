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
	public AnimationCurve m_positionCurveX;
	public AnimationCurve m_positionCurveY;
	public AnimationCurve m_positionCurveZ;

	protected ParticleTrail m_trail;
	protected float m_speed = 10.0f;
	protected float m_timeAccum;
	protected Vector3 m_target;
	protected Vector3 m_start;

// Member Methods


	public void Shoot(Vector3 _start, Vector3 _destination)
	{
		transform.position = _start;
		m_target = _destination;
		m_target.y += 1.0f;
		m_start = _start;

		m_trail = ObjectPool.Spawn(m_particleTrailPrefab.gameObject, _start).GetComponent<ParticleTrail>();
		m_trail.Initialise(transform, 0.5f);

		m_timeAccum = 0.0f;
	}

	public void Explode(Vector3 _position)
	{
		Ray ray = new Ray(_position, Vector3.down);
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, 100.0f, 1 << LayerMask.NameToLayer("Ground")))
		{
			_position = hitInfo.point;
			_position.y += 0.1f;
		}

		var explosion = ObjectPool.Spawn(m_explosionPrefab.gameObject, _position);
		explosion.GetComponent<Explosion>().Explode();

		m_trail.Detach();
		m_trail = null;

		ObjectPool.Recycle(gameObject);
	}


	void Awake()
	{
		gameObject.SetActive(false);
	}


	void Update()
	{
		m_timeAccum += Time.deltaTime;
		if (m_timeAccum > 1.0f)
		{
			m_timeAccum = 1.0f;
		}

		Vector3 direction = (m_target - m_start);

		Vector3 position = m_start + (direction * m_timeAccum);

		if (m_positionCurveX != null)
			position.x = m_start.x + (direction.x * m_positionCurveX.Evaluate(m_timeAccum));

		if (m_positionCurveY != null)
			position.y = m_start.y + (direction.y * m_positionCurveY.Evaluate(m_timeAccum));

		if (m_positionCurveZ != null)
			position.z = m_start.z + (direction.z * m_positionCurveZ.Evaluate(m_timeAccum));

		transform.position = position;

		if (m_timeAccum == 1.0f)
			Explode(transform.position);
	}
};
