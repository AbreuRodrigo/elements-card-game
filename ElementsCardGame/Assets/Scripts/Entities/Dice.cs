using UnityEngine;
using System.Collections;

public class Dice : MonoBehaviour {

	public Animator myAnimator;

	public void RollDice() {
		int result = Random.Range (1, 7);
		GamePlayController.instance.NotificationFromDiceRoll (result);
	}

	private void RollDiceFaceOneUp() {
		ValidateAnimatorAndPlayAnimation ("RollDiceFaceOneUp");
	}

	private void RollDiceFaceTwoUp() {
		ValidateAnimatorAndPlayAnimation ("RollDiceFaceTwoUp");
	}

	private void RollDiceFaceThreeUp() {
		ValidateAnimatorAndPlayAnimation ("RollDiceFaceThreeUp");
	}

	private void RollDiceFaceFourUp() {
		ValidateAnimatorAndPlayAnimation ("RollDiceFaceFourUp");
	}

	private void RollDiceFaceFiveUp() {
		ValidateAnimatorAndPlayAnimation ("RollDiceFaceFiveUp");
	}

	private void RollDiceFaceSixUp() {
		ValidateAnimatorAndPlayAnimation ("RollDiceFaceSixUp");
	}

	private void ValidateAnimatorAndPlayAnimation(string animationName) {
		if(myAnimator != null) {
			myAnimator.Play (animationName);
		}
	}
}
