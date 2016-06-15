using UnityEngine;
using System.Collections;

public class ArcaneCircle : MonoBehaviour {
	public Animator runes;

	public void ShowArcaneCircle() {
		if(runes != null) {
			runes.gameObject.SetActive (true);
			runes.enabled = true;
		}
	}

	public void HideArcaneCircle() {
		if(runes != null) {
			runes.Stop ();
			runes.enabled = false;
			runes.gameObject.SetActive (false);
		}
	}
}