using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IceSpell : SpellBase {

	public IceSpell() {
		spellEffectBySelection = new Dictionary<SpellSelection, SpellEffect> (3) {
			{ SpellSelection.Circle, IceShards },
			{ SpellSelection.Square, WallOfIce },
			{ SpellSelection.Rhombus, GlacialBlast }
		};
	}

	private void IceShards(Player target, Player source) {
		Debug.Log ("IceShards");
	}

	private void WallOfIce(Player target, Player source) {
		Debug.Log ("WallOfIce");
	}

	private void GlacialBlast(Player target, Player source) {
		Debug.Log ("GlacialBlast");
	}
}
