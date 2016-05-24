using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BloodSpell : SpellBase {

	public BloodSpell() {
		spellEffectBySelection = new Dictionary<SpellSelection, SpellEffect> (3) {
			{ SpellSelection.Circle, HeavyStrike },
			{ SpellSelection.Square, VampiricStrike },
			{ SpellSelection.Rhombus, Impale }
		};
	}

	private void HeavyStrike(Player target, Player source) {
		CauseDamage (5, target);

		if (target.Debuffs.IsBleeding) {
			CauseDamage (3, target);
			target.Debuffs.RemoveBleed ();
		} else {
			target.Debuffs.AddBleed ();
		}
	}

	private void VampiricStrike(Player target, Player source) {
		if(target.Debuffs.IsBleeding) {
			int extraDamage = target.Debuffs.Bleed.ElapsedTurns;

			CauseDamage (1 + extraDamage, target);
			HealDamage (3 + extraDamage, source);

			target.Debuffs.RemoveBleed();
		}
	}

	private void Impale(Player target, Player source) {
		CauseDamage (5, target);

		if(target.Debuffs.IsBleeding) {
			int extraDamage = 1;
		}
	}
}