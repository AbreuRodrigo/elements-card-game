using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {
	[Header("Properties")]
	public string playerName;
	public int HP = 80;
	public bool localPlayer;
	public bool isAI;
	public PlayerStats stats;
	public bool goesFirst;
	public Card currentCard;
	public Card savedCard;
	public Card mixedCard;

	[SerializeField]
	[Header("Debuffs")]
	private PlayerDebuffs debuffs;
	public PlayerDebuffs Debuffs {
		get { return debuffs; }
	}

	void Awake() {
		InitializePlayerStats ();
		debuffs = new PlayerDebuffs ();
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
			HP -= amount;
			UpdateStatsHP ();
		}
	}

	public void IncreaseHP(int amount) {
		if (HP > 0) {
			HP += amount;
			UpdateStatsHP ();
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
			stats.UpdateHP (HP);
		}
	}
}