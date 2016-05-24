using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LogoBehaviour : MonoBehaviour {
	public Animator myAnimator;
	public ParticleSystem particles;
	public float delay;

	void Start() {
		StartCoroutine (StartAnimationWithDelay ());
	}

	public void PlayImpactSound() {
		SoundManager.instance.PlayImpact ();
	}

	public void PlayBrightBellSound() {
		SoundManager.instance.PlayBrightBell ();
	}

	public void ThroughParticles() {
		if (particles != null) {
			particles.Play ();
		}
	}

	public void ShakeIt() {
		if (myAnimator != null) {
			myAnimator.Play ("ShakeIt");
		}
		ThroughParticles ();
		PlayImpactSound ();
	}

	private void FromTopToBottom() {
		if(myAnimator != null) {
			myAnimator.Play ("ShowFromTopToBottom1");
		}
	}

	IEnumerator StartAnimationWithDelay() {
		yield return new WaitForSeconds (delay);

		FromTopToBottom ();
	}
}