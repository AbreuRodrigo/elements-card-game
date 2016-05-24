using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CompanyLogo : MonoBehaviour {

	public Animator myAnimator;
	public InteractionBlocker interactonBlocker;

	void Start() {
		StartCoroutine (PlayLogoAnimation ());
	}

	public void PlaySplatSound() {
		SoundManager.instance.PlaySplat ();
	}

	public void PlayFallingSound() {
		SoundManager.instance.PlayFalling ();
	}

	public void FadeInteractionBlockerOut() {
		StartCoroutine (WaitAndLoadMenuScene ());
	}

	IEnumerator PlayLogoAnimation() {
		yield return new WaitForSeconds (2);

		if(myAnimator != null) {
			myAnimator.Play ("Show");
		}
	}

	IEnumerator WaitAndLoadMenuScene() {
		yield return new WaitForSeconds (2);

		if(interactonBlocker != null) {
			interactonBlocker.Enable ();
			interactonBlocker.FadeOut ();
		}

		SoundManager.instance.ChangeToMenuMusic ();

		yield return new WaitForSeconds (2);

		SceneManager.LoadScene ("Menu");
	}
}