using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PlayerDebuffs {
	[SerializeField]
	private bool bleeding;
	[SerializeField]
	private bool blinded;
	[SerializeField]
	private bool burned;
	[SerializeField]
	private bool cursed;
	[SerializeField]
	private bool frozen;
	[SerializeField]
	private bool knockedDown;
	[SerializeField]
	private bool poisoned;
	[SerializeField]
	private bool wet;

	private Dictionary<DebuffType, Debuff> debuffByType;

	public PlayerDebuffs() {
		debuffByType = new Dictionary<DebuffType, Debuff> (8) {
			{ DebuffType.Bleed, new BleedDebuff() },
			{ DebuffType.Blind, new BleedDebuff() },
			{ DebuffType.Burn, new BleedDebuff() },
			{ DebuffType.Curse, new BleedDebuff() },
			{ DebuffType.Freeze, new BleedDebuff() },
			{ DebuffType.KnockDown, new BleedDebuff() },
			{ DebuffType.Poison, new BleedDebuff() },
			{ DebuffType.Wet, new BleedDebuff() }
		};
	}

	public Debuff Bleed {
		get { return debuffByType [DebuffType.Bleed]; }
	}

	public Debuff Blind {
		get { return debuffByType [DebuffType.Blind]; }
	}

	public Debuff Burn {
		get { return debuffByType [DebuffType.Burn]; }
	}

	public Debuff Curse {
		get { return debuffByType [DebuffType.Curse]; }
	}

	public Debuff Freeze {
		get { return debuffByType [DebuffType.Freeze]; }
	}

	public Debuff KnockedDown {
		get { return debuffByType [DebuffType.KnockDown]; }
	}

	public Debuff Poison {
		get { return debuffByType [DebuffType.Poison]; }
	}

	public Debuff Wet {
		get { return debuffByType [DebuffType.Wet]; }
	}

	public bool IsBleeding {
		get { return bleeding; }
	}

	public bool IsBlind {
		get { return blinded; }
	}

	public bool IsBurned {
		get { return burned; }
	}

	public bool IsCursed {
		get { return cursed; }
	}

	public bool IsFrozen {
		get { return frozen; }
	}

	public bool IsKnockedDown {
		get { return knockedDown; }
	}

	public bool IsPoisoned {
		get { return poisoned; }
	}

	public bool IsWet {
		get { return wet; }
	}

	public void AddBleed() {
		bleeding = true;
	}

	public void RemoveBleed() {
		bleeding = false;
	}

	public void AddBlind() {
		blinded = true;
	}

	public void RemoveBlind() {
		blinded = false;
	}

	public void AddBurn() {
		burned = true;
	}

	public void RemoveBurn() {
		burned = false;
	}

	public void AddCurse() {
		cursed = true;
	}

	public void RemoveCurse() {
		cursed = false;
	}

	public void AddFreeze() {
		frozen = true;
	}

	public void RemoveFreeze() {
		frozen = false;
	}

	public void AddKnockDown() {
		knockedDown = true;
	}

	public void RemoveKnockDown() {
		knockedDown = false;
	}

	public void AddPoison() {
		poisoned = true;
	}

	public void RemovePoison() {
		poisoned = false;
	}

	public void AddWet() {
		wet = true;
	}

	public void RemoveWet() {
		wet = false;
	}
}