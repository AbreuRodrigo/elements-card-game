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

	private void LavaBlast(Player target, Player source) {
	}

	private void MagmaPunch(Player target, Player source) {
	}
}
