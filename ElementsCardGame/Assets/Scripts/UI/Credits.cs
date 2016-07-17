using UnityEngine;
using System.Collections;

public class Credits : MonoBehaviour {
	public Animator myAnimator;
	public GameObject closeBtn;

	void Awake() {
		if (myAnimator == null) {
			myAnimator = GetComponent<Animator> ();
		}
	}

	public void Show() {
		if(myAnimator != null) {			
			myAnimator.Play ("Show");
			GUIMenuController.instance.HideLogo ();
			SoundManager.instance.PlayCardMoveSound ();
		}
	}

	public void Hide() {
		if(myAnimator != null) {
			myAnimator.Play ("Hide");
		}
	}

	public void ShowUI() {
		GUIMenuController.instance.ShowUI ();
		GUIMenuController.instance.ShowLogo ();
	}

	public void ShowCloseBtn() {
		if(closeBtn != null) {
			closeBtn.gameObject.SetActive (true);
		}
	}

	public void HideCloseBtn() {
		if(closeBtn != null) {
			closeBtn.gameObject.SetActive (false);
		}
	}
}