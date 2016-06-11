using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameMenuController : MonoBehaviour {
	public static GameMenuController instance;

	public GameObject soundManagerPrefab;

	[Header("Components")]
	public MessageManager messageManager;
	public ParticleSystem particleWhisps;

	private bool singlePlayerWasStarted;

	void Awake() {
		if(instance == null) {
			instance = this;
		}
		Application.targetFrameRate = 60;
	}

	void Start() {
		if(SoundManager.instance == null && soundManagerPrefab != null) {
			Instantiate (soundManagerPrefab);			
		}

		StartCoroutine (WaitAndStartParticles ());
	}

	public void SinglePlayerButtonPress() {
		if(!singlePlayerWasStarted) {
			particleWhisps.Stop ();
			singlePlayerWasStarted = true;
			SoundManager.instance.PlayClickSound ();
			GUIMenuController.instance.FadeScreenOut ();
			StartCoroutine (WaitAndLoadGameSinglePlayerMode());
		}
	}

	public void MultiplayerButtonPressed() {
		if(messageManager != null) {
			messageManager.ShowKickstartMessage ();
			SoundManager.instance.PlayClickSound ();
		}
	}

	public void DeckBuilderButtonPress() {
		StartCoroutine (PrepareAndLoadDeckBuilder());
	}

	public void LibraryButtonPress() {
		StartCoroutine (PrepareAndShowCardLibrary());
	}

	IEnumerator PrepareAndShowCardLibrary() {
		SoundManager.instance.PlayClickSound ();

		yield return new WaitForSeconds (0.5f);

		GUIMenuController.instance.HideUI ();
		GUIMenuController.instance.ShowLibrary ();
	}

	IEnumerator PrepareAndLoadDeckBuilder() {
		SoundManager.instance.PlayClickSound ();
		GUIMenuController.instance.ShowDeckBuilder ();

		yield return new WaitForSeconds (1f);

		SceneManager.LoadScene ("DeckBuilder");
	}

	IEnumerator WaitAndStartParticles() {
		yield return new WaitForSeconds (2);

		if(particleWhisps != null) {
			particleWhisps.Play ();
		}
	}

	IEnumerator WaitAndLoadGameSinglePlayerMode() {
		yield return new WaitForSeconds (2);

		SoundManager.instance.ChangeToBattleMusic ();

		yield return new WaitForSeconds (2);

		SceneManager.LoadScene ("SinglePlayer");
	}
}