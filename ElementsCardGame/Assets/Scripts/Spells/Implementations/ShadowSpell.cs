using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShadowSpell : SpellBase {
	
	public ShadowSpell() {
		spellEffectBySelection = new Dictionary<SpellSelection, SpellEffect> (3) {
			{ SpellSelection.Circle, ShadowBall },
			{ SpellSelection.Square, SoulDrain },
			{ SpellSelection.Rhombus, LifeTap }
		};
	}

	private SpellResponse ShadowBall(Player target, Player source) {
		damage = 5;

		if (target.Debuffs.IsCursed) {
			GamePlayController.instance.Roll1Die ();
			damage += GamePlayController.instance.Dice1Result;
			target.Debuffs.RemoveCurse ();
		} else {
			target.Debuffs.AddCurse (0);
		}

		CauseDamage (damage, target);

		return response.ResetResponse ("Shadow Ball", SpellType.Special);
	}

	private SpellResponse SoulDrain(Player target, Player source) {
		damage = 3;

		int heal = 3;

		if(target.Debuffs.IsCursed) {
			damage += 2;
			heal += 2;
		}

		CauseDamage (damage, target);
		HealDamage (heal, source);

		return response.ResetResponse ("Soul Drain", SpellType.Special);
	}

	private SpellResponse LifeTap(Player target, Player source) {
		GamePlayController.instance.Roll2Dice ();
		int r1 = GamePlayController.instance.Dice1Result;
		int r2 = GamePlayController.instance.Dice2Result;

		if (r1 < r2) {
			CauseDamage (r1, source);
			CauseDamage (r1 + r2, target);
		} else {
			CauseDamage (r2, source);
			CauseDamage (r1 + r2, target);
		}

		target.stats.UpdateHP ();
		source.stats.UpdateHP ();

		return response.ResetResponse ("Life Tap", SpellType.Special);
	}
}