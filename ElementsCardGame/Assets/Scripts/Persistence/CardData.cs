using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class CardData {
	public CardElement cardElement;
	public int amount;
	public CardType type;

	public CardData(CardElement cardElement, int amount, CardType type) {
		this.cardElement = cardElement;
		this.amount = amount;
		this.type = type;
	}
}