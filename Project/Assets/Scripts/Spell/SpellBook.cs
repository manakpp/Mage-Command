//  
//  File Name   :   SpellBook.cs
//


// Namespaces
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SpellBook : MonoBehaviour
{

// Member Types

	[System.Flags]
	public enum SpellInputBinding
	{
		Tap = 1,
		Hold = 2,
		HoldRelease = 3,
		SwipeLeft = 4,
		SwipeRight = 5,
		SwipeUp = 6,
		SwipeDown = 7,
	}



// Member Delegates & Events


// Member Properties


// Member Fields

	const int k_maxSpells = 6;

	Dictionary<SpellInputBinding, Spell> m_spellLookup;


// Member Methods

	public void BindSpell(SpellInputBinding _binding, Spell _spell)
	{
		// Ensure binds match
		if (_binding != _spell.m_spellInputBind)
		{
			Debug.LogError(string.Format("You attempted to {0} which is a {1} typ to {2}.", _spell.name, _spell.m_spellInputBind, _binding));
			return;
		}

		m_spellLookup[_binding] = _spell;
	}


	void InitialiseBindedSpells()
	{
		// Create a bunch of projectiles
		foreach(var spellPair in m_spellLookup)
		{
			spellPair.Value.Initialise();
		}
	}


	public void LoadSpells()
	{
		// TODO: From save data we can populate the spell book.
		m_spellLookup = new Dictionary<SpellInputBinding, Spell>();

		// These ones are working
		BindSpell(SpellInputBinding.Tap, SpellLibrary.Instance.GetSpell("FireBallSpell"));
		//BindSpell(SpellInputBinding.HoldRelease, SpellLibrary.Instance.GetSpell("IceBlockSpell"));
		//BindSpell(SpellInputBinding.Tap, SpellLibrary.Instance.GetSpell("LightningStrikeSpell"));
		BindSpell(SpellInputBinding.SwipeRight, SpellLibrary.Instance.GetSpell("LightningSpell"));

		// These ones are WIP
		//BindSpell(SpellInputBinding.SwipeUp, SpellLibrary.Instance.GetSpell("FireBallSpell"));
		//BindSpell(SpellInputBinding.SwipeDown, SpellLibrary.Instance.GetSpell("FireBallSpell"));
		//BindSpell(SpellInputBinding.SwipeLeft, SpellLibrary.Instance.GetSpell("FireBallSpell"));
		//BindSpell(SpellInputBinding.SwipeRight, SpellLibrary.Instance.GetSpell("FireBallSpell"));

		InitialiseBindedSpells();
	}


	public Spell GetSpellBindedTo(SpellBook.SpellInputBinding _inputBinding)
	{
		if(!m_spellLookup.ContainsKey(_inputBinding))
			return null;
		
		return m_spellLookup[_inputBinding];
	}

};
