using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {
	public static SoundManager instance;

	public AudioClip cardMove;
	public AudioClip click;
	public AudioClip coinFlip;
	public AudioClip yourTurn;
	public AudioClip yourTurnHiding;
	public AudioClip magicCircle;
	public AudioClip slice;
	public AudioClip impact;
	public AudioClip brightBell;
	public AudioClip pageFlip;

	void Awake() {
		if (instance == null) {
			instance = this;
		}
		DontDestroyOnLoad (gameObject);
	}

	public void PlayCardMoveSound() {
		PlaySound (cardMove);
	}

	public void PlayClickSound() {
		PlaySound (click);
	}

	public void PlayCoinFlip() {
		PlaySound (coinFlip);
	}

	public void PlayYourTurn() {
		PlaySound (yourTurn);
	}

	public void PlayYourTurnHiding() {
		PlaySound (yourTurnHiding);
	}

	public void PlayMagicCircle() {
		PlaySound (magicCircle);
	}

	public void PlaySlice() {
		PlaySound (slice);
	}

	public void PlayImpact() {
		PlaySound (impact);
	}

	public void PlayBrightBell() {
		PlaySound (brightBell);
	}

	public void PlayPageFlip() {
		PlaySound (pageFlip);
	}

	private void PlaySound (AudioClip clip) {
		if (clip) {
			AudioSource.PlayClipAtPoint (clip, Vector3.zero);
		}
	}
}
