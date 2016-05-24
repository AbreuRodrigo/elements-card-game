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

	private CardElement lastActivated;

	void Start() {
		spriteTrailByCardName = new Dictionary<CardElement, Sprite> () {
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

	public void ShowActions(CardElement cardElement, CardState cardState) {
		lastActivated = cardElement;
		SetupTrails (cardState);

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

	private void SetupTrails(CardState cardState) {
		circleActionTrail.sprite = spriteTrailByCardName [lastActivated];
		squareActionTrail.sprite = spriteTrailByCardName [lastActivated];
		rhombusActionTrail.sprite = spriteTrailByCardName [lastActivated];
		saveActionTrail.sprite = spriteTrailByCardName [lastActivated];
		mixActionTrail.sprite = spriteTrailByCardName [lastActivated];

		if (cardState.Equals (CardState.MixedSelection)) {
			circleActionBtn.gameObject.SetActive (false);
			circleActionTrail.gameObject.SetActive (false);
			squareActionBtn.gameObject.SetActive (false);
			squareActionTrail.gameObject.SetActive (false);
			rhombusActionBtn.gameObject.SetActive (false);
			rhombusActionTrail.gameObject.SetActive (false);
			saveActionBtn.gameObject.SetActive (false);
			saveActionTrail.gameObject.SetActive (false);
			mixActionBtn.gameObject.SetActive (true);
			mixActionTrail.gameObject.SetActive (true);
		} else {
			circleActionBtn.gameObject.SetActive (true);
			circleActionTrail.gameObject.SetActive (true);
			squareActionBtn.gameObject.SetActive (true);
			squareActionTrail.gameObject.SetActive (true);
			rhombusActionBtn.gameObject.SetActive (true);
			rhombusActionTrail.gameObject.SetActive (true);
			saveActionBtn.gameObject.SetActive (true);
			saveActionTrail.gameObject.SetActive (true);
			mixActionBtn.gameObject.SetActive (false);
			mixActionTrail.gameObject.SetActive (false);
		}
	}
}