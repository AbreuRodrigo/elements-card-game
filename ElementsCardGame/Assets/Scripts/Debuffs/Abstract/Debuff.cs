using UnityEngine;
using System.Collections;

public abstract class Debuff {
	protected DebuffType type;
	private int initialTurn;
	private int remainingTurns;
	private int elapsedTurns;
	private bool active;

	public Debuff(DebuffType type) {
		this.type = type;
	}

	public DebuffType Type {
		get { return type; }
	}

	public int ElapsedTurns {
		get { return elapsedTurns; }
		set { elapsedTurns = value; }
	}

	public int RemainingTurns {
		get { return remainingTurns; }
	}

	public void DecreaseRemainingTurn() {
		if(active && remainingTurns > 0) {
			remainingTurns--;

			if(remainingTurns == 0) {
				DeactivateDebuff ();
			}
		}
	}

	public int InitialTurn {
		get { return initialTurn; }
	}

	public bool IsActive {
		get { return active; }
	}

	public void ActivateDebuff(int currentTurn, int duration) {
		if(!active && remainingTurns == 0) {
			active = true;
			remainingTurns = duration;
		}
	}

	public void DeactivateDebuff() {
		if (active) {
			active = false;
			initialTurn = 0;
			elapsedTurns = 0;
			remainingTurns = 0;
		}
	}

	public abstract void ExecuteDebuff (Player host);
}