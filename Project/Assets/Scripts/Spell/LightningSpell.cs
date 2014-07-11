//  
//  File Name   :   LightningSpell.cs
//	Description	:	Lightning straight bolt of lightning in a swiped direction.
//


// Namespaces
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class LightningSpell : Spell
{

	// Member Types


	// Member Delegates & Events


	// Member Properties


	// Member Fields


	// Member Methods


	public override void SwipeCast(Mage _mage, Tile[] _targetTiles, float _swipeVelocity)
	{
		Vector3 startPosition = _targetTiles[0].transform.position;
		Vector3 destination = _targetTiles[_targetTiles.Length - 1].transform.position;

		var newProjectile = ObjectPool.Spawn(m_projectilePrefab.gameObject, startPosition);
		if (newProjectile == null)
			return;

		var ball = newProjectile.GetComponent<Projectile>();
		ball.Shoot(startPosition, destination);

		if (ball.m_castSound != null)
			AudioSource.PlayClipAtPoint(ball.m_castSound, Vector3.zero);
	}
};
