using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChaosSpell : SpellBase {

	public ChaosSpell() {
		spellEffectBySelection = new Dictionary<SpellSelection, SpellEffect> (2) {
			{ SpellSelection.Circle, Hellfire },
			{ SpellSelection.Square, SoulDestroyer }
		};

		spellResponseBySelection = new Dictionary<SpellSelection, SpellResponse> (2) {
			{ SpellSelection.Circle, new SpellResponse("HellFire", SpellType.Special) },
			{ SpellSelection.Square, new SpellResponse("Soul Destroyer", SpellType.Special) }
		};
	}

	private void Hellfire(Player target, Player source) {
		int damage = 10;

		if(target.Debuffs.IsBurned || target.Debuffs.IsCursed) {
			damage += 5;
		}

		target.Debuffs.AddBurn (5);

		CauseDamage (damage, target);

		target.Debuffs.RemoveWet ();
		target.Debuffs.RemoveFreeze ();
	}

	private void SoulDestroyer(Player target, Player source) {
		int damage = 10;

		if(target.Debuffs.IsCursed) {
			damage += 5;
		}

		if(target.Debuffs.IsBurned) {
			HealDamage (5, target);
		}

		target.Debuffs.AddCurse (5);

		CauseDamage (damage, target);
	}
}