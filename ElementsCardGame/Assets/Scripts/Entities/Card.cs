using System;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class Card : MonoBehaviour {
	[Header("Properties")]
	public CardElement element;
	public CardType type;
	public CardState state = CardState.OnDeck;
	public SpellSelection selectedSpell;

	[Header("Mixed Requirements")]
	public CardElement elementA;
	public CardElement elementB;

	private bool underMovement;

	[Header("Morphing Target")]
	public CardElement targetElement; 
	public Sprite newFrontSprite;
	public Sprite newSpriteAura;

	[Header("Conditions")]
	public bool zoomed;
	public bool selected;
	public bool wildCard;

	public bool Zoomed {
		get { return zoomed; }
	}
		
	public bool Selected {
		get { return selected; }
	}

	public Sprite FrontImage {
		set { mySpriteRenderer.sprite = value; }
	}

	public SpriteRenderer mySpriteRenderer;
	public SpriteRenderer mySelection;
	public Animator myAnimator;

	public bool IsElementCard() {
		return type.Equals (CardType.Element);
	}

	public bool IsMixedCard() {
		return type.Equals (CardType.Mixed);
	}

	public bool IsWildCard() {
		return type.Equals (CardType.Wild);
	}

	public void UpdatePosition(Vector3 p) {
		transform.position = p;
	}

	public Vector3 GetCoordenates() {
		return transform.position;
	}

	public void ActivateAnimator() {
		myAnimator.enabled = true;
	}

	public void DeactivateAnimator() {
		myAnimator.enabled = false;
	}

	public void ChangeToOnDeckState() {
		state = CardState.OnDeck;
	}

	public void ChangeToMixedSelectionState() {
		state = CardState.MixedSelection;
	}

	public void ChangeToInHandState() {
		state = CardState.InHand;	
	}

	public void ChangeToInGameState() {
		state = CardState.InGame;	
	}

	public void ChangeToSavedState() {
		state = CardState.Saved;	
	}

	public void ChangeToDiscardedState() {
		state = CardState.Discarded;	
	}

	public void ChangeToWaitingToMixState() {
		state = CardState.WaitingToMix;
	}

	public void ChangeToMorphingState() {
		state = CardState.Morphing;
	}

	public bool IsCardStateOnDeck() {
		return state.Equals (CardState.OnDeck);
	}

	public bool IsCardStateMixedSelection() {
		return state.Equals (CardState.MixedSelection);
	}

	public bool IsCardStateInHand() {
		return state.Equals (CardState.InHand);
	}

	public bool IsCardStateInGame() {
		return state.Equals (CardState.InGame);
	}

	public bool IsCardStateSaved() {
		return state.Equals (CardState.Saved);
	}

	public bool IsCardStateWaitingToMix() {
		return state.Equals (CardState.WaitingToMix);
	}

	public bool IsCardStateDiscarded() {
		return state.Equals (CardState.Discarded);
	}

	public bool IsRequirementA(CardElement element) {
		return elementA != null && element.Equals(elementA);
	}

	public bool IsRequirementB(CardElement element) {
		return elementB != null && element.Equals(elementB);
	}

	public void SelectCard() {
		if(!selected) {
			selected = true;
			mySelection.gameObject.SetActive (true);
			mySelection.enabled = true;
			ZoomIn ();
		}
	}

	public void DeselectCard() {
		if (selected) {
			selected = false;
			mySelection.enabled = false;
			mySelection.gameObject.SetActive (false);
			ZoomOut ();
		}
	}

	public void PutIntoGameAsLocalPlayer(SpellSelection selectedSpell) {
		DeselectCard ();
		PutCardIntoGameAsLocalPlayer();
		ChangeToInGameState ();
		this.selectedSpell = selectedSpell;

		GamePlayController.instance.PutAiOpponentCardIntoGame ();
	}

	public void PutIntoWaitingToMixState() {
		DeselectCard ();

		GUIController.instance.HideInteractionBlocker ();

		MoveCardToTargetPositionWithITween (
			"easeOutCubic", GUIController.instance.GetMixedCardBaseTransformedPosition (), 1f, 0.25f,
			IsCardStateMixedSelection (), "EndPutToMixedCardPile"
		);

		ScaleCardToTargetSizeWithITween ("linear", new Vector3 (0.25f, 0.25f, 1), 1f, 0.25f, IsCardStateMixedSelection (), "");
		RotateCardToTargetAngleWithITween ("linear", Vector3.zero, 0.8f, 0.25f, IsCardStateMixedSelection (), "");

		ChangeToWaitingToMixState ();
	}

	public void PutIntoDiscardedState() {
		MoveCardToTargetPositionWithITween (
			"easeOutCubic", GUIController.instance.GetDiscardBaseTransformedPosition (), 1f, 0.25f,
			IsCardStateInGame (), "EndPutToDiscardedCardPile"
		);

		ScaleCardToTargetSizeWithITween ("linear", new Vector3 (0.25f, 0.25f, 1), 1f, 0.25f, IsCardStateInGame (), "");
		RotateCardToTargetAngleWithITween ("linear", Vector3.zero, 0.8f, 0.25f, IsCardStateInGame (), "");
	}

	public void PutToSavedState() {
		DeselectCard ();

		GUIController.instance.HideInteractionBlocker ();

		MoveCardToTargetPositionWithITween ("easeOutCubic", GUIController.instance.GetSaveBaseTransformedPosition (), 1f, 0.25f,
			IsCardStateInHand (), "EndPutToSavedCardPile"
		);

		ScaleCardToTargetSizeWithITween ("linear", new Vector3 (0.25f, 0.25f, 1), 1f, 0.25f, IsCardStateInHand (), "");
		RotateCardToTargetAngleWithITween ("linear", Vector3.zero, 0.8f, 0.25f, IsCardStateInHand (), "");

		ChangeToSavedState ();
	}

	public void MoveMeToCenter () {
		myAnimator.enabled = false;

		MoveCardToTargetPositionWithITween ("easeInQuint", Vector3.zero, 1f, 0, true, "Float");
		ScaleCardToTargetSizeWithITween ("easeInQuint", new Vector3(1, 1, 1), 1f, 0, true, "");
		RotateCardToTargetAngleWithITween ("easeInQuint", new Vector3(0, 180, 0), 0.8f, 0, true, "");

		SoundManager.instance.PlayCardMoveSound ();
	}

	public void PutToCenter() {
		state = CardState.InHand;
		Float ();
	}

	public void MoveFromInHandToMorphing() {
		if(state.Equals(CardState.InHand) && type.Equals(CardType.Wild)) {
			myAnimator.enabled = false;
			state = CardState.Morphing;

			RotateCardToTargetAngleWithITween ("linear", new Vector3(0, 1080, 0), 1.5f, 0, true, "FinishMorph", "none");
			ScaleCardToTargetSizeWithITween ("linear", new Vector3(1, 1, 0.01f), 0.8f, 0.5f, true, "", "pingPong");

			SoundManager.instance.PlayCardMoveSound ();
		}
	}

	public void MoveFromInHandToRightAndStartMixing() {
		if(state.Equals(CardState.InHand)) {
			myAnimator.enabled = false;
			state = CardState.Mixing;

			MoveCardToTargetPositionWithITween ("easeInQuint", new Vector3(2.7f, 0, 0), 0.8f, 0, true, "");
			ScaleCardToTargetSizeWithITween ("easeInQuint", new Vector3(1, 1, 1), 1f, 0, true, "");

			SoundManager.instance.PlayCardMoveSound ();
		}
	}

	public void MoveFromSavedToLeftAndStartMixing() {
		if(state.Equals(CardState.Saved)) {
			myAnimator.enabled = false;
			state = CardState.Mixing;

			MoveCardToTargetPositionWithITween ("easeInQuint", new Vector3(-2.7f, 0, 0), 1f, 0, true, "");
			ScaleCardToTargetSizeWithITween ("easeInQuint", new Vector3(1, 1, 1), 1f, 0, true, "");
			RotateCardToTargetAngleWithITween ("easeInQuint", new Vector3(0, 180, 0), 0.8f, 0, true, "");

			SoundManager.instance.PlayCardMoveSound ();
		}
	}

	private void FinishMorph() {
		this.mySpriteRenderer.sprite = newFrontSprite;
		this.mySelection.sprite = newSpriteAura;
		this.element = targetElement;
		this.type = CardType.Element;
		wildCard = false;
		ChangeToInHandState ();
	}

	private void EndPutToMixedCardPile() {
		GamePlayController.instance.EndPutToMixedCardPile ();
	}

	private void EndPutToDiscardedCardPile() {
		GUIController.instance.ShowDiscardedCardButton ();

		ChangeToDiscardedState ();
		GamePlayController.instance.EndPutToDiscardedCardPile ();

		gameObject.SetActive (false);
	}

	private void EndPutToSavedCardPile() {
		GamePlayController.instance.EndPutToSavedCardPile ();
	}

	public void Float() {
		if(myAnimator != null) {
			myAnimator.enabled = true;
			myAnimator.Play ("Floating");
		}
	}

	public void MoveBack() {
		if(myAnimator != null) {
			myAnimator.enabled = true;
			myAnimator.Play ("MoveBack");
			SoundManager.instance.PlayCardMoveSound ();
		}
	}

	public void LocalPlayerReveal() {
		if(myAnimator != null) {
			myAnimator.enabled = true;
			myAnimator.Play ("LocalPlayerReveal");
			SoundManager.instance.PlayCardMoveSound ();
		}
	}

	public void FadeIn() {
		if(myAnimator != null) {
			myAnimator.enabled = true;
			myAnimator.Play ("FadeIn");
		}
	}

	public void EnemyReveal() {
		if(myAnimator != null) {
			myAnimator.enabled = true;
			myAnimator.Play ("EnemyReveal");
			SoundManager.instance.PlayCardMoveSound ();
		}
	}

	public void ScaleOut() {
		ScaleCardToTargetSizeWithITween ("linear", new Vector3 (0, 0, 1), 0.5f, 0, IsCardStateInGame (), "");
	}

	public void DoWildCardElementTransition(CardElement element, Sprite newFrontSprite, Sprite newSpriteAura) {
		if(type.Equals(CardType.Wild)) {
			this.targetElement = element;
			this.newFrontSprite = newFrontSprite;
			this.newSpriteAura = newSpriteAura;

			DeselectCard ();
			MoveFromInHandToMorphing ();
		}
	}

	private void PutCardIntoGameAsLocalPlayer() {
		if(myAnimator != null) {
			myAnimator.enabled = true;
			myAnimator.Play ("PutIntoGameAsLocalPlayer");
			SoundManager.instance.PlayCardMoveSound ();
		}
	}

	private void ZoomIn() {
		if(myAnimator != null && !Zoomed) {
			zoomed = true;
			myAnimator.enabled = true;
			myAnimator.Play ("ZoomIn");
			SoundManager.instance.PlayCardMoveSound ();
		}
	}

	private void ZoomOut() {
		if(myAnimator != null && Zoomed) {
			zoomed = false;
			myAnimator.enabled = true;
			myAnimator.Play ("ZoomOut");
			SoundManager.instance.PlayCardMoveSound ();
		}
	}

	private void MoveCardToTargetPositionWithITween(string easing, Vector3 target, float timer, float delay, bool condition, string doAfter)  {
		if(condition) {
			myAnimator.enabled = false;

			Hashtable hash = 
				iTween.Hash (
					"position", target, 
					"easetype", easing,
					"delay", delay,
					"looptype", "none", 
					"oncomplete", doAfter,
					"time", timer
				);

			iTween.MoveTo (gameObject, hash);
		}
	}

	private void ScaleCardToTargetSizeWithITween(string easing, Vector3 target, float timer, float delay, bool condition, string doAfter, string loopType = "none") {
		if (condition) {
			myAnimator.enabled = false;

			Hashtable hash = 
				iTween.Hash (
					"scale", target, 
					"easetype", easing,
					"delay", delay,
					"looptype", loopType, 
					"oncomplete", doAfter,
					"time", timer
				);

			iTween.ScaleTo (gameObject, hash);
		}
	}

		private void RotateCardToTargetAngleWithITween(string easing, Vector3 target, float timer, float delay, bool condition, string doAfter, string loopType = "none") {
		if (condition) {
			myAnimator.enabled = false;

			Hashtable hash = 
				iTween.Hash (
					"rotation", target,
					"easetype", easing,
					"delay", delay,
					"looptype", loopType,
					"oncomplete", doAfter,
					"time", timer
				);

			iTween.RotateTo (gameObject, hash);
		}
	}
}