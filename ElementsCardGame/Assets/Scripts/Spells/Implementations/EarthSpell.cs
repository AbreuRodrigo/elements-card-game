using UnityEngine;
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

	private SpellResponse RockPunch(Player target, Player source) {
		damage = 5;

		if(target.Debuffs.IsFrozen) {
			damage += 3;
		}

		response.ResetResponse ("Rock Punch", SpellType.Melee);

		CauseDamage (damage, target);

		if(GamePlayController.instance.TakeAChanceUnder(50)) {
			target.Debuffs.AddKnockDown ();
		}

		return response;
	}

	private SpellResponse BoulderWall(Player target, Player source) {
		source.protectionType = SpellType.Melee;
		return response.ResetResponse ("Boulder Wall", SpellType.Shield);
	}

	private SpellResponse QuakeStomp(Player target, Player source) {
		damage = 3;

		if (target.Debuffs.IsFrozen) {
			damage += 3;
		}

		if(!source.goesFirst && target.goesFirst && target.WasLastSpellMelee()) {
			damage += (source.lastDamageReceived * 2);
			target.Debuffs.AddKnockDown ();
		}

		CauseDamage(damage, target);

		return response.ResetResponse ("Quake Stomp", SpellType.Melee);
	}
}