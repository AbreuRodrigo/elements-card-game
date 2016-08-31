using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TurnOrderRibbon : MonoBehaviour {
	public Animator myAnimator;
	public Image myImage;
	public Image youGoFirst;
	public Image enemyGoesFirst;
	public Image victory;
	public Image gameOver;

	public void Enable() {
		gameObject.SetActive (true);

		if(myImage != null) {
			myImage.enabled = true;
		}

		if (myAnimator != null) {
			myAnimator.enabled = true;
		}
	}

	public void Disable() {
		if (myAnimator != null) {
			myAnimator.enabled = false;
			myImage.enabled = false;
			youGoFirst.enabled = false;
			enemyGoesFirst.enabled = false;
			myAnimator.gameObject.SetActive (false);
		}

		GUIController.instance.HideInteractionBlocker ();
	}

	public void StartHidingProcess() {
		SoundManager.instance.PlayYourTurnHiding ();
	}

	public void ShowYouGoFirst() {
		if (myAnimator != null) {
			if(youGoFirst != null) {
				youGoFirst.enabled = true;
			}

			myAnimator.Play ("Show");
		}
	}

	public void ShowEnemyGoesFirst() {		
		if (myAnimator != null) {
			if(enemyGoesFirst != null) {
				enemyGoesFirst.enabled = true;
			}

			myAnimator.Play ("Show");
		}
	}

	public void ShowVictoryMessage() {
		if(myAnimator != null) {
			if(victory != null) {
				victory.enabled = true;
			}

			myAnimator.Play ("StaticShow");
		}
	}

	public void ShowGameOverMessage() {
		if(myAnimator != null) {
			if(gameOver != null) {
				gameOver.enabled = true;
			}

			myAnimator.Play ("StaticShow");
		}
	}
}