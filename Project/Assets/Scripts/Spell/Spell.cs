
//  
//  File Name   :   Spell.cs
//


// Namespaces
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public abstract class Spell : MonoBehaviour
{

// Member Types


// Member Delegates & Events


// Member Properties


// Member Fields

	public string m_iconSprite;
	public SpellBook.SpellInputBinding m_spellInputBind;
	public float m_baseDamage;
	public float m_baseManaCost;
	public GameObject m_projectilePrefab;
	public int m_maxProjectiles = 20;
	public GameObject m_castPrefab;

// Member Methods

	public virtual void Initialise()
	{
		if (m_projectilePrefab == null)
		{
			Debug.LogError("This spell does not have a projectile attached.");
			return;
		}

		Projectile projectile = m_projectilePrefab.GetComponent<Projectile>();
		if (projectile == null)
		{
			Debug.LogError("This spell does not have a projectile attached.");
			return;
		}

		// Create pool for this and all children
		while (projectile != null)
		{
			ObjectPool.CreatePool(projectile.gameObject, m_maxProjectiles);
	
			projectile = projectile.GetComponent<Projectile>();

			if(projectile.m_explosionPrefab == null)
				break;

			projectile = projectile.m_explosionPrefab.GetComponent<Projectile>();
		}
	}


	public virtual void TapCast(Mage _mage, Tile _target)
	{
		// To be derived
	}

	
	public virtual void HoldCast(Mage _mage, Tile _target)
	{
		// To be derived
		TapCast(_mage, _target);
	}


	public virtual void HoldReleaseCast(Mage _mage, Tile _target)
	{
		// To be derived
		TapCast(_mage, _target);
	}


	public virtual void SwipeCast(Mage _mage, Tile[] _targetTiles, float _swipeVelocity)
	{
		// To be derived (This can be used if the direction makes no difference)
	}

	
	public virtual void SwipeUpCast(Mage _mage, Tile[] _targetTiles, float _swipeVelocity)
	{
		// To be derived
		SwipeCast(_mage, _targetTiles, _swipeVelocity);
	}


	public virtual void SwipeDownCast(Mage _mage, Tile[] _targetTiles, float _swipeVelocity)
	{
		// To be derived
		SwipeCast(_mage, _targetTiles, _swipeVelocity);
	}


	public virtual void SwipeLeftCast(Mage _mage, Tile[] _targetTiles, float _swipeVelocity)
	{
		// To be derived
		SwipeCast(_mage, _targetTiles, _swipeVelocity);
	}


	public virtual void SwipeRightCast(Mage _mage, Tile[] _targetTiles, float _swipeVelocity)
	{
		// To be derived
		SwipeCast(_mage, _targetTiles, _swipeVelocity);
	}

};