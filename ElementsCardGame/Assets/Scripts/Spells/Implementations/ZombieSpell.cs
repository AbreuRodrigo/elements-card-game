using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZombieSpell : SpellBase {

	public ZombieSpell() {
		spellEffectBySelection = new Dictionary<SpellSelection, SpellEffect> (2) {
			{ SpellSelection.Circle, InfectedBite },
			{ SpellSelection.Square, Canibalize }
		};
	}

	private void InfectedBite(Player target, Player source) {
		Debug.Log ("InfectedBite");
	}

	private void Canibalize(Player target, Player source) {
		Debug.Log ("Canibalize");
	}
}