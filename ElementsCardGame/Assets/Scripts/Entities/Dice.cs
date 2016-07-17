using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Dice : MonoBehaviour {

	public Animator myAnimator;

	private Dictionary<int , System.Action> diceAnimationByFace;

	void Start() {
		diceAnimationByFace = new Dictionary<int, System.Action> () {
			{1, RollDiceFaceOneUp},
			{2, RollDiceFaceTwoUp},
			{3, RollDiceFaceThreeUp},
			{4, RollDiceFaceFourUp},
			{5, RollDiceFaceFiveUp},
			{6, RollDiceFaceSixUp}
		};
	}

	public void RollDice1(int advancedDieResult) {
		GamePlayController.instance.NotificationFromDiceRoll1 (advancedDieResult);
		diceAnimationByFace [advancedDieResult] ();
	}

	public void RollDice2(int advancedDieResult) {
		GamePlayController.instance.NotificationFromDiceRoll2 (advancedDieResult);
		diceAnimationByFace [advancedDieResult] ();
	}

	public void RollDice3(int advancedDieResult) {
		GamePlayController.instance.NotificationFromDiceRoll3 (advancedDieResult);
		diceAnimationByFace [advancedDieResult] ();
	}

	public void Reset() {
		ValidateAnimatorAndPlayAnimation ("Idle");
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
