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

	private SpellResponse IceShards(Player target, Player source) {
		damage = 5;

		int chanceForFreeze = 50;

		if(target.Debuffs.IsWet) {
			damage += 3;
			chanceForFreeze = 100;
		}

		if(GamePlayController.instance.TakeAChanceUnder(chanceForFreeze)) {
			target.Debuffs.AddFreeze ();
		}

		return response.ResetResponse ("Ice Shards", SpellType.Special);
	}

	private SpellResponse WallOfIce(Player target, Player source) {
		source.protectionType = SpellType.Special;
		return response.ResetResponse ("Wall Of Ice", SpellType.Shield);
	}

	private SpellResponse GlacialBlast(Player target, Player source) {
		damage = 3;

		if (target.Debuffs.IsWet) {
			damage += 3;
		}

		if(!source.goesFirst && target.goesFirst && target.WasLastSpellSpecial()) {
			damage += (source.lastDamageReceived * 2);
		}

		CauseDamage(damage, target);

		return response.ResetResponse ("Glacial Blast", SpellType.Special);
	}
}
