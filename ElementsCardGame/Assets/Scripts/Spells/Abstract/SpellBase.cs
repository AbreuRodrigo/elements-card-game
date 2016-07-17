using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class SpellBase {
	protected int damage;

	protected int extraDamage;
	public int ExtraDamage {
		get { return extraDamage; }
	}

	protected bool alwaysGoesFirst;
	public bool AlwaysGoesFirst {
		get { return alwaysGoesFirst; }
	}

	protected bool extraDamageInDice;
	public bool ExtraDamageInDice {
		get { return extraDamageInDice; }
	}

	protected SpellResponse response = new SpellResponse();

	protected Dictionary<SpellSelection, SpellEffect> spellEffectBySelection;

	public delegate SpellResponse SpellEffect(Player target, Player source);

	public SpellResponse CastSpell (SpellSelection selection, Player target, Player source) {
		extraDamage = 0;
		alwaysGoesFirst = false;
		extraDamageInDice = false;

		if (spellEffectBySelection != null) {
			response = spellEffectBySelection [selection] (target, source);
		}

		return response;
	}

	protected void CauseDamage(int amount, Player target) {
		target.DecreaseHP (amount);
	}

	protected void HealDamage(int amount, Player target) {
		target.IncreaseHP (amount);		
	}

	private void CauseDebuff() {
	}
}