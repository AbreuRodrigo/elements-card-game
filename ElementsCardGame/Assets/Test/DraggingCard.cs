using UnityEngine;
using System.Collections;

public class DraggingCard : MonoBehaviour {
	public SpriteRenderer spriteRenderer;
	public Animator myAnimator;

	private CardType type;
	public CardType Type {
		get { return type; }
	}

	private CardElement element;
	public CardElement Element {
		get { return element; }
	}

	private bool enabled;
	public bool Enabled {
		get { return enabled; }
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject.tag == "DeckSide") {
			enabled = false;
			FadeOut ();
			DeckBuilder.Instance.AddDraggingCardToCurrentDeck ();
		}
	}

	void LateUpdate () {
		if(enabled) {
			Vector3 p = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			p.z = 0;

			transform.position = p;
		}
	}

	public void CancelDragging() {
		if (enabled) {
			enabled = false;
			FadeOut ();
		}
	}

	public void ResetSprite(Sprite sprite, CardElement element, CardType type) {
		spriteRenderer.enabled = true;
		spriteRenderer.sprite = sprite;

		this.element = element;
		this.type = type;

		Idle ();
	}

	public void Enable() {
		enabled = true;
	}

	public void Disable() {
		enabled = false;
		DeckBuilder.Instance.EmitCardParticles ();
		SoundManager.instance.PlayMagicDust ();
		spriteRenderer.enabled = false;
	}

	private void Idle() {
		if(myAnimator != null) {
			myAnimator.Play ("Idle");
		}
	}

	private void FadeOut() {
		if(myAnimator != null) {
			myAnimator.Play ("FadeOut");
		}
	}
}