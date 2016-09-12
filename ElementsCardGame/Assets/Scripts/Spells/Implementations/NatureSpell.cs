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

		spellResponseBySelection = new Dictionary<SpellSelection, SpellResponse> (3) {
			{ SpellSelection.Circle, new SpellResponse ("Rose Whip", SpellType.Melee) },
			{ SpellSelection.Square, new SpellResponse ("Poison Ivy", SpellType.Special) },
			{ SpellSelection.Rhombus, new SpellResponse ("Herbal Medicine", SpellType.Cure) }
		};
	}

	private void RoseWhip(Player target, Player source) {
		damage = 5;

		if(target.Debuffs.IsBleeding) {
			damage += 3;
		}
		if (GamePlayController.instance.TakeAChanceUnder (50)) {
			HealDamage (3, source);
		} else {
			target.Debuffs.AddBleed (5);
		}

		CauseDamage (damage, target);
	}

	private void PoisonIvy(Player target, Player source) {
		damage = 5;

		if(target.Debuffs.IsBleeding) {
			damage += 3;
		}

		target.Debuffs.AddPoison (3);

		CauseDamage (damage, target);
	}

	private void HerbalMedicine(Player target, Player source) {
		if(source.Debuffs.IsPoisoned || source.Debuffs.IsBleeding) {

			source.Debuffs.RemovePoison ();
			source.Debuffs.RemoveBleed ();

			HealDamage (3, source);
		}
	}
}