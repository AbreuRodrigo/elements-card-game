using UnityEngine;
using System.Collections;

public class ArcaneCircle : MonoBehaviour {

	public GameObject arcaneCircle;
	public GameObject runes;

	public void ShowArcaneCircle() {
		if(arcaneCircle != null && runes != null) {
			runes.gameObject.SetActive (true);
			arcaneCircle.SetActive (true);

			runes.GetComponent<Animator> ().enabled = true;
			arcaneCircle.GetComponent<Animator> ().enabled = true;
		}

		SoundManager.instance.PlayMagicCircle ();
	}

	public void HideArcaneCircle() {
		if(arcaneCircle != null && runes != null) {
			runes.GetComponent<Animator> ().enabled = false;
			arcaneCircle.GetComponent<Animator> ().enabled = false;
		}
	}
}