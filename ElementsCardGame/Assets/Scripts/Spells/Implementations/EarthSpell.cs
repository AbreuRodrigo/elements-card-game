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

		spellResponseBySelection = new Dictionary<SpellSelection, SpellResponse> (3) {
			{ SpellSelection.Circle, new SpellResponse("Rock Punch", SpellType.Melee) },
			{ SpellSelection.Square, new SpellResponse("Boulder Wall", SpellType.Shield) },
			{ SpellSelection.Rhombus, new SpellResponse("Quake Stomp", SpellType.Melee) }
		};
	}

	private void RockPunch(Player target, Player source) {
		damage = 5;

		if(target.Debuffs.IsFrozen) {
			damage += 3;
		}

		CauseDamage (damage, target);

		if(GamePlayController.instance.TakeAChanceUnder(50)) {
			target.Debuffs.AddKnockDown ();
		}
	}

	private void BoulderWall(Player target, Player source) {
		source.protectionType = SpellType.Melee;
	}

	private void QuakeStomp(Player target, Player source) {
		damage = 3;

		if (target.Debuffs.IsFrozen) {
			damage += 3;
		}

		if(!source.goesFirst && target.goesFirst && target.WasLastSpellMelee()) {
			damage += (source.lastDamageReceived * 2);
			target.Debuffs.AddKnockDown ();
		}

		CauseDamage(damage, target);
	}
}