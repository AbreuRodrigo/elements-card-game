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
		return lastSpellCasted != null && lastSpellCasted.GetType().Equals (SpellType.Melee);
	}

	public bool WasLastSpellSpecial() {
		return lastSpellCasted != null && lastSpellCasted.GetType().Equals (SpellType.Special);
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