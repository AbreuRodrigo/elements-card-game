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
			{ SpellSelection.Circle, new SpellResponse("Cheap Shot", SpellType.Melee, false, false, false, true) },
			{ SpellSelection.Square, new SpellResponse("Copy", SpellType.Special, true, false, false, false) },
			{ SpellSelection.Rhombus, new SpellResponse("Peek", SpellType.Special, false, false, true, false) }
		};
	}

	private void CheapShot(Player target, Player source) {
		int damage = 5;

		if(target.Debuffs.IsKnockedDown) {
			damage += 5;	
		}

		CauseDamage (damage, target);
	}

	private void Copy(Player target, Player source) {
	}

	private void Peek(Player target, Player source) {
		GamePlayController.instance.StartPeekingMode (target);
	}
}