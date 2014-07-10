//  
//  File Name   :   Mage.cs
//


// Namespaces
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Mage : MonoBehaviour
{

	// Member Delegates & Events

	public delegate void OnNotEnoughManaHandler(Mage _sender);
	public event OnNotEnoughManaHandler EventNotEnoughMana;

	public delegate void OnManaChangedHandler(Mage _sender, float _currentMana, float _maxMana, float _oldMana);
	public event OnManaChangedHandler EventManaChanged;

	public delegate void OnHealthChangedHandler(Mage _sender, int _currentHealth, int _maxHealth, int _oldHealth);
	public event OnHealthChangedHandler EventHealthChanged;


	// Member Properties

	public int Health
	{
		get { return m_health; }
		set
		{
			if (m_health == value)
				return;

			int prevHealth = m_health;

			m_health = value;
			m_health = Mathf.Clamp(m_health, 0, m_maxHealth);

			if (EventHealthChanged != null)
			{
				EventHealthChanged.Invoke(this, m_health, m_maxHealth, prevHealth);
			}
		}
	}

	public float Mana
	{
		get { return m_mana; }
		set
		{
			if (m_mana == value)
				return;

			float prevMana = m_mana;

			m_mana = value;
			m_mana = Mathf.Clamp(m_mana, 0, m_maxMana);

			if (EventManaChanged != null)
			{
				EventManaChanged.Invoke(this, m_mana, m_maxMana, prevMana);
			}
		}
	}


	public SpellBook SpellBook
	{
		get { return m_spellBook; }
	}


	// Member Fields

	public Projectile m_projectilePrefab;
	public int m_maxHealth = 3;
	public float m_maxMana = 20.0f;
	public float m_manaRegenPerSecond = 5.0f;
	public const int k_maxProjectiles = 20;

	SpellBook m_spellBook;
	int m_health;
	float m_mana;


	// Member Methods


	void Awake()
	{
		m_spellBook = GetComponent<SpellBook>();
	}


	void Start()
	{
		// Listen to player input
		Game.Instance.EventRestart += OnRestart;
	}


	void Initialise()
	{
		// Set up stats
		Health = m_maxHealth;
		Mana = m_maxMana;

		m_spellBook.LoadSpells();
	}


	void OnRestart(Game _sender)
	{
		Initialise();
	}


	void OnDestroy()
	{
		Game.Instance.EventRestart -= OnRestart;
	}


	void Update()
	{
		if (Mana < m_maxMana)
		{
			Mana += m_manaRegenPerSecond * Time.deltaTime;
		}
	}


	void CastSpell(Spell _spell, Tile[] _targetTiles, SpellBook.SpellInputBinding _binding, float _swipeVelocity = 0.0f)
	{
		if (_spell == null || _targetTiles == null)
			return;

		if (CheckAndRemoveMana(_spell))
		{
			switch (_binding)
			{
				case SpellBook.SpellInputBinding.SwipeLeft:
					_spell.SwipeLeftCast(this, _targetTiles, _swipeVelocity);
					break;
				case SpellBook.SpellInputBinding.SwipeRight:
					_spell.SwipeRightCast(this, _targetTiles, _swipeVelocity);
					break;
				case SpellBook.SpellInputBinding.SwipeUp:
					_spell.SwipeUpCast(this, _targetTiles, _swipeVelocity);
					break;
				case SpellBook.SpellInputBinding.SwipeDown:
					_spell.SwipeDownCast(this, _targetTiles, _swipeVelocity);
					break;
				default:
					break;
			}
		}
	}


	void CastSpell(Spell _spell, Tile _targetTile, SpellBook.SpellInputBinding _binding)
	{
		if (_spell == null || _targetTile == null)
			return;

		if (CheckAndRemoveMana(_spell))
		{
			switch (_binding)
			{
				case SpellBook.SpellInputBinding.Tap:
					_spell.TapCast(this, _targetTile);
					break;
				case SpellBook.SpellInputBinding.Hold:
					_spell.HoldCast(this, _targetTile);
					break;
				case SpellBook.SpellInputBinding.HoldRelease:
					_spell.HoldReleaseCast(this, _targetTile);
					break;
				default:
					break;
			}
		}
	}


	bool CheckAndRemoveMana(Spell _spell)
	{
		if (Mana >= _spell.m_baseManaCost) // TODO: Check for enough Mana to cast this spell
		{
			Mana -= _spell.m_baseManaCost;
			return true;
		}
		
		if (EventNotEnoughMana != null)
			EventNotEnoughMana.Invoke(this);

		return false;
	}


	public void OnTap(Tile _targetTile)
	{
		var spell = SpellBook.GetSpellBindedTo(SpellBook.SpellInputBinding.Tap);

		CastSpell(spell, _targetTile, SpellBook.SpellInputBinding.Tap);
	}


	public void OnHold(Tile _targetTile)
	{
		var spell = SpellBook.GetSpellBindedTo(SpellBook.SpellInputBinding.Hold);

		CastSpell(spell, _targetTile, SpellBook.SpellInputBinding.Hold);
	}


	public void OnHoldEnded(Tile _targetTile)
	{
		var spell = SpellBook.GetSpellBindedTo(SpellBook.SpellInputBinding.HoldRelease);
		
		CastSpell(spell, _targetTile, SpellBook.SpellInputBinding.HoldRelease);
	}


	public void OnSwipeLeft(Tile[] _targetTiles, float _swipeVelocity)
	{
		var spell = SpellBook.GetSpellBindedTo(SpellBook.SpellInputBinding.SwipeLeft);
		CastSpell(spell, _targetTiles, SpellBook.SpellInputBinding.SwipeLeft, _swipeVelocity);
	}


	public void OnSwipeRight(Tile[] _targetTiles, float _swipeVelocity)
	{
		var spell = SpellBook.GetSpellBindedTo(SpellBook.SpellInputBinding.SwipeRight);
		CastSpell(spell, _targetTiles, SpellBook.SpellInputBinding.SwipeRight, _swipeVelocity);
	}


	public void OnSwipeUp(Tile[] _targetTiles, float _swipeVelocity)
	{
		var spell = SpellBook.GetSpellBindedTo(SpellBook.SpellInputBinding.SwipeUp);
		CastSpell(spell, _targetTiles, SpellBook.SpellInputBinding.SwipeUp, _swipeVelocity);
	}


	public void OnSwipeDown(Tile[] _targetTiles, float _swipeVelocity)
	{
		var spell = SpellBook.GetSpellBindedTo(SpellBook.SpellInputBinding.SwipeDown);
		CastSpell(spell, _targetTiles, SpellBook.SpellInputBinding.SwipeDown, _swipeVelocity);
	}


	void CastFireBall(Vector3 _destination)
	{
		Vector3 startPosition = transform.position + transform.forward + Vector3.up;

		var newProjectile = ObjectPool.Spawn(m_projectilePrefab.gameObject, startPosition);

		if (newProjectile == null)
			return;

		var ball = newProjectile.GetComponent<FireBallProjectile>();
		_destination.y += 0.5f;
		ball.Shoot(startPosition, _destination);

		if (ball.m_castSound != null)
			AudioSource.PlayClipAtPoint(ball.m_castSound, Vector3.zero);
	}


};
