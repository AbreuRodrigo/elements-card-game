using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DeckBuilderController : MonoBehaviour {

	public GameObject soundManagerPrefab;
	public InteractionBlocker interactionBlocker;

	void Start() {
		if(SoundManager.instance == null && soundManagerPrefab != null) {
			Instantiate (soundManagerPrefab);
		}
	}

	void OnApplicationQuit() {
		SaveAndLeaveDeckBuilder ();
	}

	public void GoBackToMenu() {
		StartCoroutine (GoBackToMenuRoutine ());
	}

	private void SaveAndLeaveDeckBuilder() {
		if(DeckBuilder.Instance != null) {
			int currentValidDeckIndex = DeckBuilder.Instance.currentDeckIndex;

			if(currentValidDeckIndex < 0 || currentValidDeckIndex > 2) {
				currentValidDeckIndex = 0;
			}

			DeckData selectedDeck = DeckBuilder.Instance.GetDeckDataByIndex (currentValidDeckIndex);

			if (selectedDeck != null && !selectedDeck.IsDeckComplete) {
				currentValidDeckIndex = 0;
			} 

			PersistenceManager.Instance.SavePlayerDeckInUse (currentValidDeckIndex);

			DeckData deck2 = DeckBuilder.Instance.GetDeckDataByIndex (1);
			deck2.deckName = DeckBuilder.Instance.decks [1].DeckName;
			ValidateAndSaveDeck (deck2, "Deck 2", DeckBuilder.DECK2_FILE_NAME);

			DeckData deck3 = DeckBuilder.Instance.GetDeckDataByIndex (2);
			deck3.deckName = DeckBuilder.Instance.decks [2].DeckName;
			ValidateAndSaveDeck (deck3, "Deck 3", DeckBuilder.DECK3_FILE_NAME);
		}
	}

	IEnumerator GoBackToMenuRoutine() {
		SoundManager.instance.PlayClickSound ();

		interactionBlocker.Enable ();
		interactionBlocker.FadeOut ();

		yield return new WaitForSeconds (1f);

		SaveAndLeaveDeckBuilder ();

		yield return new WaitForSeconds (1f);

		SceneManager.LoadScene ("Menu");
	}

	private void ValidateAndSaveDeck(DeckData deck, string defaultName, string filePath) {
		PersistenceManager.Instance.SaveData (filePath, deck);
	}
}