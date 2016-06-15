using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LightiningSpell : SpellBase {

	public LightiningSpell() {
		spellEffectBySelection = new Dictionary<SpellSelection, SpellEffect> (3) {
			{ SpellSelection.Circle, LightiningBolt },
			{ SpellSelection.Square, Thunder },
			{ SpellSelection.Rhombus, Static }
		};
	}

	private void LightiningBolt(Player target, Player source) {
		Debug.Log ("LightningBolt");
	}

	private void Thunder(Player target, Player source) {
		Debug.Log ("Thunder");
	}

	private void Static(Player target, Player source) {
		Debug.Log ("Static");
	}
}
