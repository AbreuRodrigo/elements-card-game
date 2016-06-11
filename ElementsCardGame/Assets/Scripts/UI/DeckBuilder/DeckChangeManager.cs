using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DeckChangeManager : MonoBehaviour {
	public DeckChangeButton selectedDeckButton;

	public void ReceiveDeckChangeButtonPress(DeckChangeButton selectedButton) {
		MoveDecks(selectedButton);
	}

	void MoveDecks(DeckChangeButton selectedButton) {
		Sprite deselectedSprite = selectedButton.myImage.sprite;
		selectedButton.myImage.sprite = selectedDeckButton.myImage.sprite;

		selectedDeckButton.myImage.sprite = deselectedSprite;

		selectedDeckButton.selected = false;
		selectedButton.selected = true;

		selectedDeckButton = selectedButton;

		DeckBuilder.Instance.MoveUIToSpecificDeckStructureByIndex (selectedButton.index);
	}
}