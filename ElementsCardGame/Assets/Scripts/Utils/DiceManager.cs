using UnityEngine;
using System.Collections;

public class DiceManager : MonoBehaviour {
	public Dice die1;
	public Dice die2;
	public Dice die3;

	public void Roll1Die(int advancedDieResult) {
		int r = RandomizeInt0To2 ();

		if (r == 0) {
			die1.RollDice1 (advancedDieResult);
		} else if (r == 1) {
			die2.RollDice2 (advancedDieResult);	
		} else {
			die3.RollDice3 (advancedDieResult);
		}
	}

	public void Roll2Dice(int advancedDieResult1, int advancedDieResult2) {
		int r = RandomizeInt0To2 ();

		if (r == 0) {
			die1.RollDice1 (advancedDieResult1);
			die2.RollDice2 (advancedDieResult2);
		} else if (r == 1) {
			die1.RollDice1 (advancedDieResult1);
			die3.RollDice3 (advancedDieResult2);
		} else {
			die2.RollDice2 (advancedDieResult1);
			die3.RollDice3 (advancedDieResult2);
		}
	}

	public void Roll3Dice(int advancedDieResult1, int advancedDieResult2, int advancedDieResult3) {
		die1.RollDice1 (advancedDieResult1);
		die2.RollDice2 (advancedDieResult2);
		die3.RollDice3 (advancedDieResult3);
	}

	public int AdvancedDieResult() {
		return Random.Range (1, 7);
	}

	public void ResetDice() {
		die1.Reset ();
		die2.Reset ();
		die3.Reset ();
	}

	private int RandomizeInt0To2() {
		return Random.Range (0, 3);
	}
}