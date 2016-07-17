using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {
	public static CameraManager instance;

	public Animator myAnimator;

	void Awake() {
		if(instance == null) {
			instance = this;
		}

		if(myAnimator == null) {
			myAnimator = GetComponent<Animator> ();
		}
	}

	public void Shake() {
		if(myAnimator != null) {
			myAnimator.enabled = true;
			myAnimator.Play ("Shake");
		}
	}
}
