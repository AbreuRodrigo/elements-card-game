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

	private SpellResponse WaterBall(Player target, Player source) {
		damage = 5;

		if(target.Debuffs.IsPoisoned) {
			damage += 3;
		}

		target.Debuffs.AddWet ();
		target.Debuffs.RemoveBurn ();

		CauseDamage (damage, target);

		return response.ResetResponse ("Water Ball", SpellType.Special);
	}

	private SpellResponse Refresh(Player target, Player source) {
		source.Debuffs.AddRefresh ();

		source.Debuffs.RemoveBurn ();

		return response.ResetResponse ("Refresh", SpellType.Buff);
	}

	private SpellResponse HighPressureBlast(Player target, Player source) {
		damage = 10;

		if(target.Debuffs.IsPoisoned) {
			damage += 3;
		}

		source.skipNextTurn = true;
		target.Debuffs.RemoveBurn ();

		if(target.Debuffs.IsWet) {
			target.Debuffs.RemoveWet ();
		}else {
			target.Debuffs.AddWet ();
		}

		CauseDamage (damage, target);

		return response.ResetResponse ("High Pressure Blast", SpellType.Special);
	}
}