using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class DeckData {
	private const int DECK_SIZE_LIMIT = 32;

	public string deckName;
	private List<CardData> cards;
	public List<CardData> Cards {
		get {  return cards; }
	}
	public int mixedCardIndex = -1;
	public int wildCardIndex = -1;

	public int totalCards = 0;

	public int changedElementIndex = -1;

	public DeckData(string deckName) {
		this.deckName = deckName;
		this.cards = new List<CardData> (DECK_SIZE_LIMIT);
	}

	public bool IsDeckComplete {
		get { 
			return totalCards == DECK_SIZE_LIMIT;
		}
	}

	public bool Empty {
		get { return cards.Count > 0; }
	}

	public void AddCard(CardData newCardData) {
		totalCards += newCardData.amount;

		if (newCardData.type.Equals (CardType.Element)) {
			changedElementIndex = 0;
			CardData card;

			for(int i = 0; i < cards.Count; i++) {
				card = cards [i];
				if (card.cardElement.Equals (newCardData.cardElement)) {
					changedElementIndex = i;
					card.amount++;
					return;
				} else {
					changedElementIndex = i + 1;
				}
			}
			cards.Add (newCardData);
		} else if (newCardData.type.Equals (CardType.Mixed)) {
			if(mixedCardIndex == -1) {
				cards.Add (newCardData);
				mixedCardIndex = cards.IndexOf (newCardData);
				changedElementIndex = mixedCardIndex;
			}else {
				cards [mixedCardIndex] = newCardData;
			}
		} else if (newCardData.type.Equals (CardType.Wild)) {
			if (wildCardIndex == -1) {
				cards.Add (newCardData);
				wildCardIndex = cards.IndexOf (newCardData);
				changedElementIndex = wildCardIndex;
			}					
		}
	}

	public void RemoveCard(int index) {
		CardType type = cards [index].type;
		CardElement element = cards [index].cardElement;

		int indexToRemove = -1;

		if(cards.Count > 0 && cards.Count >= index) {
			if (type.Equals (CardType.Element)) {				
				int counter = 0;
				foreach(CardData cd in cards) {
					if (cd.cardElement.Equals (element)) {
						if(cd.amount > 0) {
							cd.amount--;
							if(cd.amount == 0) {
								indexToRemove = counter;
							}
						}
						break;
					}
					counter++;
				}
			} else if (type.Equals (CardType.Mixed)) {
				if(mixedCardIndex > -1) {
					indexToRemove = mixedCardIndex;
					mixedCardIndex = -1;

					if(wildCardIndex > -1 && wildCardIndex > indexToRemove){
						wildCardIndex--;
					}
				}
			} else if (type.Equals (CardType.Wild)) {
				if(wildCardIndex > -1) {
					indexToRemove = wildCardIndex;
					wildCardIndex = -1;

					if(mixedCardIndex > -1 && mixedCardIndex > indexToRemove) {
						mixedCardIndex--;
					}
				}
			}

			if(indexToRemove > -1) {
				cards.RemoveAt (indexToRemove);
			}
		}

		totalCards--;
	}
}