using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaterSpell : SpellBase {

	public WaterSpell() {
		spellEffectBySelection = new Dictionary<SpellSelection, SpellEffect> (3) {
			{ SpellSelection.Circle, WaterBall },
			{ SpellSelection.Square, Refresh },
			{ SpellSelection.Rhombus, HighPressureBlast }
		};
	}

	private void WaterBall(Player target, Player source) {
	}

	private void Refresh(Player target, Player source) {
	}

	private void HighPressureBlast(Player target, Player source) {
	}
}