using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {
	public static SoundManager instance;

	public AudioSource source;

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
	public AudioClip splat;
	public AudioClip falling;

	public AudioClip menuMusic;
	public AudioClip battleMusic;

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

	public void PlaySplat() {
		PlaySound (splat);
	}

	public void PlayFalling() {
		PlaySound (falling);	
	}

	public void ChangeToMenuMusic() {
		StartCoroutine(FadeMusicOutAndPlayAnother(menuMusic));
	}

	public void ChangeToBattleMusic() {
		StartCoroutine(FadeMusicOutAndPlayAnother(battleMusic));
	}

	private void PlaySound (AudioClip clip) {
		if (clip) {
			AudioSource.PlayClipAtPoint (clip, Vector3.zero);
		}
	}

	IEnumerator FadeMusicOutAndPlayAnother(AudioClip newMusic) {
		float volume = source.volume;

		while(volume > 0) {

			volume -= 0.01f;

			yield return new WaitForSeconds (0.1f);
		}

		source.clip = newMusic;
		source.Play ();

		while(volume < 0.1f) {

			volume += 0.01f;

			yield return new WaitForSeconds (0.1f);
		}
	}
}