//  
//  File Name   :   SpellLibrary.cs
//


// Namespaces
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public sealed class SpellLibrary : MonoBehaviour
{

	// Member Types


	// Member Delegates & Events


	// Member Properties

	public static SpellLibrary Instance
	{
		get
		{
			if (s_instance != null)
			{
				s_instance = Game.Instance.GetComponent<SpellLibrary>();
			}

			return s_instance;
		}
	}


	public List<Spell> AllSpells
	{
		get { return m_spells; }
	}


	// Member Fields

	public List<Spell> m_spells;

	static SpellLibrary s_instance;


	// Member Methods

	public Spell GetSpell(int _spellNameHash)
	{
		return m_spells[_spellNameHash];
	}


	public Spell GetSpell(string _spellName)
	{
		var spell = m_spells.Find(x => x.name == _spellName);

		if (spell == null)
		{
			Debug.LogError("Couldn't find spell: " + _spellName + " in the SpellLibrary.");
		}

		return spell;
	}


	public List<Spell> GetSpellsOfBindingType(SpellBook.SpellInputBinding _bindings, bool _ordered = false)
	{
		// Find any type of spells by using bitwise operations
		// eg myBinds = Tap | Hold | etc

		List<Spell> newList = new List<Spell>();

		foreach(var spell in m_spells)
		{
			if ((spell.m_spellInputBind & _bindings) == spell.m_spellInputBind)
				newList.Add(spell);
		}

		if (_ordered)
		{
			// This is sorted the enum value
			newList.Sort((a, b) => a.m_spellInputBind.CompareTo(b.m_spellInputBind));
		}


		return newList;
	}


	void Awake()
	{
		s_instance = this;
	}
};
