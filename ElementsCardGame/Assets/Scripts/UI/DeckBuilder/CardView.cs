using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class CardView : MonoBehaviour , IPointerDownHandler, IPointerUpHandler {
	public CardElement element;
	public CardType type;
	public Image image;

	public void OnPointerDown (PointerEventData eventData) {
		DeckBuilder.Instance.StartDraggingCard (image.sprite, element, type);
	}

	public void OnPointerUp (PointerEventData eventData) {
		DeckBuilder.Instance.CancelDraggingCard ();
	}
}