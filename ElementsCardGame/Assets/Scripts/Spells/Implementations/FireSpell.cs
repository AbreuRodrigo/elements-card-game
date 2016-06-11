using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FireSpell : SpellBase {

	public FireSpell() {
		spellEffectBySelection = new Dictionary<SpellSelection, SpellEffect> (3) {
			{ SpellSelection.Circle, FireBall },
			{ SpellSelection.Square, Scorch },
			{ SpellSelection.Rhombus, Overheat }
		};
	}

	private void FireBall(Player target, Player source) {
	}

	private void Scorch(Player target, Player source) {
	}

	private void Overheat(Player target, Player source) {
	}
}
