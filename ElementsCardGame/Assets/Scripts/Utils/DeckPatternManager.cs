using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DeckPatternManager : MonoBehaviour {
	public static DeckPatternManager instance;

	void Awake() {
		if(instance == null) {
			instance = this;
		}
	}

	public void BuildDefaultDeck1(Player player) {
		if (player != null && player.deck == null) {
			player.Deck = new Deck ("DefaultDeck1");

			DeckManager.instance.AddCardsToDeck (player, CardElement.Blood, CardType.Element, 3);
			DeckManager.instance.AddCardsToDeck (player, CardElement.Fire, CardType.Element, 3);
			DeckManager.instance.AddCardsToDeck (player, CardElement.Lightning, CardType.Element, 3);
			DeckManager.instance.AddCardsToDeck (player, CardElement.Nature, CardType.Element, 3);
			DeckManager.instance.AddCardsToDeck (player, CardElement.Ice, CardType.Element, 3);
			DeckManager.instance.AddCardsToDeck (player, CardElement.Water, CardType.Element, 3);
			DeckManager.instance.AddCardsToDeck (player, CardElement.Shadow, CardType.Element, 3);
			DeckManager.instance.AddCardsToDeck (player, CardElement.Earth, CardType.Element, 3);
			DeckManager.instance.AddCardsToDeck (player, CardElement.Light, CardType.Element, 3);
			DeckManager.instance.AddCardsToDeck (player, CardElement.Dark, CardType.Element, 3);

			DeckManager.instance.AddCardsToDeck (player, CardElement.Magma, CardType.Mixed, 1);
			DeckManager.instance.AddCardsToDeck (player, player.localPlayer ?  CardElement.Wild : RandomizeWildCard(), CardType.Wild, 1);
		}
	}

	public void BuildDeckInUse(Player player) {
		if (player != null && player.deck == null) {
			int deckInUseId = PersistenceManager.Instance.GetPlayerDeckInUse ();

			if (deckInUseId != 0) {
				deckInUseId += 1;

				DeckData deckInUse = (DeckData)PersistenceManager.Instance.LoadData ("dk" + deckInUseId);

				player.Deck = new Deck (deckInUse.deckName);

				foreach (CardData c in deckInUse.Cards) {
					DeckManager.instance.AddCardsToDeck (player, c.cardElement, c.type, c.amount);
				}
			} else {
				BuildDefaultDeck1 (player);
			}
		}
	}

	public void BuildOnlyBloodDeck(Player player) {
		if (player != null && player.deck == null) {
			player.Deck = new Deck ("OnlyBlood");
			DeckManager.instance.AddCardsToDeck (player, CardElement.Blood, CardType.Element, 31);
			DeckManager.instance.AddCardsToDeck (player, CardElement.Zombie, CardType.Mixed, 1);
		}
	}

	public void BuildMagmaDeck(Player player) {
		if (player != null && player.deck == null) {
			player.Deck = new Deck ("MagmaDeck");
			DeckManager.instance.AddCardsToDeck (player, CardElement.Earth, CardType.Element, 15);
			DeckManager.instance.AddCardsToDeck (player, CardElement.Fire, CardType.Element, 15);
			DeckManager.instance.AddCardsToDeck (player, CardElement.Magma, CardType.Mixed, 1);
			DeckManager.instance.AddCardsToDeck (player, player.localPlayer ? RandomizeWildCard () : RandomizeWildCard (), CardType.Wild, 1);
		}
	}

	public void BuildChaosDeck(Player player) {
		if (player != null && player.deck == null) {
			player.Deck = new Deck ("ChaosDeck");
			DeckManager.instance.AddCardsToDeck (player, CardElement.Shadow, CardType.Element, 15);
			DeckManager.instance.AddCardsToDeck (player, CardElement.Fire, CardType.Element, 15);
			DeckManager.instance.AddCardsToDeck (player, CardElement.Chaos, CardType.Mixed, 1);
			DeckManager.instance.AddCardsToDeck (player, player.localPlayer ? RandomizeWildCard () : RandomizeWildCard (), CardType.Wild, 1);
		}
	}

	public void BuildWaterDeck(Player player) {
		if (player != null && player.deck == null) {
			player.Deck = new Deck ("WaterDeck");
			DeckManager.instance.AddCardsToDeck (player, CardElement.Water, CardType.Element, 31);
			DeckManager.instance.AddCardsToDeck (player, CardElement.Chaos, CardType.Mixed, 1);
		}
	}

	public void BuildZombieDeck(Player player) {
		if (player != null && player.deck == null) {
			player.Deck = new Deck ("ZombieDeck");
			DeckManager.instance.AddCardsToDeck (player, CardElement.Shadow, CardType.Element, 15);
			DeckManager.instance.AddCardsToDeck (player, CardElement.Blood, CardType.Element, 15);
			DeckManager.instance.AddCardsToDeck (player, CardElement.Zombie, CardType.Mixed, 1);
			DeckManager.instance.AddCardsToDeck (player, player.localPlayer ? RandomizeWildCard () : RandomizeWildCard (), CardType.Wild, 1);
		}
	}

	private CardElement RandomizeMixedCard() {
		int v = Random.Range (0, 3);

		if(v == 0) {
			return CardElement.Zombie;
		}
		if(v == 1) {
			return CardElement.Chaos;
		}

		return CardElement.Magma;
	}

	private CardElement RandomizeWildCard() {
		int i = Random.Range (0, 10);

		System.Array enums = System.Enum.GetValues (typeof(CardElement));

		return (CardElement) enums.GetValue (i);
	}
}