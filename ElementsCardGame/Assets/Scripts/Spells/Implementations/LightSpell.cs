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
	}

	private void Purge(Player target, Player source) {
	}

	private void InnerGlow(Player target, Player source) {
	}
}
