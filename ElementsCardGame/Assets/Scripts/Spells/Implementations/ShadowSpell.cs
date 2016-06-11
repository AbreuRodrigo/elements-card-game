using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShadowSpell : SpellBase {
	
	public ShadowSpell() {
		spellEffectBySelection = new Dictionary<SpellSelection, SpellEffect> (3) {
			{ SpellSelection.Circle, ShadowBall },
			{ SpellSelection.Square, SoulDrain },
			{ SpellSelection.Rhombus, LifeTap }
		};
	}

	private void ShadowBall(Player target, Player source) {
	}

	private void SoulDrain(Player target, Player source) {
	}

	private void LifeTap(Player target, Player source) {
	}
}