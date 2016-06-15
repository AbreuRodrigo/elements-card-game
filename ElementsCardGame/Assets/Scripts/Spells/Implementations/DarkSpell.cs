using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DarkSpell : SpellBase {

	public DarkSpell() {
		spellEffectBySelection = new Dictionary<SpellSelection, SpellEffect> (3) {
			{ SpellSelection.Circle, CheapShot },
			{ SpellSelection.Square, Copy },
			{ SpellSelection.Rhombus, Peek }
		};
	}

	private void CheapShot(Player target, Player source) {
		Debug.Log ("CheapShot");
	}

	private void Copy(Player target, Player source) {
		Debug.Log ("Copy");
	}

	private void Peek(Player target, Player source) {
		Debug.Log ("Peek");
	}
}