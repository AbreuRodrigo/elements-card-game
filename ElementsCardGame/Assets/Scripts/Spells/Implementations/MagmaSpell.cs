using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MagmaSpell : SpellBase {

	public MagmaSpell() {
		spellEffectBySelection = new Dictionary<SpellSelection, SpellEffect> (2) {
			{ SpellSelection.Circle, LavaBlast },
			{ SpellSelection.Square, MagmaPunch }
		};
	}

	private SpellResponse LavaBlast(Player target, Player source) {
		damage = 10;

		if (target.Debuffs.IsBurned) {
			damage += 5;
		} else {
			target.Debuffs.AddBurn (5);
		}

		if(GamePlayController.instance.TakeAChanceUnder(50)) {
			target.Debuffs.AddKnockDown ();
		}

		target.Debuffs.RemoveWet ();
		target.Debuffs.RemoveFreeze ();

		CauseDamage (damage, target);

		return response.ResetResponse ("Lava Blast", SpellType.Special);
	}

	private SpellResponse MagmaPunch(Player target, Player source) {
		damage = 10;

		if(target.Debuffs.IsBurned) {
			damage += 5;
		}

		target.Debuffs.AddKnockDown ();

		if(GamePlayController.instance.TakeAChanceUnder(50)) {
			if (target.Debuffs.IsBurned) {
				target.Debuffs.RemoveBurn ();
			} else {
				target.Debuffs.AddBurn (0);
			}
		}

		CauseDamage (damage, target);

		return response.ResetResponse ("Magma Punch", SpellType.Melee);
	}
}
