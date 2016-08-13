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

		spellResponseBySelection = new Dictionary<SpellSelection, SpellResponse> (3) {
			{ SpellSelection.Circle, new SpellResponse ("Blinding Light", SpellType.Special) },
			{ SpellSelection.Square, new SpellResponse ("Purge", SpellType.Cure) },
			{ SpellSelection.Rhombus, new SpellResponse ("Inner Glow", SpellType.Heal) }
		};
	}

	private void BlindingLight(Player target, Player source) {
		damage = 5;		

		if(target.Debuffs.IsCursed) {
			damage += 3;
		}

		if (GamePlayController.instance.TakeAChanceUnder (50)) {
			target.Debuffs.AddBlind ();
		}

		CauseDamage (damage, target);
	}

	private void Purge(Player target, Player source) {
		if(source.Debuffs.IsCursed) {
			source.Debuffs.RemoveCurse ();
			HealDamage (3, source);
		}
	}

	private void InnerGlow(Player target, Player source) {
		HealDamage (6, source);
	}
}