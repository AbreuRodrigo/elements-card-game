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
	[SerializeField]
	private bool statics;
	[SerializeField]
	private bool refresh;

	private Player player;

	[Header("Debuff Durations")]
	public int burnedDuration;
	public int cursedDuration;
	public int staticDuration;

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

	public bool IsStatic {
		get { return statics; }
	}

	public bool IsRefreshing {
		get { return refresh; }
	}

	public void AddBleed() {
		if(!player.Debuffs.IsBleeding) {
			bleeding = true;
			Bleed.ActivateDebuff (GamePlayController.instance.currentTurn, 0);
			player.stats.debuffManager.AddDebuffMarker (CardElement.Blood, 0);
		}
	}

	public void RemoveBleed() {
		if (player.Debuffs.IsBleeding) {
			bleeding = false;
			Bleed.DeactivateDebuff ();
			player.stats.debuffManager.RemoveDebuffMarker (CardElement.Blood);
		}
	}

	public void AddBlind() {
		if (!player.Debuffs.IsBlind) {
			blinded = true;
			Blind.ActivateDebuff (GamePlayController.instance.currentTurn, 0);
			player.stats.debuffManager.AddDebuffMarker (CardElement.Light, 0);
		}
	}

	public void RemoveBlind() {
		if (player.Debuffs.IsBlind) {
			blinded = false;
			Blind.DeactivateDebuff ();
			player.stats.debuffManager.RemoveDebuffMarker (CardElement.Light);
		}
	}

	public void AddBurn(int duration) {
		if (!player.Debuffs.IsBurned) {
			burned = true;
			burnedDuration = duration;
			Burn.ActivateDebuff (GamePlayController.instance.currentTurn, duration);
			player.stats.debuffManager.AddDebuffMarker (CardElement.Fire, duration);
		}
	}

	public void RemoveBurn() {
		if (player.Debuffs.IsBurned) {
			burned = false;
			Burn.DeactivateDebuff ();
			player.stats.debuffManager.RemoveDebuffMarker (CardElement.Fire);
		}
	}

	public void AddCurse(int duration) {
		if(!player.Debuffs.IsBleeding) {
			cursed = true;
			cursedDuration = duration;
			Curse.ActivateDebuff (GamePlayController.instance.currentTurn, duration);
			player.stats.debuffManager.AddDebuffMarker (CardElement.Shadow, duration);
		}
	}

	public void RemoveCurse() {
		if (player.Debuffs.IsCursed) {
			cursed = false;
			Curse.DeactivateDebuff ();
			player.stats.debuffManager.RemoveDebuffMarker (CardElement.Shadow);
		}
	}

	public void AddFreeze() {		
		if(!player.Debuffs.IsFrozen) {
			frozen = true;
			Freeze.ActivateDebuff (GamePlayController.instance.currentTurn, 0);
			player.stats.debuffManager.AddDebuffMarker (CardElement.Ice, 0);
		}
	}

	public void RemoveFreeze() {
		if(player.Debuffs.IsFrozen) {
			frozen = false;
			Freeze.DeactivateDebuff ();
			player.stats.debuffManager.RemoveDebuffMarker (CardElement.Ice);
		}
	}

	public void AddKnockDown() {
		if (!player.Debuffs.IsKnockedDown) {
			knockedDown = true;
			KnockedDown.ActivateDebuff (GamePlayController.instance.currentTurn, 0);
			player.stats.debuffManager.AddDebuffMarker (CardElement.Earth, 0);
			player.skipNextTurn = true;
		}
	}

	public void RemoveKnockDown() {
		if(player.Debuffs.IsKnockedDown) {
			knockedDown = false;
			KnockedDown.DeactivateDebuff ();
			player.stats.debuffManager.RemoveDebuffMarker (CardElement.Earth);
		}
	}

	public void AddPoison() {
		if (!player.Debuffs.IsPoisoned) {
			poisoned = true;
			Poison.ActivateDebuff (GamePlayController.instance.currentTurn, 0);
			player.stats.debuffManager.AddDebuffMarker (CardElement.Nature, 0);
		}
	}

	public void RemovePoison() {
		if(player.Debuffs.IsPoisoned) {
			poisoned = false;
			Poison.DeactivateDebuff ();
			player.stats.debuffManager.RemoveDebuffMarker (CardElement.Nature);
		}
	}

	public void AddWet() {
		if (!player.Debuffs.IsWet) {
			wet = true;
			Wet.ActivateDebuff (GamePlayController.instance.currentTurn, 0);
			player.stats.debuffManager.AddDebuffMarker (CardElement.Water, 0);
		}
	}

	public void RemoveWet() {
		if(player.Debuffs.IsWet) {
			wet = false;
			Wet.DeactivateDebuff ();
			player.stats.debuffManager.RemoveDebuffMarker (CardElement.Water);
		}
	}

	public void AddStatics(int duration) {
		if (!player.Debuffs.IsStatic) {
			statics = true;
			staticDuration = duration;
			Static.ActivateDebuff (GamePlayController.instance.currentTurn, duration);
			player.stats.debuffManager.AddBuffMarker (CardElement.Lightning, duration);
		} else {
			statics = true;
			staticDuration = duration;
			Static.ActivateDebuff (GamePlayController.instance.currentTurn, duration);
			player.stats.debuffManager.ResetBuffMarker (CardElement.Lightning, duration);
		}
	}

	public void RemoveStatics() {
		if(player.Debuffs.IsStatic) {
			statics = false;
			Static.DeactivateDebuff ();
		}
	}

	public void AddRefresh() {
		if (!player.Debuffs.IsRefreshing) {
			refresh = true;
			Refresh.ActivateDebuff (GamePlayController.instance.currentTurn, 0);
			player.stats.debuffManager.AddBuffMarker (CardElement.Water, 0);
		}
	}

	public void RemoveRefresh() {
		if(player.Debuffs.IsRefreshing) {
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
