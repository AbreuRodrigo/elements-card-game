using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class CardMarkerView : MonoBehaviour, IPointerDownHandler {
	public Image orb;
	public Text cardName;
	public Text amount;

	public int index;

	public Animator myAnimator;

	public void OnPointerDown (PointerEventData eventData) {
		SoundManager.instance.PlayClickSound ();
		DeckBuilder.Instance.RemoveCardMarkFromDeck (cardName.text, index);
	}

	public void ShowUp() {
		myAnimator.Rebind ();
		myAnimator.Play ("ShowUp");
	}

	public void Pulse() {
		myAnimator.Rebind ();
		myAnimator.Play ("Pulse");
	}

	public void Idle() {
		myAnimator.Rebind ();
		myAnimator.Play ("Idle");
	}
}