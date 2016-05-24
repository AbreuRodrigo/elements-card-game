using UnityEngine;
using System.Collections;

public class DiceManager : MonoBehaviour {
	public Dice die1;
	public Dice die2;
	public Dice die3;

	public void Roll1Die() {
		int r = RandomizeInt0To2 ();

		if (r == 0) {
			die1.RollDice ();
		} else if (r == 1) {
			die2.RollDice ();	
		} else {
			die3.RollDice ();
		}
	}

	public void Roll2Dice() {
		int r = RandomizeInt0To2 ();

		if (r == 0) {
			die1.RollDice ();
			die2.RollDice ();
		} else if (r == 1) {
			die1.RollDice ();
			die3.RollDice ();
		} else {
			die2.RollDice ();
			die3.RollDice ();
		}
	}

	public void Roll3Dice() {
		die1.RollDice ();
		die2.RollDice ();
		die3.RollDice ();
	}

	private int RandomizeInt0To2() {
		return Random.Range (0, 3);
	}
}