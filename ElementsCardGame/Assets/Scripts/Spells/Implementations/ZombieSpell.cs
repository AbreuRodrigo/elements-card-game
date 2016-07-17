using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZombieSpell : SpellBase {

	public ZombieSpell() {
		spellEffectBySelection = new Dictionary<SpellSelection, SpellEffect> (2) {
			{ SpellSelection.Circle, InfectedBite },
			{ SpellSelection.Square, Canibalize }
		};
	}

	private SpellResponse InfectedBite(Player target, Player source) {
		damage = 10;

		if(target.Debuffs.IsBleeding) {
			damage += 5;
		}

		target.Debuffs.AddCurse (5);

		CauseDamage (damage, target);

		return response.ResetResponse ("Infected Bite", SpellType.Melee);
	}

	private SpellResponse Canibalize(Player target, Player source) {
		damage = 5;
		int heal = 5;

		if(target.Debuffs.IsCursed) {
			damage += 5;
		}
		if(target.Debuffs.IsBleeding) {
			heal += 5;
		}

		CauseDamage (damage, target);
		HealDamage (heal, source);

		return response.ResetResponse ("Canibalize", SpellType.Melee);
	}
}