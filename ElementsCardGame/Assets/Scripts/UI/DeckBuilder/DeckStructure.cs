using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DeckStructure : MonoBehaviour {
	public bool hasMixed = false;
	public bool hasWild = false;
	public int amountOfCards = 0;

	public Transform[] cardMarkerSlots;

	public InputField deckName;
	public Text cardCounterUI;
	public Animator cardCounterUIAnimator;

	public string DeckName {
		get { return deckName.text; }
	}

	int bloodCards;
	int waterCards;
	int natureCards;
	int fireCards;
	int shadowCards;
	int darkCards;
	int lightCards;
	int lightningCards;
	int iceCards;
	int earthCards;
	CardElement mixedCard;

	Dictionary<CardElement, System.Action> addCardExecution;
	Dictionary<CardElement, System.Action> removeCardExecution;

	void Awake() {
		addCardExecution = new Dictionary<CardElement, System.Action> (12) {
			{CardElement.Blood, AddBloodCard},
			{CardElement.Dark, AddDarkCard},
			{CardElement.Earth, AddEarthCard},
			{CardElement.Fire, AddFireCard},
			{CardElement.Ice, AddIceCard},
			{CardElement.Light, AddLightCard},
			{CardElement.Lightning, AddLightningCard},
			{CardElement.Nature, AddNatureCard},
			{CardElement.Shadow, AddShadowCard},
			{CardElement.Water, AddWaterCard},
			{CardElement.Magma, AddMixedCard},
			{CardElement.Chaos, AddMixedCard},
			{CardElement.Zombie, AddMixedCard},
			{CardElement.Wild, AddWildCard}
		};
		removeCardExecution = new Dictionary<CardElement, System.Action> (12) {
			{CardElement.Blood, RemoveBloodCard},
			{CardElement.Dark, RemoveDarkCard},
			{CardElement.Earth, RemoveEarthCard},
			{CardElement.Fire, RemoveFireCard},
			{CardElement.Ice, RemoveIceCard},
			{CardElement.Light, RemoveLightCard},
			{CardElement.Lightning, RemoveLightningCard},
			{CardElement.Nature, RemoveNatureCard},
			{CardElement.Shadow, RemoveShadowCard},
			{CardElement.Water, RemoveWaterCard},
			{CardElement.Magma, RemoveMixedCard},
			{CardElement.Chaos, RemoveMixedCard},
			{CardElement.Zombie, RemoveMixedCard},
			{CardElement.Wild, RemoveWildCard}
		};
	}

	public void AddCard(CardElement element, CardType type) {
		if(type.Equals(CardType.Mixed)) {
			mixedCard = element;
		}

		addCardExecution [element].Invoke ();
	}

	public void RemoveCard(CardElement element) {
		removeCardExecution [element].Invoke ();
	}

	public void AddMixedCard() {
		if(!hasMixed) {
			hasMixed = true;
			IncrementTotal ();
		}
	}

	public void AddWildCard() {
		if(!hasWild) {
			hasWild = true;
			IncrementTotal ();
		}
	}

	public void AddBloodCard() {
		bloodCards++;
		IncrementTotal ();
	}

	public void AddWaterCard() {
		waterCards++;
		IncrementTotal ();
	}

	public void AddNatureCard() {
		natureCards++;
		IncrementTotal ();
	}

	public void AddFireCard() {
		fireCards++;
		IncrementTotal ();
	}

	public void AddDarkCard() {
		darkCards++;
		IncrementTotal ();
	}

	public void AddLightCard() {
		lightCards++;
		IncrementTotal ();
	}

	public void AddLightningCard() {
		lightningCards++;
		IncrementTotal ();
	}

	public void AddIceCard() {
		iceCards++;
		IncrementTotal ();
	}

	public void AddEarthCard() {
		earthCards++;
		IncrementTotal ();
	}

	public void AddShadowCard() {
		shadowCards++;
		IncrementTotal ();
	}

	public void RemoveMixedCard() {
		if(hasMixed) {
			hasMixed = false;
			DecrementTotal ();
		}
	}

	public void RemoveWildCard() {
		if(hasWild) {
			hasWild = false;
			DecrementTotal ();
		}
	}

	public void RemoveBloodCard() {
		bloodCards--;
		DecrementTotal ();
	}

	public void RemoveWaterCard() {
		waterCards--;
		DecrementTotal ();
	}

	public void RemoveNatureCard() {
		natureCards--;
		DecrementTotal ();
	}

	public void RemoveFireCard() {
		fireCards--;
		DecrementTotal ();
	}

	public void RemoveDarkCard() {
		darkCards--;
		DecrementTotal ();
	}

	public void RemoveLightCard() {
		lightCards--;
		DecrementTotal ();
	}

	public void RemoveLightningCard() {
		lightningCards--;
		DecrementTotal ();
	}

	public void RemoveIceCard() {
		iceCards--;
		DecrementTotal ();
	}

	public void RemoveEarthCard() {
		earthCards--;
		DecrementTotal ();
	}

	public void RemoveShadowCard() {
		shadowCards--;
		DecrementTotal ();
	}

	public void InitializeDeckStructure(DeckData data) {
		deckName.text = data.deckName;
		cardCounterUI.text = "" + (amountOfCards = data.totalCards);
	}

	//PRIVATE
	private void PulseCardCounterUI() {
		if(cardCounterUI != null && cardCounterUIAnimator != null) {
			cardCounterUI.text = "" + amountOfCards;
			cardCounterUIAnimator.Play ("Pulse");
		}
	}

	private void IncrementTotal() {
		amountOfCards++;
		PulseCardCounterUI ();
	}

	private void DecrementTotal() {
		amountOfCards--;
		PulseCardCounterUI ();
	}
}