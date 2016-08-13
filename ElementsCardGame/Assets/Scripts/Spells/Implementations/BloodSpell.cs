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

		spellResponseBySelection = new Dictionary<SpellSelection, SpellResponse> (3) {
			{ SpellSelection.Circle, new SpellResponse("Heavy Strike", SpellType.Melee)},
			{ SpellSelection.Square, new SpellResponse("Vampiric Strike", SpellType.Melee)},
			{ SpellSelection.Rhombus, new SpellResponse("Impale", SpellType.Melee, false, true)}
		};
	}

	private void HeavyStrike(Player target, Player source) {
		damage = 5;

		if (target.Debuffs.IsBleeding) {
			damage += 3;
			target.Debuffs.RemoveBleed ();
		} else {
			target.Debuffs.AddBleed ();
		}

		CauseDamage (damage, target);
	}

	private void VampiricStrike(Player target, Player source) {
		if (target.Debuffs.IsBleeding) {
			extraDamage = target.Debuffs.Bleed.ElapsedTurns;

			CauseDamage (1 + extraDamage, target);
			HealDamage (3 + extraDamage, source);

			target.Debuffs.RemoveBleed ();
		}
	}

	private void Impale(Player target, Player source) {
		damage = 5;

		if(target.Debuffs.IsBleeding) {
			GamePlayController.instance.Roll1Die ();
			damage += GamePlayController.instance.Dice1Result;
		}

		CauseDamage (damage, target);
	}
}