using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CardLibraryScrollView : MonoBehaviour {

	//PUBLIC
	public RectTransform panel;
	public RectTransform centerAnchor;
	public RectTransform[] cards;

	//PRIVATE
	private float[] distances;
	private bool dragging = false;
	private int totalCards;
	private float minDistance;
	private int closestIndex;

	void Start () {
		totalCards = cards.Length;
		distances = new float[totalCards];

		minDistance = Mathf.Abs (
			cards [0].anchoredPosition.x - cards [1].anchoredPosition.x
		);
	}

	void Update() {
		for(int i = 0; i < totalCards; i++) {
			distances [i] = Mathf.Abs (centerAnchor.transform.position.x - cards [i].transform.position.x);
		}

		float min = Mathf.Min (distances);

		for(int i = 0; i < totalCards; i++) {
			if (distances [i] != min) {
				cards [i].sizeDelta = new Vector2 (195, 300);
				cards [i].GetComponent<Image> ().color = new Color (1, 1, 1, 0.25f);
			} else {
				if(closestIndex != i) {
					closestIndex = i;
					cards [i].sizeDelta = new Vector2 (223, 343);
					cards[i].transform.SetSiblingIndex (totalCards - 1);
					cards[i].GetComponent<Image> ().color = Color.white;
					SoundManager.instance.PlayCardMoveSound ();
				}
			} 
		}
		if(!dragging) {
			float newX = Mathf.Lerp (panel.anchoredPosition.x, -minDistance * closestIndex, Time.deltaTime * 5);
			panel.anchoredPosition = new Vector2 (newX, panel.anchoredPosition.y);
		}
	}

	public void EndDragging() {
		dragging = false;
	}

	public void StartDragging() {
		dragging = true;
	}

	public void ResetPositions() {
		panel.anchoredPosition = new Vector2 (3000, panel.anchoredPosition.y);
	}
}
