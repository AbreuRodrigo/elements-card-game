using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DebuffManager : MonoBehaviour {
	[Header("Properties")]
	public int nextMarkerIndex;

	[Header("Debuff Markers")]
	public DebuffMarker bleedDebuff;
	public DebuffMarker blindDebuff;
	public DebuffMarker burnDebuff;
	public DebuffMarker curseDebuff;
	public DebuffMarker freezeDebuff;
	public DebuffMarker knockDownDebuff;
	public DebuffMarker poisonDebuff;
	public DebuffMarker wetDebuff;

	[Header("Buff Markers")]
	public DebuffMarker staticBuff;
	public DebuffMarker refreshBuff;

	private float[] markerPositions;
	private DebuffMarker currentMarker;

	private Dictionary<CardElement, DebuffMarker> debuffMarkerByElement;
	private Dictionary<CardElement, DebuffMarker> buffMarkerByElement;

	private List<DebuffMarker> activeMarkers;

	void Awake() {
		nextMarkerIndex = 0;

		activeMarkers = new List<DebuffMarker> (10);

		markerPositions = new float[10];
		markerPositions [0] = bleedDebuff.rect.anchoredPosition.x;
		markerPositions [1] = blindDebuff.rect.anchoredPosition.x;
		markerPositions [2] = burnDebuff.rect.anchoredPosition.x;
		markerPositions [3] = curseDebuff.rect.anchoredPosition.x;
		markerPositions [4] = freezeDebuff.rect.anchoredPosition.x;
		markerPositions [5] = knockDownDebuff.rect.anchoredPosition.x;
		markerPositions [6] = poisonDebuff.rect.anchoredPosition.x;
		markerPositions [7] = freezeDebuff.rect.anchoredPosition.x;
		markerPositions [8] = staticBuff.rect.anchoredPosition.x;
		markerPositions [9] = refreshBuff.rect.anchoredPosition.x;

		debuffMarkerByElement = new Dictionary<CardElement, DebuffMarker> (8) {
			{ CardElement.Blood, bleedDebuff },
			{ CardElement.Light, blindDebuff },
			{ CardElement.Fire, burnDebuff },
			{ CardElement.Shadow, curseDebuff },
			{ CardElement.Ice, freezeDebuff },
			{ CardElement.Earth, knockDownDebuff },
			{ CardElement.Nature, poisonDebuff },
			{ CardElement.Water, wetDebuff }
		};

		buffMarkerByElement = new Dictionary<CardElement, DebuffMarker> (2) {
			{ CardElement.Lightning, staticBuff },
			{ CardElement.Water, refreshBuff }
		};
	}

	/*HANDLE DEBUFF MARKERS*/
	public void AddDebuffMarker(CardElement element, int duration) {
		currentMarker = debuffMarkerByElement [element];
		AddElementMark (duration);
	}

	public void ResetDebuffMarker(CardElement element, int duration) {
		currentMarker = debuffMarkerByElement [element];
		currentMarker.Show (duration);
	}

	public void RemoveDebuffMarker(CardElement element) {
		currentMarker = debuffMarkerByElement [element];
		RemoveElementMark ();
	}

	/*HANDLE BUFF MARKERS*/
	public void AddBuffMarker(CardElement element, int duration) {
		currentMarker = buffMarkerByElement [element];
		AddElementMark (duration);
	}

	public void ResetBuffMarker(CardElement element, int duration) {
		currentMarker = buffMarkerByElement [element];
		currentMarker.Show (duration);
	}

	public void RemoveBuffMarker(CardElement element) {
		currentMarker = buffMarkerByElement [element];
		RemoveElementMark ();
	}

	private void AddElementMark(int duration) {
		Vector2 p = currentMarker.rect.anchoredPosition;

		if(nextMarkerIndex >= 0) {
			p.x = markerPositions [nextMarkerIndex];

			currentMarker.rect.anchoredPosition = p;
			currentMarker.Show (duration);
			currentMarker.index = nextMarkerIndex;

			activeMarkers.Add(currentMarker);

			nextMarkerIndex++;
		}
	}

	private void RemoveElementMark() {
		int removedIndex = currentMarker.index;
		currentMarker.Hide ();

		activeMarkers.RemoveAt(removedIndex);

		foreach(DebuffMarker marker in activeMarkers) {
			if(marker.index > removedIndex) {
				marker.index -= 1;

				Vector2 p = marker.rect.anchoredPosition;
				p.x = markerPositions [marker.index];

				marker.rect.anchoredPosition = p;
			}
		}

		if (nextMarkerIndex > 0) {
			nextMarkerIndex--;
		}
	}
}