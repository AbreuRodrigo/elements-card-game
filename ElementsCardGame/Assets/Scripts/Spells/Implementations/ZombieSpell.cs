using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZombieSpell : SpellBase {

	public ZombieSpell() {
		spellEffectBySelection = new Dictionary<SpellSelection, SpellEffect> (2) {
			{ SpellSelection.Circle, InfectedBite },
			{ SpellSelection.Square, Canibalize }
		};

		spellResponseBySelection = new Dictionary<SpellSelection, SpellResponse> (2) {
			{ SpellSelection.Circle, new SpellResponse ("Infected Bite", SpellType.Melee) },
			{ SpellSelection.Square, new SpellResponse ("Canibalize", SpellType.Melee) }
		};
	}

	private void InfectedBite(Player target, Player source) {
		damage = 10;

		if(target.Debuffs.IsBleeding) {
			damage += 5;
		}

		target.Debuffs.AddCurse (5);

		CauseDamage (damage, target);
	}

	private void Canibalize(Player target, Player source) {
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
	}
}