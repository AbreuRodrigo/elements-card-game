using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WildCardActionContainer : MonoBehaviour {
	public Animator myAnimator;

	[Header("Elements")]
	[SerializeField]
	private Sprite blood;
	[SerializeField]
	private Sprite dark;
	[SerializeField]
	private Sprite earth;
	[SerializeField]
	private Sprite fire;
	[SerializeField]
	private Sprite ice;
	[SerializeField]
	private Sprite light;
	[SerializeField]
	private Sprite lightning;
	[SerializeField]
	private Sprite nature;
	[SerializeField]
	private Sprite shadow;
	[SerializeField]
	private Sprite water;

	[Header("Auras")]
	[SerializeField]
	private Sprite bloodAura;
	[SerializeField]
	private Sprite darkAura;
	[SerializeField]
	private Sprite earthAura;
	[SerializeField]
	private Sprite fireAura;
	[SerializeField]
	private Sprite iceAura;
	[SerializeField]
	private Sprite lightAura;
	[SerializeField]
	private Sprite lightningAura;
	[SerializeField]
	private Sprite natureAura;
	[SerializeField]
	private Sprite shadowAura;
	[SerializeField]
	private Sprite waterAura;

	private Dictionary<string, CardElement> elementByStringName;
	private Dictionary<string, Sprite> spriteByStringName;
	private Dictionary<string, Sprite> auraByStringName;

	void Start() {
		elementByStringName = new Dictionary<string, CardElement> (10) {
			{"Blood", CardElement.Blood},
			{"Dark", CardElement.Dark},
			{"Earth", CardElement.Earth},
			{"Fire", CardElement.Fire},
			{"Ice", CardElement.Ice},
			{"Light", CardElement.Light},
			{"Lightning", CardElement.Lightning},
			{"Nature", CardElement.Nature},
			{"Shadow", CardElement.Shadow},
			{"Water", CardElement.Water}
		};

		spriteByStringName = new Dictionary<string, Sprite> (10) {
			{"Blood", blood},
			{"Dark", dark},
			{"Earth", earth},
			{"Fire", fire},
			{"Ice", ice},
			{"Light", light},
			{"Lightning", lightning},
			{"Nature", nature},
			{"Shadow", shadow},
			{"Water", water}
		};

		auraByStringName = new Dictionary<string, Sprite> (10) {
			{"Blood", bloodAura},
			{"Dark", darkAura},
			{"Earth", earthAura},
			{"Fire", fireAura},
			{"Ice", iceAura},
			{"Light", lightAura},
			{"Lightning", lightningAura},
			{"Nature", natureAura},
			{"Shadow", shadowAura},
			{"Water", waterAura}
		};
	}

	public void ShowActions() {
		PlayAnimation ("Show");
	}

	public void HideActions() {
		PlayAnimation ("Hide");
	}

	public void TurnWildCardIntoElement(string element) {
		if(element != null) {
			HideActions ();

			GamePlayController.instance.localPlayer.currentCard.DoWildCardElementTransition(
				elementByStringName [element], spriteByStringName[element], auraByStringName[element]
			);
		}
	}

	private void PlayAnimation(string animationName) {
		if (myAnimator != null) {
			myAnimator.Play (animationName);
		}
	}
}