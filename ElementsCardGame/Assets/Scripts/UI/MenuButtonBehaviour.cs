using UnityEngine;
using System.Collections;

public class MenuButtonBehaviour : MonoBehaviour {
	public Animator myAnimator;

	private void PressingAnimation() {
		if(myAnimator != null) {
			myAnimator.Play ("PressButton");
		}
	}

	public void DeckBuilderButtonPress() {
		PressingAnimation ();
		GameMenuController.instance.DeckBuilderButtonPress ();
	}

	public void LibraryButtonPress() {
		PressingAnimation ();
		GameMenuController.instance.LibraryButtonPress ();
	}

	public void SinglePlayerButtonPress() {
		PressingAnimation ();
		GameMenuController.instance.SinglePlayerButtonPress ();
	}

	public void MultiplayerButtonPress() {
		PressingAnimation ();
		GameMenuController.instance.MultiplayerButtonPressed ();
	}

	public void PlayClickingSound() {
		SoundManager.instance.PlayClickSound ();
	}
}
