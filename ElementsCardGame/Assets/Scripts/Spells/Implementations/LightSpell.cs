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

	private void BlindingLight(Player target, Player source) {
		Debug.Log ("BlindingLight");
	}

	private void Purge(Player target, Player source) {
		Debug.Log ("Purge");
	}

	private void InnerGlow(Player target, Player source) {
		Debug.Log ("InnerGlow");
	}
}
