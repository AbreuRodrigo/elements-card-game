using UnityEngine;
using System.Collections;

public class ParticleTest : MonoBehaviour {
	public Animator myAnimator;
	public ParticleSystem myParticle;

	public void ThrowParticles() {
		myParticle.Play ();
	}
}