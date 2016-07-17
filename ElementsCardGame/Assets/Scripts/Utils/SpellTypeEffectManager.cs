using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpellTypeEffectManager : MonoBehaviour {
	public Animator myAnimator;
	public SpriteRenderer spellTypeSprite;
	public TextMesh spellNameText;

	public Sprite meleeType;
	public Sprite specialType;
	public Sprite cureType;
	public Sprite healType;
	public Sprite shieldType;
	public Sprite buffType;

	private Dictionary<SpellType, System.Action> actionBySpellType;

	void Awake() {
		if(myAnimator == null) {
			myAnimator = GetComponent<Animator> ();
		}

		actionBySpellType = new Dictionary<SpellType, System.Action> () {
			{SpellType.Melee, ShowAsMeleeType},
			{SpellType.Special, ShowAsSpecialType},
			{SpellType.Cure, ShowAsCureType},
			{SpellType.Heal, ShowAsHealType},
			{SpellType.Shield, ShowAsShieldType},
			{SpellType.Buff, ShowAsBuffType}
		};
	}

	public void ShowSpellType(SpellType type, string spellName, Vector3 spawnPoint) {
		transform.position = spawnPoint;

		if (spellNameText != null) {
			spellNameText.text = spellName;
		}

		actionBySpellType[type]();
	}

	private void Show(Sprite sprite) {
		if(sprite != null) {
			spellTypeSprite.sprite = sprite;

			if(myAnimator != null) {
				myAnimator.Play ("Show");
			}
		}
	}

	private void ShowAsMeleeType() {		
		Show (meleeType);
	}

	private void ShowAsSpecialType() {
		Show (specialType);
	}

	private void ShowAsCureType() {
		Show (cureType);
	}

	private void ShowAsHealType() {
		Show (healType);
	}

	private void ShowAsShieldType() {
		Show (shieldType);
	}

	private void ShowAsBuffType() {
		Show (buffType);
	}
}