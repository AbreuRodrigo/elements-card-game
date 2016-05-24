using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChaosSpell : SpellBase {

	public ChaosSpell() {
		spellEffectBySelection = new Dictionary<SpellSelection, SpellEffect> (2) {
			{ SpellSelection.Circle, Hellfire },
			{ SpellSelection.Square, SoulDestroyer }
		};
	}

	private void Hellfire(Player target, Player source) {
	
	}

	private void SoulDestroyer(Player target, Player source) {
	
	}
}