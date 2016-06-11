using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DeckChangeButton : MonoBehaviour {
	public int index;
	public bool selected;
	public Image myImage;
	public RectTransform myRect;

	public DeckChangeManager deckChangeManager;

	public void OnClick() {
		RunButtonChange ();
		SoundManager.instance.PlayClickSound ();
	}

	public void RunButtonChange() {
		if (deckChangeManager != null) {
			deckChangeManager.ReceiveDeckChangeButtonPress (this);
		}
	}
}