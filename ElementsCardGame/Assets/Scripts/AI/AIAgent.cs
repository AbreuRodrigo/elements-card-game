using UnityEngine;
using System.Collections;

public class AIAgent : MonoBehaviour {
	public Player aiPlayer;

	public Vector3 inGamePlayPoint;

	public void AIDrawTopCardFromDeck() {
		if(aiPlayer != null) {
			Card card = aiPlayer.Deck.GetNextCard ();

			if (card != null) {
				card.ChangeToInHandState ();
				aiPlayer.currentCard = card;
			}
		}

		StartCoroutine (TakeDecision ());
	}

	public void AIDrawMixedCardFromDeck() {
		if(aiPlayer != null) {
			aiPlayer.mixedCard = aiPlayer.Deck.GetMixedCardFromDeck ();
			aiPlayer.currentCard = aiPlayer.mixedCard;

			if(aiPlayer.mixedCard != null) {
				aiPlayer.mixedCard.ChangeToWaitingToMixState ();
			}
		}
	}

	IEnumerator TakeDecision() {
		yield return new WaitForSeconds (1);

		PlayCardIntoGame ();
	}

	private void PlayCardIntoGame() {
		aiPlayer.currentCard.selectedSpell = SelectSpellRandomly ();
		aiPlayer.currentCard.ChangeToInGameState ();
		StartCoroutine (MoveCardToGamePlayPosition (aiPlayer.currentCard));
	}

	private SpellSelection SelectSpellRandomly() {
		int limit = 3;

		if(aiPlayer.currentCard.element.Equals(CardElement.Dark)) {
			limit = 2;
		}

		int decision = Random.Range (0, limit);

		if(decision == 0) {
			return SpellSelection.Rhombus;
		}else if(decision == 1) {
			return SpellSelection.Square;
		}

		return SpellSelection.Circle;
	}

	IEnumerator MoveCardToGamePlayPosition(Card card) {
		card.myAnimator.enabled = false;
		card.transform.localScale = Vector3.zero;
		card.gameObject.SetActive (true);

		Vector3 startPosition = Vector3.zero;
		Vector3 finalScale = new Vector3 (0.6f, 0.6f, 1);
		Vector3 speedT = new Vector3 (1, 1, 0);
		Vector3 speedS = new Vector3 (1, 1, 0);

		float animationSpeed = 0.2f;

		GamePlayController.instance.ShowRedPlayerArcaneCircle ();

		while(card.transform.position != inGamePlayPoint) {
			card.transform.position = Vector3.SmoothDamp (card.transform.position, inGamePlayPoint, ref speedT, animationSpeed);
			card.transform.localScale = Vector3.SmoothDamp (card.transform.localScale, finalScale, ref speedS, animationSpeed);

			if(Vector3.Distance(card.transform.position, inGamePlayPoint) <= 0.25f) {
				card.transform.position = inGamePlayPoint;
				card.transform.localScale = finalScale;
			}

			yield return new WaitForSeconds (0.01f);
		}
	}
}