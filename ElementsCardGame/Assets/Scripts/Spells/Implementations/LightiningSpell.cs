using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LightiningSpell : SpellBase {

	public LightiningSpell() {
		spellEffectBySelection = new Dictionary<SpellSelection, SpellEffect> (3) {
			{ SpellSelection.Circle, LightiningBolt },
			{ SpellSelection.Square, Thunder },
			{ SpellSelection.Rhombus, Static }
		};

		spellResponseBySelection = new Dictionary<SpellSelection, SpellResponse> (3) {
			{ SpellSelection.Circle, new SpellResponse("Lightning Bolt", SpellType.Special) },
			{ SpellSelection.Square, new SpellResponse("Thunder", SpellType.Special) },
			{ SpellSelection.Rhombus, new SpellResponse("Static", SpellType.Buff) }
		};
	}

	private void LightiningBolt(Player target, Player source) {
		damage = 5;

		if(target.Debuffs.IsWet) {
			damage += 3;
		}

		if(!source.goesFirst) {
			GamePlayController.instance.invertGoesFirstOnTurnEnd = true;
			source.goesFirst = true;
			target.goesFirst = false;
		}

		CauseDamage (damage, target);
	}

	private void Thunder(Player target, Player source) {
		damage = 5;

		if(target.Debuffs.IsWet) {
			damage += 3;
		}

		if (GamePlayController.instance.TakeAChanceUnder (50)) {
			damage += 3;
		} else {
			CauseDamage (3, source);
		}

		CauseDamage (damage, target);
	}

	private void Static(Player target, Player source) {
		source.Debuffs.AddStatics (5);
	}
}
