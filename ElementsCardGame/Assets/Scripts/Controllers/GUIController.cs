using UnityEngine;

using UnityEngine.UI;
using System.Collections;

public class GUIController : MonoBehaviour {
	public static GUIController instance;

	private const string HIDE_ANIMATION_LABEL = "Hide";
	private const string SHOW_ANIMATION_LABEL = "Show";

	public Canvas canvas;

	public Button endTurnButton;
	public Image endTurnButtonImage;

	public Button deckButton;

	public GameObject mixedCardButton;
	public GameObject savedCardButton;
	public GameObject discardedCardButton;

	public RectTransform deckButtonRect;
	public RectTransform discardBaseRect;

	public RectTransform opponentShieldRect;

	public Text remainingCardsTxt;

	[Header("UIComponents")]
	public InteractionBlocker interactionBlocker;
	public TurnOrderRibbon turnOrderRibbon;
	public CardActionContainer cardActionContainer;
	public WildCardActionContainer wildcardActionContainer;

	[Header("Animators")]
	public Animator savedCardBase;
	public Animator mixedCardBase;
	public Animator discardedCardBase;
	public Animator deckCardBase;

	private int remainingCards;

	void Awake() {
		if(instance == null) {
			instance = this;
		}
	}

	public void ShowEndTurnButton() {
		if(endTurnButton != null && !endTurnButton.enabled) {
			endTurnButton.gameObject.SetActive (true);
			endTurnButton.enabled = true;
			endTurnButtonImage.enabled = true;
		}
	}

	public void HideEndTurnButton() {
		if(endTurnButton != null) {			
			endTurnButtonImage.enabled = false;
			endTurnButton.enabled = false;
			endTurnButton.gameObject.SetActive (false);
		}
	}

	public void DeckButtonTurnOn() {
		if(deckButton != null) {
			deckButton.gameObject.SetActive (true);
		}
	}

	public void DeckButtonTurnOff() {
		if (deckButton != null) {
			deckButton.gameObject.SetActive (false);
		}
	}

	public void DefineRemainingCardsTxt(int remainingCards) {
		this.remainingCards = remainingCards;
		remainingCardsTxt.text = this.remainingCards.ToString();
	}

	public void IncreaseRemainingCards() {
		remainingCards++;
		remainingCardsTxt.text = remainingCards.ToString();
	}

	public void DecreaseRemainingCards() {
		remainingCards--;
		remainingCardsTxt.text = remainingCards.ToString();
	}

	public Vector3 GetOpponentShieldTransformPosition() {
		return RectToTransformedPosition (opponentShieldRect);
	}

	public Vector3 GetSaveBaseTransformedPosition() {
		return RectToTransformedPosition (savedCardButton.GetComponent<RectTransform>());
	}

	public Vector3 GetMixedCardBaseTransformedPosition() {
		return RectToTransformedPosition (mixedCardButton.GetComponent<RectTransform>());
	}

	public Vector3 GetDiscardBaseTransformedPosition() {
		return RectToTransformedPosition (discardedCardButton.GetComponent<RectTransform>());
	}

	public Vector3 GetDeckButtonTransformedPosition() {
		return RectToTransformedPosition (deckButtonRect);
	}

	public Vector3 RectToTransformedPosition (RectTransform rectTransform) {
		return rectTransform.transform.TransformPoint(rectTransform.rect.center.x, rectTransform.rect.center.x, 0);
	}

	public void ShowCardActionButtons() {
		cardActionContainer.ShowActions (GamePlayController.instance.localPlayer);
	}

	public void HideCardActionButtons() {
		cardActionContainer.HideActions ();
	}

	public void ShowWildCardActionButtons() {
		wildcardActionContainer.ShowActions ();
	}

	public void HideWildCardActionButtons() {
		wildcardActionContainer.HideActions ();
	}

	public void FadeOutInteractionBlocker() {
		if(interactionBlocker != null) {
			interactionBlocker.Enable ();
			interactionBlocker.FadeOut ();
		}
	}

	public void FadeInInteractionBlocker() {
		if(interactionBlocker != null) {
			interactionBlocker.Enable ();
			interactionBlocker.FadeIn ();
		}
	}

	public void ShowInteractionBlockerHalfFaded() {
		if(interactionBlocker != null) {
			interactionBlocker.HalfFaded ();
		}
	}

	public void HideInteractionBlocker() {
		if(interactionBlocker != null) {
			interactionBlocker.Disable ();
		}
	}

	public bool IsInteractionBlockerEnabled() {
		if(interactionBlocker == null) {
			return false;
		}

		return interactionBlocker.gameObject.activeSelf;
	}

	public void ShowYouGoFirstRibbon() {
		ShowInteractionBlockerHalfFaded ();
	
		if(turnOrderRibbon != null) {
			turnOrderRibbon.Enable ();
			turnOrderRibbon.ShowYouGoFirst ();
		}
	}

	public void ShowEnemyGoesFirstRibbon() {
		ShowInteractionBlockerHalfFaded ();

		if(turnOrderRibbon != null) {
			turnOrderRibbon.Enable ();
			turnOrderRibbon.ShowEnemyGoesFirst ();
		}
	}

	public void ShowCardBases() {
		CallAnimationWhenNoNull (savedCardBase, SHOW_ANIMATION_LABEL);
		CallAnimationWhenNoNull (mixedCardBase, SHOW_ANIMATION_LABEL);
		CallAnimationWhenNoNull (discardedCardBase, SHOW_ANIMATION_LABEL);
		CallAnimationWhenNoNull (deckCardBase, SHOW_ANIMATION_LABEL);
	}

	public void HideCardBases() {
		CallAnimationWhenNoNull (savedCardBase, HIDE_ANIMATION_LABEL);
		CallAnimationWhenNoNull (mixedCardBase, HIDE_ANIMATION_LABEL);
		CallAnimationWhenNoNull (discardedCardBase, HIDE_ANIMATION_LABEL);
		CallAnimationWhenNoNull (deckCardBase, HIDE_ANIMATION_LABEL);
	}

	public void ShowMixedCardButton() {
		if(mixedCardBase != null) {
			mixedCardButton.SetActive (true);
			mixedCardButton.GetComponent<Button> ().enabled = true;
			mixedCardButton.GetComponent<Image> ().enabled = true;
			mixedCardButton.GetComponent<Animator> ().enabled = true;
		}
	}

	public void HideMixedCardButton() {
		if(mixedCardBase != null) {			
			mixedCardButton.GetComponent<Button> ().enabled = false;
			mixedCardButton.GetComponent<Image> ().enabled = false;
			mixedCardButton.GetComponent<Animator> ().enabled = false;
			mixedCardButton.SetActive (false);
		}
	}

	public void ShowSavedCardButton() {
		if(savedCardButton != null) {
			savedCardButton.SetActive (true);
			savedCardButton.GetComponent<Button> ().enabled = true;
			savedCardButton.GetComponent<Image> ().enabled = true;
			savedCardButton.GetComponent<Animator> ().enabled = true;
		}
	}

	public void ShowDiscardedCardButton() {
		if(discardedCardButton != null) {
			discardedCardButton.SetActive (true);
			discardedCardButton.GetComponent<Button> ().enabled = true;
			discardedCardButton.GetComponent<Image> ().enabled = true;
			discardedCardButton.GetComponent<Animator> ().enabled = true;
		}
	}

	public void HideSavedCardButton() {
		if(savedCardButton != null) {			
			savedCardButton.GetComponent<Button> ().enabled = false;
			savedCardButton.GetComponent<Image> ().enabled = false;
			savedCardButton.GetComponent<Animator> ().enabled = false;
			savedCardButton.SetActive (false);
		}
	}

	private void CallAnimationWhenNoNull(Animator animator, string animation) {
		if(animator != null) {
			animator.Play (animation);
		}
	}
}