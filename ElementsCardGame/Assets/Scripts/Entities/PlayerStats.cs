using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerStats : MonoBehaviour {
	public Image shield;
	public Text playerNameUI;
	public Text playerHPUI;
	public Animator myAnimator;
	public Animator goFirstTokenAnimator;
	public Animator sunBurstGoFirstAnimator;

	public Sprite brokenShieldSprite;

	public void UpdateName(string name) {
		if(playerNameUI != null && name != null) {
			playerNameUI.text = name;
		}
	}

	public void UpdateHP(int hp) {
		if(playerHPUI != null) {
			playerHPUI.text = hp + "/80";

			if(hp <= 0 && shield != null && brokenShieldSprite != null) {
				playerHPUI.enabled = false;
				shield.sprite = brokenShieldSprite;
			}
		}
	}

	public void ShowPlayerStats() {
		if(myAnimator != null) {
			myAnimator.Play ("Show");
		}
	}

	public void HidePlayerStats() {
		if(myAnimator != null) {
			myAnimator.Play ("Hide");
		}
	}

	public void ShowGoFirstToken() {
		if(goFirstTokenAnimator != null) {
			goFirstTokenAnimator.Play ("Show");
		}
		if(sunBurstGoFirstAnimator != null) {
			sunBurstGoFirstAnimator.Play ("Show");
		}
	}
}