using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NatureSpell : SpellBase {

	public NatureSpell() {
		spellEffectBySelection = new Dictionary<SpellSelection, SpellEffect> (3) {
			{ SpellSelection.Circle, RoseWhip },
			{ SpellSelection.Square, PoisonIvy },
			{ SpellSelection.Rhombus, HerbalMedicine }
		};
	}

	private void RoseWhip(Player target, Player source) {
	}

	private void PoisonIvy(Player target, Player source) {
	}

	private void HerbalMedicine(Player target, Player source) {
	}
}