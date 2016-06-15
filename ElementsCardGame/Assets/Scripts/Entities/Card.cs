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

	private bool underMovement;

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

		GamePlayController.instance.PutCardIntoGame ();
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

	private void EndPutToMixedCardPile() { 
		GamePlayController.instance.EndPutToMixedCardPile ();
	}

	private void EndPutToDiscardedCardPile() {
		GamePlayController.instance.EndPutToDiscardedCardPile ();
		GUIController.instance.ShowDiscardedCardButton ();

		ChangeToDiscardedState ();

		gameObject.SetActive (false);
	}

	private void EndPutToSavedCardPile() {
		GamePlayController.instance.EndPutToSavedCardPile ();
	}

	public void Float() {
		if(myAnimator != null) {
			myAnimator.enabled = true;
			myAnimator.Play ("Floating");
			SoundManager.instance.PlayCardMoveSound ();
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

	private void ScaleCardToTargetSizeWithITween(string easing, Vector3 target, float timer, float delay, bool condition, string doAfter) {
		if (condition) {
			myAnimator.enabled = false;

			Hashtable hash = 
				iTween.Hash (
					"scale", target, 
					"easetype", easing,
					"delay", delay,
					"looptype", "none", 
					"oncomplete", doAfter,
					"time", timer
				);

			iTween.ScaleTo (gameObject, hash);
		}
	}

	private void RotateCardToTargetAngleWithITween(string easing, Vector3 target, float timer, float delay, bool condition, string doAfter) {
		if (condition) {
			myAnimator.enabled = false;

			Hashtable hash = 
				iTween.Hash (
					"rotation", target, 
					"easetype", easing,
					"delay", delay,
					"looptype", "none", 
					"oncomplete", doAfter,
					"time", timer
				);

			iTween.RotateTo (gameObject, hash);
		}
	}
}