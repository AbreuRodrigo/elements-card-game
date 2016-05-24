using UnityEngine;
using System.Collections;

public class PageFlipper : MonoBehaviour {
	public Animator myAnimator;

	void Update() {
		if(Input.GetKeyDown(KeyCode.P)) {
			FlipThePage ();
		}
	}

	public void FlipThePage() {
		if(myAnimator != null) {
			myAnimator.Play ("Flip");
		}
	}

	public void PlayPageFlipSound() {
		SoundManager.instance.PlayPageFlip ();
	}
}
