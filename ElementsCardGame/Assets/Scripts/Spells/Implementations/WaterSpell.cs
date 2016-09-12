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

		spellResponseBySelection = new Dictionary<SpellSelection, SpellResponse> (3) {
			{ SpellSelection.Circle, new SpellResponse("Water Ball", SpellType.Special) },
			{ SpellSelection.Square, new SpellResponse("Refresh", SpellType.Buff) },
			{ SpellSelection.Rhombus, new SpellResponse("High Pressure Blast", SpellType.Special) }
		};
	}

	private void WaterBall(Player target, Player source) {
		damage = 5;

		if(target.Debuffs.IsPoisoned) {
			damage += 3;
		}

		target.Debuffs.AddWet (5);
		target.Debuffs.RemoveBurn ();

		CauseDamage (damage, target);
	}

	private void Refresh(Player target, Player source) {
		source.Debuffs.AddRefresh (5);

		source.Debuffs.RemoveBurn ();
	}

	private void HighPressureBlast(Player target, Player source) {
		damage = 10;

		if(target.Debuffs.IsPoisoned) {
			damage += 3;
		}

		source.skipNextTurn = true;
		target.Debuffs.RemoveBurn ();

		target.Debuffs.AddWet (5);

		CauseDamage (damage, target);
	}
}