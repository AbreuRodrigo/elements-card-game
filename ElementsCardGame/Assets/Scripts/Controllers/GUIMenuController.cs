using UnityEngine;
using System.Collections;

public class GUIMenuController : MonoBehaviour {
	public static GUIMenuController instance;

	public GameObject libraryCanvas;

	public GameObject bgDecoration1Obj;
	public GameObject bgDecoration2Obj;
	public GameObject singlePlayerBtnObj;
	public GameObject multiplayerBtnObj;
	public GameObject deckBuilderBtnObj;
	public GameObject libraryBtnObj;
	public GameObject creditsBtnObj;

	private bool hasAlreadyCalledLibrary;
	private bool hasAlreadyCalledDeckBuilder;
	private bool hasAlreadyCalledCredits;

	[Header("Components")]
	public CardLibraryScrollView cardLibraryScroller;
	public MessageManager messageManagerObj;
	public InteractionBlocker interactionBlocker;
	public Credits credits;
	public LogoBehaviour logo;

	void Awake() {
		if(instance == null) {
			instance = this;
		}
		libraryCanvas.SetActive (true);
		libraryCanvas.SetActive (false);
	}

	public void PressCloseLibraryButton() {
		SoundManager.instance.PlayClickSound ();
		HideLibrary ();
	}

	public void HideUI() {
		bgDecoration1Obj.SetActive (false);
		bgDecoration2Obj.SetActive (false);
		singlePlayerBtnObj.SetActive (false);
		multiplayerBtnObj.SetActive (false);
		deckBuilderBtnObj.SetActive (false);
		libraryBtnObj.SetActive (false);
		creditsBtnObj.SetActive (false);
	}

	public void ShowUI() {
		bgDecoration1Obj.SetActive (true);
		bgDecoration2Obj.SetActive (true);
		singlePlayerBtnObj.SetActive (true);
		multiplayerBtnObj.SetActive (true);
		deckBuilderBtnObj.SetActive (true);
		libraryBtnObj.SetActive (true);
		creditsBtnObj.SetActive (true);
	}

	public void ShowLibrary() {
		if(!hasAlreadyCalledLibrary) {
			cardLibraryScroller.ResetPositions ();
			libraryCanvas.SetActive (true);
			hasAlreadyCalledLibrary = true;
		}
	}

	public void HideLibrary() {
		if (hasAlreadyCalledLibrary) {
			libraryCanvas.SetActive (false);
			ShowUI ();
			hasAlreadyCalledLibrary = false;
		}
	}

	public void ShowLogo() {
		if(logo != null) {
			logo.gameObject.SetActive (true);
		}
	}

	public void HideLogo() {
		if(logo != null) {
			logo.gameObject.SetActive (false);
		}
	}

	public void FadeScreenOut() {
		if(interactionBlocker != null) {
			interactionBlocker.Enable ();
			interactionBlocker.FadeOut ();
		}
	}

	public void ShowDeckBuilder() {
		if(!hasAlreadyCalledDeckBuilder) {
			hasAlreadyCalledDeckBuilder = true;
			FadeScreenOut ();
		}
	}

	public void ShowCredits() {
		if(!hasAlreadyCalledCredits) {
			hasAlreadyCalledCredits = true;

			if (credits != null) {
				credits.Show ();
			}
		}
	}

	public void HideCredits() {
		if (hasAlreadyCalledCredits) {
			hasAlreadyCalledCredits = false;

			SoundManager.instance.PlayClickSound ();

			if (credits != null) {
				credits.Hide ();
			}
		}
	}
}