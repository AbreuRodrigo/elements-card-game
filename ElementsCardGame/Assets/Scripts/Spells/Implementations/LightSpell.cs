using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LightSpell : SpellBase {

	public LightSpell() {
		spellEffectBySelection = new Dictionary<SpellSelection, SpellEffect> (3) {
			{ SpellSelection.Circle, BlindingLight },
			{ SpellSelection.Square, Purge },
			{ SpellSelection.Rhombus, InnerGlow }
		};
	}

	private SpellResponse BlindingLight(Player target, Player source) {
		damage = 5;		

		if(target.Debuffs.IsCursed) {
			damage += 3;
		}

		if (GamePlayController.instance.TakeAChanceUnder (50)) {
			target.Debuffs.AddBlind ();
		}

		CauseDamage (damage, target);

		return response.ResetResponse ("Blinding Light", SpellType.Special);
	}

	private SpellResponse Purge(Player target, Player source) {
		if(source.Debuffs.IsCursed) {
			source.Debuffs.RemoveCurse ();
			HealDamage (3, source);
		}

		return response.ResetResponse ("Purge", SpellType.Cure);
	}

	private SpellResponse InnerGlow(Player target, Player source) {
		HealDamage (6, source);
		return response.ResetResponse ("Inner Glow", SpellType.Heal);
	}
}