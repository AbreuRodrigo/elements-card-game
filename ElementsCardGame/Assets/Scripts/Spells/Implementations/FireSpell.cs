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

		spellResponseBySelection = new Dictionary<SpellSelection, SpellResponse> (3) {
			{ SpellSelection.Circle, new SpellResponse("Fire Ball", SpellType.Special) },
			{ SpellSelection.Square, new SpellResponse("Scorch", SpellType.Special) },
			{ SpellSelection.Rhombus, new SpellResponse("Overheat", SpellType.Cure) }
		};
	}

	private void FireBall(Player target, Player source) {
		damage = 5;

		if (target.Debuffs.IsBurned) {
			damage += 3;
		}

		target.Debuffs.AddBurn (4);

		CauseDamage (damage, target);
		RemoveFreezeAndWetFromTarget (target);
	}

	private void Scorch(Player target, Player source) {
		damage = 8;

		if (target.Debuffs.IsBurned) {
			damage += 3;
		}

		target.Debuffs.AddBurn (4);

		CauseDamage (damage, target);
		RemoveFreezeAndWetFromTarget (target);
	}

	private void Overheat(Player target, Player source) {
		if(source.Debuffs.IsFrozen || source.Debuffs.IsWet) {
			RemoveFreezeAndWetFromTarget (source);
			HealDamage (3, source);
		}
	}

	private void RemoveFreezeAndWetFromTarget(Player target) {
		target.Debuffs.RemoveFreeze ();
		target.Debuffs.RemoveWet ();
	}
}
