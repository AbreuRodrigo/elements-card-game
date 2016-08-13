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

		spellResponseBySelection = new Dictionary<SpellSelection, SpellResponse> (3) {
			{ SpellSelection.Circle, new SpellResponse("Ice Shards", SpellType.Special) },
			{ SpellSelection.Square, new SpellResponse("Wall Of Ice", SpellType.Shield) },
			{ SpellSelection.Rhombus, new SpellResponse("Glacial Blast", SpellType.Special) }
		};
	}

	private void IceShards(Player target, Player source) {
		damage = 5;

		int chanceForFreeze = 50;

		if(target.Debuffs.IsWet) {
			damage += 3;
			chanceForFreeze = 100;
		}

		if(GamePlayController.instance.TakeAChanceUnder(chanceForFreeze)) {
			target.Debuffs.AddFreeze ();
		}

		CauseDamage (damage, target);
	}

	private void WallOfIce(Player target, Player source) {
		source.protectionType = SpellType.Special;
	}

	private void GlacialBlast(Player target, Player source) {
		damage = 3;

		if (target.Debuffs.IsWet) {
			damage += 3;
		}

		if(!source.goesFirst && target.goesFirst && target.WasLastSpellSpecial()) {
			damage += (source.lastDamageReceived * 2);
		}

		CauseDamage(damage, target);
	}
}
