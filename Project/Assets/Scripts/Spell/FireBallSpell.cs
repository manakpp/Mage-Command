//  
//  File Name   :   FireBallSpell.cs
//


// Namespaces
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class FireBallSpell : Spell
{

// Member Types


// Member Delegates & Events


// Member Properties


// Member Fields


// Member Methods

	public override void TapCast(Mage _mage, Tile _targetTile)
	{
		Shoot(_mage, _targetTile);
	}


	public override void SwipeCast(Mage _mage, Tile[] _targetTiles, float _swipeVelocity)
	{
		float duration = 0.5f;
		Game.Instance.StartCoroutine(ShootOverTime(_mage, _targetTiles, duration));
	}


	void Shoot(Mage _mage, Tile _targetTile)
	{
		Vector3 startPosition = _mage.transform.position + _mage.transform.forward + Vector3.up;
		Vector3 destination = _targetTile.transform.position;
		destination.y += 0.5f;


		var newProjectile = ObjectPool.Spawn(m_projectilePrefab.gameObject, startPosition);
		if (newProjectile == null)
			return;

		var ball = newProjectile.GetComponent<Projectile>();

		ball.Shoot(startPosition, destination);

		if (ball.m_castSound != null)
			AudioSource.PlayClipAtPoint(ball.m_castSound, Vector3.zero);
	}


	IEnumerator ShootOverTime(Mage _mage, Tile[] _targetTiles, float _duration)
	{
		float waitTime = _duration / (float)_targetTiles.Length;
		for (int i = 0; i < _targetTiles.Length; ++i)
		{
			Shoot(_mage, _targetTiles[i]);

			yield return new WaitForSeconds(0.1f);
		}
	}

};
