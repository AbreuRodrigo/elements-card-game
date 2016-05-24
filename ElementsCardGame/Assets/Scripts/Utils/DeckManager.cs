using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DeckManager : MonoBehaviour {
	public static DeckManager instance;

	public GameObject cardPrefab;

	public GameObject cardBlood;
	public GameObject cardFire;
	public GameObject cardWater;
	public GameObject cardIce;
	public GameObject cardDark;
	public GameObject cardLight;
	public GameObject cardLightning;
	public GameObject cardEarth;
	public GameObject cardShadow;
	public GameObject cardNature;
	public GameObject cardMagma;
	public GameObject cardZombie;
	public GameObject cardChaos;
	public GameObject cardWild;

	public List<CardReference> cardReference;
	private Dictionary<CardElement, Sprite> references = new Dictionary<CardElement, Sprite>();

	private Vector3 dotpos = Vector3.zero;

	void Awake() {
		if(instance == null) {
			instance = this;

			foreach (CardReference cr in cardReference) {
				references.Add (cr.name, cr.sprite);
			}
		}
	}

	public void ShufflePlayersCurrentDeck(Deck deck) {
		Shuffle (deck.Cards);

		Card card = null;

		for(int i = 0; i < deck.Cards.Count; i++) {
			card = deck.Cards [i];
			if(card.IsMixedCard()) {
				deck.mixedCardIndex = i;
				return;
			}
		}
	}

	public void AddCardsToDeck(Player player, CardElement element, CardType type, int amount) {
		if (player.localPlayer) {
			dotpos = GUIController.instance.GetDeckButtonTransformedPosition ();
		} else {
			dotpos = GUIController.instance.GetOpponentShieldTransformPosition ();
		}

		for (int i = 0; i < amount; i++) {
			Card card = GetCardPrefabByCardName (element, dotpos);

			card.transform.parent = player.transform;
			card.element = element;
			card.type = type;
			card.gameObject.name = element.ToString () + "_Card_" + (i + 1);

			player.Deck.AddCard (card);
		}
	}

	private Card GetCardPrefabByCardName(CardElement name, Vector3 dotPos) {
		switch (name) {
		case CardElement.Blood:
			return ((GameObject)(Instantiate (cardBlood, dotPos, Quaternion.identity))).GetComponent<Card> ();
		case CardElement.Dark:
			return ((GameObject)(Instantiate (cardDark, dotPos, Quaternion.identity))).GetComponent<Card> ();
		case CardElement.Earth:
			return ((GameObject)(Instantiate (cardEarth, dotPos, Quaternion.identity))).GetComponent<Card> ();
		case CardElement.Fire:
			return ((GameObject)(Instantiate (cardFire, dotPos, Quaternion.identity))).GetComponent<Card> ();
		case CardElement.Ice:
			return ((GameObject)(Instantiate (cardIce, dotPos, Quaternion.identity))).GetComponent<Card> ();
		case CardElement.Light:
			return ((GameObject)(Instantiate (cardLight, dotPos, Quaternion.identity))).GetComponent<Card> ();
		case CardElement.Lightning:
			return ((GameObject)(Instantiate (cardLightning, dotPos, Quaternion.identity))).GetComponent<Card> ();
		case CardElement.Nature:
			return ((GameObject)(Instantiate (cardNature, dotPos, Quaternion.identity))).GetComponent<Card> ();
		case CardElement.Shadow:
			return ((GameObject)(Instantiate (cardShadow, dotPos, Quaternion.identity))).GetComponent<Card> ();
		case CardElement.Water:
			return ((GameObject)(Instantiate (cardWater, dotPos, Quaternion.identity))).GetComponent<Card> ();
		case CardElement.Wild:
			return ((GameObject)(Instantiate (cardDark, dotPos, Quaternion.identity))).GetComponent<Card> ();
		case CardElement.Magma:
			return ((GameObject)(Instantiate (cardMagma, dotPos, Quaternion.identity))).GetComponent<Card> ();
		case CardElement.Chaos:
			return ((GameObject)(Instantiate (cardChaos, dotPos, Quaternion.identity))).GetComponent<Card> ();
		case CardElement.Zombie:
			return ((GameObject)(Instantiate (cardZombie, dotPos, Quaternion.identity))).GetComponent<Card> ();
		}

		return null;
	}

	private static void Shuffle<T>(List<T> list) {
		int n = list.Count;

		Random.seed = (int) System.DateTime.Now.Ticks;

		while (n > 1) {
			int k = (Random.Range(0, n) % n);

			T c = list[k];
			n--;
			list[k] = list[n];
			list[n] = c;
		}
	}
}