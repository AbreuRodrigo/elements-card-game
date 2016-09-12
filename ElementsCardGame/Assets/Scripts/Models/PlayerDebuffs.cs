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
	private bool freeze;
	[SerializeField]
	private bool knockedDown;
	[SerializeField]
	private bool poisoned;
	[SerializeField]
	private bool wet;
	[SerializeField]
	private bool statics;
	[SerializeField]
	private bool refresh;

	private Player player;

	private Dictionary<BuffDebuffType, BuffDebuff> debuffByType;
	private Dictionary<BuffDebuffType, BuffDebuff> buffByType;

	public PlayerDebuffs(Player observer) {
		player = observer;

		debuffByType = new Dictionary<BuffDebuffType, BuffDebuff> (8) {
			{ BuffDebuffType.Bleed, new BleedDebuff() },
			{ BuffDebuffType.Blind, new BlindDebuff() },
			{ BuffDebuffType.Burn, new BurnDebuff() },
			{ BuffDebuffType.Curse, new CurseDebuff() },
			{ BuffDebuffType.Freeze, new FreezeDebuff() },
			{ BuffDebuffType.KnockDown, new KnockDownDebuff() },
			{ BuffDebuffType.Poison, new PoisonDebuff() },
			{ BuffDebuffType.Wet, new WetDebuff() }
		};

		buffByType = new Dictionary<BuffDebuffType, BuffDebuff> (2) {
			{ BuffDebuffType.Refresh, new RefreshBuff() },
			{ BuffDebuffType.Static, new StaticBuff() }
		};
	}

	public BuffDebuff Bleed {
		get { return debuffByType [BuffDebuffType.Bleed]; }
	}

	public BuffDebuff Blind {
		get { return debuffByType [BuffDebuffType.Blind]; }
	}

	public BuffDebuff Burn {
		get { return debuffByType [BuffDebuffType.Burn]; }
	}

	public BuffDebuff Curse {
		get { return debuffByType [BuffDebuffType.Curse]; }
	}

	public BuffDebuff Freeze {
		get { return debuffByType [BuffDebuffType.Freeze]; }
	}

	public BuffDebuff KnockedDown {
		get { return debuffByType [BuffDebuffType.KnockDown]; }
	}

	public BuffDebuff Poison {
		get { return debuffByType [BuffDebuffType.Poison]; }
	}

	public BuffDebuff Wet {
		get { return debuffByType [BuffDebuffType.Wet]; }
	}

	public BuffDebuff Static {
		get { return buffByType [BuffDebuffType.Static]; }
	}

	public BuffDebuff Refresh {
		get { return buffByType [BuffDebuffType.Refresh]; }
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
		get { return freeze; }
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

	public bool IsStatic {
		get { return statics; }
	}

	public bool IsRefreshing {
		get { return refresh; }
	}

	public void AddBleed(int duration) {
		Bleed.ActivateDebuff (duration);

		if (!bleeding) {
			bleeding = true;
			player.stats.debuffManager.AddDebuffMarker (CardElement.Blood, duration);
		} else {
			player.stats.debuffManager.ResetDebuffMarker (CardElement.Blood, duration);
		}
	}

	public void RemoveBleed() {
		if (bleeding) {
			bleeding = false;
			Bleed.DeactivateDebuff ();
			player.stats.debuffManager.RemoveDebuffMarker (CardElement.Blood);
		}
	}

	public void AddBlind(int duration) {
		Blind.ActivateDebuff (duration);

		if (!blinded) {
			blinded = true;
			player.stats.debuffManager.AddDebuffMarker (CardElement.Light, duration);
		} else {
			player.stats.debuffManager.ResetDebuffMarker (CardElement.Light, duration);
		}
	}

	public void RemoveBlind() {
		if (blinded) {
			blinded = false;
			Blind.DeactivateDebuff ();
			player.stats.debuffManager.RemoveDebuffMarker (CardElement.Light);
		}
	}

	public void AddBurn(int duration) {
		Burn.ActivateDebuff (duration);

		if (!burned) {
			burned = true;
			player.stats.debuffManager.AddDebuffMarker (CardElement.Fire, duration);
		} else {
			player.stats.debuffManager.ResetDebuffMarker (CardElement.Fire, duration);
		}
	}

	public void RemoveBurn() {
		if (burned) {
			burned = false;
			Burn.DeactivateDebuff ();
			player.stats.debuffManager.RemoveDebuffMarker (CardElement.Fire);
		}
	}

	public void AddCurse(int duration) {
		Curse.ActivateDebuff (duration);

		if (!cursed) {
			cursed = true;
			player.stats.debuffManager.AddDebuffMarker (CardElement.Shadow, duration);
		} else {
			player.stats.debuffManager.ResetDebuffMarker (CardElement.Shadow, duration);
		}
	}

	public void RemoveCurse() {
		if (cursed) {
			cursed = false;
			Curse.DeactivateDebuff ();
			player.stats.debuffManager.RemoveDebuffMarker (CardElement.Shadow);
		}
	}

	public void AddFreeze(int duration) {
		Freeze.ActivateDebuff (duration);

		if(!freeze) {
			freeze = true;
			player.stats.debuffManager.AddDebuffMarker (CardElement.Ice, duration);
		}else {
			player.stats.debuffManager.ResetDebuffMarker (CardElement.Ice, duration);
		}
	}

	public void RemoveFreeze() {
		if (freeze) {
			freeze = false;
			Freeze.DeactivateDebuff ();
			player.stats.debuffManager.RemoveDebuffMarker (CardElement.Ice);
		}
	}

	public void AddKnockDown() {
		if (!knockedDown) {
			knockedDown = true;
			KnockedDown.ActivateDebuff (0);
			player.stats.debuffManager.AddDebuffMarker (CardElement.Earth, 0);
			player.skipNextTurn = true;
		}
	}

	public void RemoveKnockDown() {
		if(knockedDown) {
			knockedDown = false;
			KnockedDown.DeactivateDebuff ();
			player.stats.debuffManager.RemoveDebuffMarker (CardElement.Earth);
		}
	}

	public void AddPoison(int duration) {
		Poison.ActivateDebuff (duration);

		if (!poisoned) {
			poisoned = true;
			player.stats.debuffManager.AddDebuffMarker (CardElement.Nature, duration);
		} else {
			player.stats.debuffManager.ResetDebuffMarker (CardElement.Nature, duration);
		}
	}

	public void RemovePoison() {
		if (poisoned) {
			poisoned = false;
			Poison.DeactivateDebuff ();
			player.stats.debuffManager.RemoveDebuffMarker (CardElement.Nature);
		}
	}

	public void AddWet(int duration) {
		Wet.ActivateDebuff (duration);

		if (!wet) {
			wet = true;
			player.stats.debuffManager.AddDebuffMarker (CardElement.Water, duration);
		} else {
			player.stats.debuffManager.ResetDebuffMarker (CardElement.Water, duration);
		}
	}

	public void RemoveWet() {
		if (wet) {
			wet = false;
			Wet.DeactivateDebuff ();
			player.stats.debuffManager.RemoveDebuffMarker (CardElement.Water);
		}
	}

	public void AddStatics(int duration) {
		Static.ActivateDebuff (duration);

		if (!statics) {
			statics = true;
			player.stats.debuffManager.AddBuffMarker (CardElement.Lightning, duration);
		} else {
			player.stats.debuffManager.ResetBuffMarker (CardElement.Lightning, duration);
		}
	}

	public void RemoveStatics() {
		if (statics) {
			statics = false;
			Static.DeactivateDebuff ();
			player.stats.debuffManager.RemoveBuffMarker (CardElement.Lightning);
		}
	}

	public void AddRefresh(int duration) {
		Refresh.ActivateDebuff (duration);

		if (!refresh) {
			refresh = true;
			player.stats.debuffManager.AddBuffMarker (CardElement.Water, duration);
		} else {
			player.stats.debuffManager.ResetBuffMarker (CardElement.Water, duration);
		}
	}

	public void RemoveRefresh() {
		if (refresh) {
			refresh = false;
			Refresh.DeactivateDebuff ();
			player.stats.debuffManager.RemoveBuffMarker (CardElement.Water);
		}
	}

	public void ExecuteDebuffsIfActive() {
		ExecuteDebuffIfActive (Bleed);
		ExecuteDebuffIfActive (Blind);
		ExecuteDebuffIfActive (Burn);
		ExecuteDebuffIfActive (Curse);
		ExecuteDebuffIfActive (Freeze);
		ExecuteDebuffIfActive (KnockedDown);
		ExecuteDebuffIfActive (Poison);
		ExecuteDebuffIfActive (Wet);
		ExecuteDebuffIfActive (Static);
		ExecuteDebuffIfActive (Refresh);
	}

	private void ExecuteDebuffIfActive(BuffDebuff debuff) {
		if(debuff.IsActive) {
			debuff.ExecuteBuffDebuff (player);
		}
	}
}
