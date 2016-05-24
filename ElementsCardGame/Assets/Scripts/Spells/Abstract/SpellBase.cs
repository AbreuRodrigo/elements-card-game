using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class SpellBase {

	protected Dictionary<SpellSelection, SpellEffect> spellEffectBySelection;

	public delegate void SpellEffect(Player target, Player source);

	public void CastSpell (SpellSelection selection, Player target, Player source) {
		if (spellEffectBySelection != null) {
			spellEffectBySelection [selection] (target, source);
		}
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
