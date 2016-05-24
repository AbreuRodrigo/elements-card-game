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

	void Update() {
		/*if(!hasFlipped) {
			if (!Application.isMobilePlatform) {
				if (Input.GetMouseButtonDown(0)) {
					FlipCoinByClick (Input.mousePosition);
				}
			} else {
				if (Input.touches != null && Input.touches.Length > 0) {
					Touch t = Input.touches [0];

					if (t.phase.Equals (TouchPhase.Began)) {
						FlipCoinByClick (Input.mousePosition);
					}
				}
			}
		}*/
	}

	private void FlipCoinByClick(Vector3 point) {
		RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (point), Vector2.zero);

		if (hit.collider != null && "Coin".Equals (hit.collider.gameObject.tag)) {
			FlipCoin ();
		}
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

	public void ForceCoinNotifyGameControllerTheResult() {
		hasFlipped = true;

		GamePlayController.instance.NotificationFromCoinFlip (lastResult);
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