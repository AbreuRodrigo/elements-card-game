﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EarthSpell : SpellBase {
	
	public EarthSpell() {
		spellEffectBySelection = new Dictionary<SpellSelection, SpellEffect> (3) {
			{ SpellSelection.Circle, RockPunch },
			{ SpellSelection.Square, BoulderWall },
			{ SpellSelection.Rhombus, QuakeStomp }
		};
	}

	private void RockPunch(Player target, Player source) {	
	}

	private void BoulderWall(Player target, Player source) {
	}

	private void QuakeStomp(Player target, Player source) {
	}
}