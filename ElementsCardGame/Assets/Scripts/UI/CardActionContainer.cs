using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CardActionContainer : MonoBehaviour {
	public Sprite greenTrail;
	public Sprite yellowTrail;
	public Sprite redTrail;
	public Sprite darkBlueTrail;
	public Sprite lightBlueTrail;
	public Sprite whiteTrail;
	public Sprite orangeTrail;
	public Sprite blackTrail;
	public Sprite purpleTrail;
	public Sprite brownTrail;

	public Animator myAnimator;

	public Image circleActionTrail;
	public Image squareActionTrail;
	public Image rhombusActionTrail;
	public Image saveActionTrail;
	public Image mixActionTrail;

	public Button circleActionBtn;
	public Button squareActionBtn;
	public Button rhombusActionBtn;
	public Button saveActionBtn;
	public Button mixActionBtn;

	private Dictionary<CardElement, Sprite> spriteTrailByCardName;

	private ActionConditionProcessor actionConditionProcessor;

	public Player currentPlayer;

	void Start() {
		actionConditionProcessor = new ActionConditionProcessor (this);

		spriteTrailByCardName = new Dictionary<CardElement, Sprite> (14) {
			{CardElement.Blood, redTrail},
			{CardElement.Chaos, purpleTrail},
			{CardElement.Dark, blackTrail},
			{CardElement.Earth, brownTrail},
			{CardElement.Fire, orangeTrail},
			{CardElement.Ice, lightBlueTrail},
			{CardElement.Light, whiteTrail},
			{CardElement.Lightning, yellowTrail},
			{CardElement.Magma, orangeTrail},
			{CardElement.Nature, greenTrail},
			{CardElement.Shadow, purpleTrail},
			{CardElement.Water, darkBlueTrail},
			{CardElement.Wild, blackTrail},
			{CardElement.Zombie, redTrail}
		};
	}

	public void ShowActions(Player player) {
		SetupTrails (player);
		PlayAnimation ("Show");
	}

	public void HideActions() {
		PlayAnimation ("Hide");
	}

	private void PlayAnimation(string animationName) {
		if (myAnimator != null) {
			myAnimator.Play (animationName);
		}
	}

	private void SetupTrails(Player player) {
		currentPlayer = player;

		circleActionTrail.sprite = spriteTrailByCardName [player.currentCard.element];
		squareActionTrail.sprite = spriteTrailByCardName [player.currentCard.element];
		rhombusActionTrail.sprite = spriteTrailByCardName [player.currentCard.element];
		saveActionTrail.sprite = spriteTrailByCardName [player.currentCard.element];
		mixActionTrail.sprite = spriteTrailByCardName [player.currentCard.element];

		if(player != null && !player.isAI) {
			if (player.currentCard.state.Equals (CardState.MixedSelection)) {
				DeactivateCircleAction ();
				DeactivateSquareAction ();
				DeactivateRhombusAction ();
				DeactivateSaveAction ();
				ActivateMixAction ();
			} else if(player.currentCard.type.Equals(CardType.Element) && !player.currentCard.IsWildCard()) {
				actionConditionProcessor.ProcessConditionsPerElement (player.currentCard.element);

				if (player.savedCard != null && player.mixedCard != null) {
					if ((player.mixedCard.IsRequirementA (player.savedCard.element) && player.mixedCard.IsRequirementB (player.currentCard.element)) ||
					   (player.mixedCard.IsRequirementB (player.savedCard.element) && player.mixedCard.IsRequirementA (player.currentCard.element))) {
						ActivateMixAction ();

						return;
					}
				}

				DeactivateMixAction ();
			}else if(player.currentCard.type.Equals(CardType.Mixed)) {
				ActivateCircleAction ();
				ActivateSquareAction ();

				DeactivateRhombusAction ();
				DeactivateSaveAction ();
				DeactivateMixAction ();
			} else if(player.currentCard.type.Equals(CardType.Wild) || player.currentCard.IsWildCard()) {
				ActivateCircleAction ();
				ActivateSquareAction ();
				ActivateRhombusAction ();

				DeactivateSaveAction ();
				DeactivateMixAction ();
			}
		}
	}

	private void ActivateCircleAction() {
		circleActionBtn.gameObject.SetActive (true);
		circleActionTrail.gameObject.SetActive (true);
	}

	private void DeactivateCircleAction() {
		circleActionBtn.gameObject.SetActive (false);
		circleActionTrail.gameObject.SetActive (false);
	}

	private void ActivateSquareAction() {
		squareActionBtn.gameObject.SetActive (true);
		squareActionTrail.gameObject.SetActive (true);
	}

	private void DeactivateSquareAction() {
		squareActionBtn.gameObject.SetActive (false);
		squareActionTrail.gameObject.SetActive (false);
	}

	private void ActivateRhombusAction() {
		rhombusActionBtn.gameObject.SetActive (true);
		rhombusActionTrail.gameObject.SetActive (true);
	}

	private void DeactivateRhombusAction() {
		rhombusActionBtn.gameObject.SetActive (false);
		rhombusActionTrail.gameObject.SetActive (false);
	}

	private void ActivateSaveAction() {
		saveActionBtn.gameObject.SetActive (true);
		saveActionTrail.gameObject.SetActive (true);
	}

	private void DeactivateSaveAction() {
		saveActionBtn.gameObject.SetActive (false);
		saveActionTrail.gameObject.SetActive (false);
	}

	private void ActivateMixAction() {
		mixActionBtn.gameObject.SetActive (true);
		mixActionTrail.gameObject.SetActive (true);
	}

	private void DeactivateMixAction() {
		mixActionBtn.gameObject.SetActive (false);
		mixActionTrail.gameObject.SetActive (false);
	}

	private class ActionConditionProcessor {
		private CardActionContainer actionContainer;
		private Dictionary<CardElement, System.Action> containerActonsPerElement;

		public ActionConditionProcessor(CardActionContainer actionContainer) {
			this.actionContainer = actionContainer;

			containerActonsPerElement = new Dictionary<CardElement, System.Action>(11) {
				{CardElement.Blood, ProcessConditionsForBlood},
				{CardElement.Fire, ProcessConditionsForFire},
				{CardElement.Water, ActivateAll},
				{CardElement.Light, ProcessConditionsForLight},
				{CardElement.Lightning, ActivateAll},
				{CardElement.Nature, ProcessConditionsForNature},
				{CardElement.Dark, ProcessConditionsForDark},
				{CardElement.Shadow, ActivateAll},
				{CardElement.Ice, ProcessConditionsForIce},
				{CardElement.Earth, ProcessConditionsForEarth}
			};
		}

		public void ProcessConditionsPerElement(CardElement element) {
			containerActonsPerElement [element] ();
		}

		private void ProcessConditionsForBlood() {
			if (actionContainer.currentPlayer.opponent.Debuffs.IsBleeding) {
				actionContainer.ActivateSquareAction ();
			} else {
				actionContainer.DeactivateSquareAction ();
			}
			actionContainer.ActivateCircleAction ();
			actionContainer.ActivateRhombusAction ();
			ProcessSaveActionConditions ();
		}

		private void ProcessSaveActionConditions() {
			if (actionContainer.currentPlayer.Debuffs.IsFrozen) {
				actionContainer.DeactivateSaveAction ();
			} else {
				actionContainer.ActivateSaveAction ();
			}
		}

		private void ActivateAll() {
			actionContainer.ActivateCircleAction ();
			actionContainer.ActivateSquareAction ();
			actionContainer.ActivateRhombusAction ();
			ProcessSaveActionConditions ();
		}

		//ELEMENTS PROCESSING CONDITIONS
		private void ProcessConditionsForDark() {
			if (actionContainer.currentPlayer.lastSpellCasted != null &&
				(actionContainer.currentPlayer.lastSpellCasted.Element.Equals (CardElement.Chaos) ||
					actionContainer.currentPlayer.lastSpellCasted.Element.Equals (CardElement.Magma) ||
					actionContainer.currentPlayer.lastSpellCasted.Element.Equals (CardElement.Zombie))) {
				actionContainer.DeactivateSquareAction ();
			} else {
				actionContainer.ActivateSquareAction ();
			}

			if (actionContainer.currentPlayer.Debuffs.IsBlind) {
				actionContainer.DeactivateRhombusAction ();
			} else {
				actionContainer.ActivateRhombusAction ();
			}

			actionContainer.ActivateCircleAction ();
			ProcessSaveActionConditions ();
		}

		private void ProcessConditionsForEarth() {	
			if (actionContainer.currentPlayer.Debuffs.IsBlind) {
				actionContainer.DeactivateCircleAction ();
			} else {				
				actionContainer.ActivateCircleAction ();
			}

			actionContainer.ActivateSquareAction ();
			actionContainer.ActivateRhombusAction ();

			ProcessSaveActionConditions ();
		}

		private void ProcessConditionsForFire() {	
			if (actionContainer.currentPlayer.Debuffs.IsBlind) {
				actionContainer.DeactivateSquareAction ();
			} else {				
				actionContainer.ActivateSquareAction ();
			}

			actionContainer.ActivateCircleAction ();
			actionContainer.ActivateRhombusAction ();

			ProcessSaveActionConditions ();
		}

		private void ProcessConditionsForIce() {	
			if (actionContainer.currentPlayer.Debuffs.IsBlind) {
				actionContainer.DeactivateCircleAction ();
			} else {				
				actionContainer.ActivateCircleAction ();
			}

			actionContainer.ActivateSquareAction ();
			actionContainer.ActivateRhombusAction ();

			ProcessSaveActionConditions ();
		}

		private void ProcessConditionsForLight() {	
			if (actionContainer.currentPlayer.Debuffs.IsBlind) {
				actionContainer.DeactivateCircleAction ();
			} else {				
				actionContainer.ActivateCircleAction ();
			}

			actionContainer.ActivateSquareAction ();
			actionContainer.ActivateRhombusAction ();

			ProcessSaveActionConditions ();
		}

		private void ProcessConditionsForLightning() {	
			if (actionContainer.currentPlayer.Debuffs.IsBlind) {
				actionContainer.DeactivateSquareAction ();
			} else {				
				actionContainer.ActivateSquareAction ();
			}

			actionContainer.ActivateCircleAction ();
			actionContainer.ActivateRhombusAction ();

			ProcessSaveActionConditions ();
		}

		private void ProcessConditionsForNature() {	
			if (actionContainer.currentPlayer.Debuffs.IsBlind) {
				actionContainer.DeactivateCircleAction ();
			} else {				
				actionContainer.ActivateCircleAction ();
			}

			actionContainer.ActivateSquareAction ();
			actionContainer.ActivateRhombusAction ();

			ProcessSaveActionConditions ();
		}
	}
}