using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DarkSpell : SpellBase {

	public DarkSpell() {
		spellEffectBySelection = new Dictionary<SpellSelection, SpellEffect> (3) {
			{ SpellSelection.Circle, CheapShot },
			{ SpellSelection.Square, Copy },
			{ SpellSelection.Rhombus, Peek }
		};

		spellResponseBySelection = new Dictionary<SpellSelection, SpellResponse> (3) {
			{ SpellSelection.Circle, new SpellResponse("Cheap Shot", SpellType.Melee) },
			{ SpellSelection.Square, new SpellResponse("Copy", SpellType.Special, true) },
			{ SpellSelection.Rhombus, new SpellResponse("Peek", SpellType.Special) }
		};
	}

	private void CheapShot(Player target, Player source) {
		int damage = 5;

		if(target.Debuffs.IsKnockedDown) {
			damage += 5;	
		}

		CauseDamage (damage, target);
	
		if(!source.goesFirst) {
			GamePlayController.instance.invertGoesFirstOnTurnEnd = true;
			source.goesFirst = true;
			target.goesFirst = false;
		}
	}

	private void Copy(Player target, Player source) {
	}

	private void Peek(Player target, Player source) {
	}
}