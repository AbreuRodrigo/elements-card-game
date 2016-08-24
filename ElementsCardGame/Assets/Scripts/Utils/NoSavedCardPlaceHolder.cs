using UnityEngine;
using System.Collections;

public class NoSavedCardPlaceHolder : MonoBehaviour {
	public Animator myAnimator;

	public void Show() {
		PlayAnimation ("Show");
	}

	public void Hide() {
		PlayAnimation ("Hide");
	}

	private void PlayAnimation(string animationName) {
		if (myAnimator != null && animationName != null && animationName != "") {
			myAnimator.Play (animationName);
		}
	}
}