﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DebuffMarker : MonoBehaviour {
	public int index;
	public bool active;
	public Text counterUI;
	public RectTransform rect;

	public void IncreaseCounter(int amount) {
		if(counterUI != null && active) {
			int value = int.Parse (counterUI.text) + amount;

			if(value >= 0) {
				counterUI.text = "" + value;
			}
		}
	}

	public void DecreaseCounter() {
		if(counterUI != null && counterUI.enabled && active) {
			int value = int.Parse (counterUI.text);

			if(value > 0) {
				counterUI.text = "" + (--value);
			}
		}
	}

	public void Show(int duration) {
		active = true;

		if (counterUI != null) {
			if (duration > 0) {
				counterUI.enabled = true;
				counterUI.text = "" + duration;
			} else {
				counterUI.enabled = false;
			}
		}

		rect.localScale = new Vector3 (1, 1, 1);
	}

	public void Hide() {
		active = false;
		rect.localScale = Vector3.zero;
	}
}
