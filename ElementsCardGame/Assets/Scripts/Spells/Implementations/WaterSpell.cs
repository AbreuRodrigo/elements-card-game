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
		Debug.Log ("WaterBall");
	}

	private void Refresh(Player target, Player source) {
		Debug.Log ("Refresh");
	}

	private void HighPressureBlast(Player target, Player source) {
		Debug.Log ("HighPressureBlast");
	}
}