using UnityEngine;
using System.Collections;

public class GameMenuController : MonoBehaviour {
	public static GameMenuController instance;

	[Header("Components")]
	public MessageManager messageManager;
	public ParticleSystem particleWhisps;

	private bool singlePlayerWasStarted;

	void Awake() {
		if(instance == null) {
			instance = this;
		}
	}

	void Start() {
		StartCoroutine (WaitAndStartParticles ());
	}

	public void SinglePlayerButtonPress() {
		if(!singlePlayerWasStarted) {
			singlePlayerWasStarted = true;
			SoundManager.instance.PlayClickSound ();
			GUIMenuController.instance.FadeScreenOut ();
			particleWhisps.Stop ();
		}
	}

	public void MultiplayerButtonPressed() {
		if(messageManager != null) {
			messageManager.ShowKickstartMessage ();
			SoundManager.instance.PlayClickSound ();
		}
	}

	public void DeckBuilderButtonPress() {
		SoundManager.instance.PlayClickSound ();
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

	IEnumerator WaitAndStartParticles() {
		yield return new WaitForSeconds (2);

		if(particleWhisps != null) {
			particleWhisps.Play ();
		}
	}
}