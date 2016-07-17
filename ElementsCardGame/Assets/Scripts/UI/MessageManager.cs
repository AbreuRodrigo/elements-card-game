using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MessageManager : MonoBehaviour {
	private const string DRAW_A_CARD_MESSAGE = "Draw a card by clicking the deck button on the right.";
	private const string THIS_IS_YOUR_MIXED_CARD = "This is your mixed card, put it aside to the mixed pile.";
	private const string KICKSTARTER = "Please, consider donating to our Kickstarter campaign to make this option possible!";

	public Animator myAnimator;
	public Text myText;
	public float timerToHideMessage;
	public bool queueMessages;

	private bool visible;
	private string savedMessage;

	public void ShowDrawACardMessage() {
		ValidateAndShowMessage (DRAW_A_CARD_MESSAGE);
	}

	public void ShowMixedCardMessage() {
		ValidateAndShowMessage (THIS_IS_YOUR_MIXED_CARD);
	}

	public void ShowKickstartMessage() {
		ValidateAndShowMessage (KICKSTARTER);
	}

	private void ValidateAndShowMessage(string message) {
		gameObject.SetActive (true);

		if (!visible) {
			if (myText != null) {
				myText.text = message;
			}
		} else if (queueMessages) {
			savedMessage = message;
		}

		Show ();
	}

	private void Show() {
		if (!visible) {
			if (myAnimator != null && myText != null && myText.text != "") {
				myAnimator.Play ("Show");

				visible = true;

				StartCoroutine (HideMessageRoutine ());
			}
		} else if (queueMessages) {
			Hide ();

			StartCoroutine (ShowPostponedMessageRoutine());
		}
	}

	public void Hide() {
		if (myAnimator != null) {
			myAnimator.Play ("Hide");
		}
	}

	IEnumerator HideMessageRoutine() {
		yield return new WaitForSeconds (timerToHideMessage);

		Hide ();

		visible = false;
	}

	IEnumerator ShowPostponedMessageRoutine() {
		yield return new WaitForSeconds (2);

		visible = false;

		if(myText != null) {
			myText.text = savedMessage;
			savedMessage = "";
		}

		Show ();
	}
}
