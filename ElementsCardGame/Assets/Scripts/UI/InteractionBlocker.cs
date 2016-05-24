using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InteractionBlocker : MonoBehaviour {
	public Animator myAnimator;
	public Image myImage;

	public bool startsHalfFaded;
	public bool startsFadingIn;
	public int waitToStartFadingIn;

	void Start() {
		if(startsHalfFaded) {
			HalfFaded ();
		}
		if(startsFadingIn) {
			myAnimator.enabled = false;
			myImage.enabled = true;
			myImage.color = Color.black;

			if(waitToStartFadingIn > 0) {
				StartCoroutine (LateFadeIn ());
			}else {
				FadeIn ();
			}
		}
	}

	public void FadeIn() {
		if(myAnimator != null) {
			myAnimator.Play ("FadeIn");
		}
	}

	public void FadeOut() {
		if(myAnimator != null) {
			myAnimator.Play ("FadeOut");
		}
	}

	public void Disable() {
		myAnimator.enabled = false;
		myImage.enabled = false;
		gameObject.SetActive (false);
	}

	public void Enable() {
		gameObject.SetActive (true);
		myImage.enabled = true;
		myAnimator.enabled = true;
	}

	public void HalfFaded() {
		Enable ();

		if(myAnimator != null) {
			myAnimator.Play ("HalfFaded");
		}
	}

	IEnumerator LateFadeIn() {
		yield return new WaitForSeconds (waitToStartFadingIn);
		myAnimator.enabled = true;
		FadeIn ();
	}
}