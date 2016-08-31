using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class SpellBase {
	protected int damage;
	protected int heal;

	protected int extraDamage;
	public int ExtraDamage {
		get { return extraDamage; }
	}

	protected bool extraDamageInDice;
	public bool ExtraDamageInDice {
		get { return extraDamageInDice; }
	}

	protected Dictionary<SpellSelection, SpellEffect> spellEffectBySelection;
	protected Dictionary<SpellSelection, SpellResponse> spellResponseBySelection;

	public delegate void SpellEffect(Player target, Player source);

	public void CastSpell (SpellSelection selection, Player target, Player source) {
		extraDamage = 0;
		extraDamageInDice = false;

		if (spellEffectBySelection != null) {
			spellEffectBySelection [selection] (target, source);
		}
	}

	public SpellResponse PreviewSpell (SpellSelection selection) {
		return spellResponseBySelection [selection];
	}

	protected void CauseDamage(int amount, Player target) {
		target.DecreaseHP (amount);
	}

	protected void HealDamage(int amount, Player target) {
		target.IncreaseHP (amount);
	}

//	protected void TestReverseDamageForStaticTarget(Player target, Player source, int damage) {
//		if(target.IsStatic()) {
//			source.DecreaseHP (damage);
//		}
//	}

	private void CauseDebuff() {
	}
}