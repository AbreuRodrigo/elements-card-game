using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {
	[Header("Properties")]
	public string playerName;
	public int HP = 80;
	public bool localPlayer;
	public bool isAI;
	public bool goesFirst;
	public Card currentCard;
	public Card savedCard;
	public Card mixedCard;
	public Card hiddenCard;
	public PlayerStats stats;
	public RectTransform shieldRectTransform;
	public SpellHistoryData lastSpellCasted;
	public GameObject attackProtection;
	public SpellType protectionType;
	public Player opponent;
	public int lastDamageReceived;
	public bool skipNextTurn;
	public bool skipThisTurn;

	[SerializeField]
	[Header("Debuffs")]
	private PlayerDebuffs debuffs;
	public PlayerDebuffs Debuffs {
		get { return debuffs; }
	}

	void Awake() {
		InitializePlayerStats ();
		debuffs = new PlayerDebuffs (this);
	}

	[Header("Current Deck")]
	public List<Card> cards;

	public Deck deck;
	public Deck Deck {
		get { return deck; }
		set { 
			deck = value;

			if (deck != null) {
				cards = deck.Cards;
			}
		}
	}

	public bool HasBuff {
		get {
			return Debuffs.IsStatic || Debuffs.IsRefreshing;
		}
	}

	public bool HasSavedCard {
		get { 
			return savedCard != null;
		}
	}

	public bool HasAttackProtection {
		get {
			return attackProtection != null;
		}
	}

	public void HideCurrentCard() {
		if(currentCard != null) {
			currentCard.gameObject.SetActive (false);
		}
	}

	public void ShowCurrentCard() {
		if(currentCard != null) {
			currentCard.gameObject.SetActive (true);
		}
	}

	public void DecreaseHP(int amount) {
		if (HP > 0) {
			lastDamageReceived = amount;
			HP -= amount;
			stats.nextHPAlteration = amount * -1;
		}
	}

	public void IncreaseHP(int amount) {
		if (HP > 0 && HP < 80) {
			int diff = 80 - HP;

			if(amount > diff) {
				amount = diff;
			}

			HP += amount;
			stats.nextHPAlteration = amount;
			stats.UpdateHP ();
		}
	}

	public void EndPlayerTurn() {
		if (Debuffs != null) {
			Debuffs.ExecuteDebuffsIfActive ();
			stats.UpdateHP ();
		}
	}

	public bool WasLastSpellMelee() {
		return lastSpellCasted != null && lastSpellCasted.SpellType.Equals (SpellType.Melee);
	}

	public bool WasLastSpellSpecial() {
		return lastSpellCasted != null && lastSpellCasted.SpellType.Equals (SpellType.Special);
	}

	public bool IsBleeding() {
		return debuffs != null && debuffs.IsBleeding;
	}

	public bool IsBlind() {
		return debuffs != null && debuffs.IsBlind;
	}

	public bool IsBurned() {
		return debuffs != null && debuffs.IsBurned;
	}

	public bool IsCursed() {
		return debuffs != null && debuffs.IsCursed;
	}

	public bool IsFrozen() {
		return debuffs != null && debuffs.IsFrozen;
	}

	public bool IsKnockedDown() {
		return debuffs != null && debuffs.IsKnockedDown;
	}

	public bool IsPoisoned() {
		return debuffs != null && debuffs.IsPoisoned;
	}

	public bool IsWet() {
		return debuffs != null && debuffs.IsWet;
	}

	public bool ISRefreshing() {
		return debuffs != null && debuffs.IsRefreshing;
	}

	public bool IsStatic() {
		return debuffs != null && debuffs.IsStatic;
	}

	public void DecreaseBleedDebuff() {
		stats.debuffManager.bleedDebuff.DecreaseCounter ();
	}

	public void DecreaseBurnDebuff() {
		stats.debuffManager.burnDebuff.DecreaseCounter ();
	}

	public void DecreaseBlindBuff() {
		stats.debuffManager.blindDebuff.DecreaseCounter ();
	}

	public void DecreaseCurseDebuff() {
		stats.debuffManager.curseDebuff.DecreaseCounter ();
	}

	public void DecreaseFreezeBuff() {
		stats.debuffManager.freezeDebuff.DecreaseCounter ();
	}

	public void DecreasePoisonDebuff() {
		stats.debuffManager.poisonDebuff.DecreaseCounter ();
	}

	public void DecreaseWetBuff() {
		stats.debuffManager.wetDebuff.DecreaseCounter ();
	}

	public void DecreaseStaticBuff() {
		stats.debuffManager.staticBuff.DecreaseCounter ();
	}

	public void DecreaseRefreshBuff() {
		stats.debuffManager.refreshBuff.DecreaseCounter ();
	}


	/*****HIDE DEBUFFS ON ZERO TURN*****/

	public void HideBleedDebuffOnZeroTurnCounters(int remainingTurns) {
		if(remainingTurns <= 0) {
			stats.debuffManager.bleedDebuff.Hide ();
			//stats.debuffManager.RemoveDebuffMarker (CardElement.Blood);
			debuffs.RemoveBleed ();
		}
	}

	public void HideBurnDebuffOnZeroTurnCounters(int remainingTurns) {
		if(remainingTurns <= 0) {
			stats.debuffManager.burnDebuff.Hide ();
			//stats.debuffManager.RemoveDebuffMarker (CardElement.Fire);
			debuffs.RemoveBurn ();
		}
	}

	public void HideBlindDebuffOnZeroTurnCounters(int remainingTurns) {
		if(remainingTurns <= 0) {
			stats.debuffManager.blindDebuff.Hide ();
			//stats.debuffManager.RemoveDebuffMarker (CardElement.Light);
			debuffs.RemoveBlind ();
		}
	}

	public void HideCurseDebuffOnZeroTurnCounters(int remainingTurns) {
		if(remainingTurns <= 0) {
			stats.debuffManager.curseDebuff.Hide ();
			//stats.debuffManager.RemoveDebuffMarker (CardElement.Shadow);
			debuffs.RemoveCurse ();
		}
	}

	public void HideFreezeDebuffOnZeroTurnCounters(int remainingTurns) {
		if(remainingTurns <= 0) {
			stats.debuffManager.freezeDebuff.Hide ();
			//stats.debuffManager.RemoveDebuffMarker (CardElement.Ice);
			debuffs.RemoveFreeze ();
		}
	}

	public void HidePoisonDebuffOnZeroTurnCounters(int remainingTurns) {
		if(remainingTurns <= 0) {
			stats.debuffManager.poisonDebuff.Hide ();
			//stats.debuffManager.RemoveDebuffMarker (CardElement.Nature);
			debuffs.RemovePoison ();
		}
	}

	public void HideWetDebuffOnZeroTurnCounters(int remainingTurns) {
		if(remainingTurns <= 0) {
			stats.debuffManager.wetDebuff.Hide ();
			//stats.debuffManager.RemoveDebuffMarker (CardElement.Water);
			debuffs.RemoveWet ();
		}
	}

	/*****HIDE BUFFS ON ZERO TURN*****/

	public void HideStaticBuffOnZeroTurnCounters(int remainingTurns) {
		if(remainingTurns <= 0) {
			stats.debuffManager.staticBuff.Hide ();
			//stats.debuffManager.RemoveBuffMarker (CardElement.Lightning);
			debuffs.RemoveStatics ();
		}
	}

	public void HideRefreshBuffOnZeroTurnCounters(int remainingTurns) {
		if(remainingTurns <= 0) {
			stats.debuffManager.refreshBuff.Hide ();
			//stats.debuffManager.RemoveBuffMarker (CardElement.Water);
			debuffs.RemoveRefresh ();
		}
	}

	private void InitializePlayerStats() {
		UpdateStatsName ();
		UpdateStatsHP ();
	}

	private void UpdateStatsName() {
		if (stats != null) {
			stats.UpdateName (playerName);
		}
	}

	private void UpdateStatsHP() {
		if (stats != null) {
			stats.UpdateHP ();
		}
	}
}