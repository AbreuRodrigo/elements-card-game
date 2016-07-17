using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NatureSpell : SpellBase {

	public NatureSpell() {
		spellEffectBySelection = new Dictionary<SpellSelection, SpellEffect> (3) {
			{ SpellSelection.Circle, RoseWhip },
			{ SpellSelection.Square, PoisonIvy },
			{ SpellSelection.Rhombus, HerbalMedicine }
		};
	}

	private SpellResponse RoseWhip(Player target, Player source) {
		damage = 5;

		if(target.Debuffs.IsBleeding) {
			damage += 3;
		}
		if (GamePlayController.instance.TakeAChanceUnder (50)) {
			HealDamage (3, source);
		} else {
			target.Debuffs.AddBleed ();
		}

		CauseDamage (damage, target);

		return response.ResetResponse ("Rose Whip", SpellType.Melee);
	}

	private SpellResponse PoisonIvy(Player target, Player source) {
		damage = 5;

		if(target.Debuffs.IsBleeding) {
			damage += 3;
		}

		if (target.Debuffs.IsPoisoned) {
			target.Debuffs.RemovePoison ();
		} else {
			target.Debuffs.AddPoison ();
		}

		CauseDamage (damage, target);

		return response.ResetResponse ("Poison Ivy", SpellType.Special);
	}

	private SpellResponse HerbalMedicine(Player target, Player source) {
		if(source.Debuffs.IsPoisoned || source.Debuffs.IsBleeding) {

			source.Debuffs.RemovePoison ();
			source.Debuffs.RemoveBleed ();

			HealDamage (3, source);
		}

		return response.ResetResponse ("Herbal Medicine", SpellType.Cure);
	}
}