using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FireSpell : SpellBase {

	public FireSpell() {
		spellEffectBySelection = new Dictionary<SpellSelection, SpellEffect> (3) {
			{ SpellSelection.Circle, FireBall },
			{ SpellSelection.Square, Scorch },
			{ SpellSelection.Rhombus, Overheat }
		};
	}

	private SpellResponse FireBall(Player target, Player source) {
		damage = 5;

		if (target.Debuffs.IsBurned) {
			damage += 3;
			target.Debuffs.RemoveBurn ();
		} else {
			target.Debuffs.AddBurn (0);
		}

		CauseDamage (damage, target);
		RemoveFreezeAndWetFromTarget (target);

		return response.ResetResponse ("Fire Ball", SpellType.Special);
	}

	private SpellResponse Scorch(Player target, Player source) {
		damage = 8;

		if (target.Debuffs.IsBurned) {
			damage += 3;
			target.Debuffs.RemoveBurn ();
		} else if(GamePlayController.instance.TakeAChanceUnder(50)) {			
			target.Debuffs.AddBurn (0);
		}

		CauseDamage (damage, target);
		RemoveFreezeAndWetFromTarget (target);

		return response.ResetResponse ("Scorch", SpellType.Special);
	}

	private SpellResponse Overheat(Player target, Player source) {
		if(source.Debuffs.IsFrozen || source.Debuffs.IsWet) {
			RemoveFreezeAndWetFromTarget (source);
			HealDamage (3, source);
		}

		return response.ResetResponse ("Overheat", SpellType.Cure);
	}

	private void RemoveFreezeAndWetFromTarget(Player target) {
		target.Debuffs.RemoveFreeze ();
		target.Debuffs.RemoveWet ();
	}
}
