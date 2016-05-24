using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Deck {
	private const int DECK_MAX_SIZE = 32;

	public int mixedCardIndex = -1;

	private int currentIndex;

	private int size;
	public int Size {
		get { return size; }
	}

	public string deckName;
	public string DeckName {
		get { return deckName; }
		set { deckName = value; }
	}

	public List<Card> cards;
	public List<Card> Cards {
		get { return cards; }
	}

	public Deck(string deckName) {
		this.cards = new List<Card> (DECK_MAX_SIZE);
		this.deckName = deckName;
	}

	//The right way of adding cards to a deck, because it increases the deck size properly
	public void AddCard(Card card) {
		Cards.Add (card);
		size++;
	}

	public Card GetNextCard() {
		if(HasNext()) {
			int index = currentIndex;
			currentIndex++;

			return cards [index];
		}

		return null;
	}

	public Card GetMixedCardFromDeck() {
		if(mixedCardIndex >= 0) {
			Card mixed = cards [mixedCardIndex];

			cards.RemoveAt (mixedCardIndex);

			return mixed;
		}

		return null;
	}

	public bool HasNext() {
		return currentIndex < cards.Count;
	}
}