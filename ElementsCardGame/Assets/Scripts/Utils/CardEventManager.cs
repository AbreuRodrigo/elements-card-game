using UnityEngine;
using System.Collections;

public class CardEventManager : MonoBehaviour {
	private const float CARD_CLICK_COOL_DOWN = 0.5f;
	private float lastCardClick = 0;

	public GamePlayController game;

	void FixedUpdate() {
		if (!Application.isMobilePlatform) {
			if (Input.GetMouseButtonUp(0)) {
				ProcessCardSelectionEvent (Input.mousePosition);
			}
		} else {
			if (Input.touches != null && Input.touches.Length > 0) {
				Touch t = Input.touches [0];

				if (t.phase.Equals (TouchPhase.Ended)) {
					ProcessCardSelectionEvent (Input.mousePosition);
				}
			}
		}
	}

	void ProcessCardSelectionEvent (Vector3 point) {
		if(Time.time > (lastCardClick + CARD_CLICK_COOL_DOWN)) {
			lastCardClick = Time.time;

			RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (point), Vector2.zero);

			if (hit.collider != null && "Card".Equals(hit.collider.tag) && game.localPlayer.currentCard != null) {
				if(game.localPlayer.currentCard.state.Equals(CardState.InHand) ||
					game.localPlayer.currentCard.state.Equals(CardState.MixedSelection)) {
					if (!game.localPlayer.currentCard.Selected) {
						game.localPlayer.currentCard.SelectCard ();

						if (!game.localPlayer.currentCard.type.Equals (CardType.Wild)) {
							GUIController.instance.ShowCardActionButtons ();
						} else {
							GUIController.instance.ShowWildCardActionButtons ();
						}

						GUIController.instance.ShowInteractionBlockerHalfFaded ();
					} else {
						game.localPlayer.currentCard.DeselectCard ();

						if (!game.localPlayer.currentCard.type.Equals (CardType.Wild)) {
							GUIController.instance.HideCardActionButtons ();
						} else {
							GUIController.instance.HideWildCardActionButtons ();
						}

						GUIController.instance.HideInteractionBlocker ();
					}
				}
			}
		}
	}
}