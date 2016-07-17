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
	}

	private SpellResponse CheapShot(Player target, Player source) {
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

		return response.ResetResponse("Cheap Shot", SpellType.Melee);
	}

	private SpellResponse Copy(Player target, Player source) {
		response.ResetResponse("Copy", SpellType.Special);
		response.mockEffect = true;
		return response;
	}

	private SpellResponse Peek(Player target, Player source) {
		return response.ResetResponse("Peek", SpellType.Special);
	}
}