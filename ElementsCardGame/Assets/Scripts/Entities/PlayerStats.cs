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
	public int nextHPAlteration;
	public Player player;
	public DebuffManager debuffManager;

	public Sprite brokenShieldSprite;

	void OnCollisionEnter (Collision obj) {
		if (obj != null) {
			ShakePlayerStats ();
			UpdateHP ();
			player.opponent.stats.UpdateHP ();
		}
	}

	public void UpdateName(string name) {
		if(playerNameUI != null && name != null) {
			playerNameUI.text = name;
		}
	}

	public void UpdateHP() {
		if(player != null && playerHPUI != null) {
			playerHPUI.text = player.HP + "/80";

			if(player.HP <= 0 && shield != null && brokenShieldSprite != null) {
				playerHPUI.enabled = false;
				shield.sprite = brokenShieldSprite;

				GamePlayController.instance.NotificationFromPlayerDestroyed (player);
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

	public void ShakePlayerStats() {
		if(myAnimator != null) {
			myAnimator.Play ("Shake");
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