//  
//  File Name   :   LightningStrikeSpell.cs
//	Description	:	Lightning Strike will create a bolt of lightning directly down on a targetted tile.
//


// Namespaces
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class LightningStrikeSpell : Spell
{

	// Member Types


	// Member Delegates & Events


	// Member Properties


	// Member Fields


	// Member Methods


	public override void TapCast(Mage _mage, Tile _targetTile)
	{
		Vector3 destination = _targetTile.transform.position;

		var newProjectileObject = ObjectPool.Spawn(m_projectilePrefab.gameObject, destination);
		if (newProjectileObject == null)
			return;

		var projectile = newProjectileObject.GetComponent<Projectile>();
		projectile.Shoot(destination, destination);

		if (projectile.m_castSound != null)
			AudioSource.PlayClipAtPoint(projectile.m_castSound, Vector3.zero);
	}
};
