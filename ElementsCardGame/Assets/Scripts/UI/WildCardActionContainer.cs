using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WildCardActionContainer : MonoBehaviour {
	public Animator myAnimator;

    [Header("Card Prefabs")]
	[SerializeField]
	private Card blood;
	[SerializeField]
	private Card dark;
	[SerializeField]
	private Card earth;
	[SerializeField]
	private Card fire;
	[SerializeField]
	private Card ice;
	[SerializeField]
	private Card light;
	[SerializeField]
	private Card lightning;
	[SerializeField]
	private Card nature;
	[SerializeField]
	private Card shadow;
	[SerializeField]
	private Card water;

	private Dictionary<string, Card> elementByStringName;

	void Start() {
		elementByStringName = new Dictionary<string, Card>(10) {
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
				elementByStringName [element]
			);
		}
	}

	private void PlayAnimation(string animationName) {
		if (myAnimator != null) {
			myAnimator.Play (animationName);
		}
	}
}