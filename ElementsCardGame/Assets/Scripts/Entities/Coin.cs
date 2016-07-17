using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour {

	public Animator myAnimator;

	public bool hasFlipped;

	private CoinResult lastResult;

	private bool fadedIn;
	public bool FadedIn {
		get { return fadedIn; }
	}

	private bool fadedOut;
	public bool FadedOut {
		get { return fadedOut; }
	}

	public void FlipCoin() {
		hasFlipped = false;

		int decision = Random.Range (0, 2);

		SoundManager.instance.PlayCoinFlip ();

		if (decision == 0) {
			FlipCoinForSword ();
			lastResult = CoinResult.Sword;
		} else {
			FlipCoinForShield ();
			lastResult = CoinResult.Shield;
		}
	}

	public void EndFadeInProcess() {
		fadedIn = true;
	}

	public void EndFadeOutProcess() {
		fadedOut = true;
		Disable ();
	}

	//This methode is called from the coin animator, just after the animation finishes
	public void ForceCoinNotifyGameControllerTheResult() {
		hasFlipped = true;

		GamePlayController.instance.NotificationFromCoinFlip (lastResult);
	}

	public void Idle() {
		if (myAnimator != null) {
			myAnimator.Play ("Idle");
		}
	}

	private void FlipCoinForSword() {
		if(myAnimator != null) {
			myAnimator.Play ("FlipCoinForSword");
		}
	}

	private void FlipCoinForShield() {
		if (myAnimator != null) {
			myAnimator.Play ("FlipCoinForShield");
		}
	}

	public void FadeOutFromSword() {
		fadedOut = false;

		if (myAnimator != null) {
			myAnimator.Play ("FadeOutSword");
		}
	}

	public void FadeOutFromShield() {
		fadedOut = false;

		if (myAnimator != null) {
			myAnimator.Play ("FadeOutShield");
		}
	}

	public void FadeIn() {
		fadedIn = false;

		if (myAnimator != null) {
			myAnimator.Play ("FadeIn");
			hasFlipped = false;
		}
	}

	public void Enable() {
		gameObject.SetActive (true);

		if(myAnimator != null) {
			myAnimator.enabled = true;
		}
	}

	public void Disable() {
		if(myAnimator != null) {
			myAnimator.enabled = false;
		}

		gameObject.SetActive (false);
	}
}