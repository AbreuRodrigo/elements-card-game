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

	private SpellResponse HeavyStrike(Player target, Player source) {
		damage = 5;

		if (target.Debuffs.IsBleeding) {
			damage += 3;
			target.Debuffs.RemoveBleed ();
		} else {
			target.Debuffs.AddBleed ();
		}

		CauseDamage (damage, target);

		return response.ResetResponse ("Heavy Strike", SpellType.Melee);
	}

	private SpellResponse VampiricStrike(Player target, Player source) {
		if (target.Debuffs.IsBleeding) {
			extraDamage = target.Debuffs.Bleed.ElapsedTurns;

			CauseDamage (1 + extraDamage, target);
			HealDamage (3 + extraDamage, source);

			target.Debuffs.RemoveBleed ();
		}

		return response.ResetResponse ("Vampiric Strike", SpellType.Melee);
	}

	private SpellResponse Impale(Player target, Player source) {
		damage = 5;

		if(target.Debuffs.IsBleeding) {
			GamePlayController.instance.Roll1Die ();
			damage += GamePlayController.instance.Dice1Result;
		}

		response.ResetResponse ("Impale", SpellType.Melee);

		CauseDamage (damage, target);

		return response;
	}
}