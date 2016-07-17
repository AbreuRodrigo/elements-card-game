using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurnManager : MonoBehaviour {
	[SerializeField]
	private int currentTurn = 1;
	public int CurrentTurn {
		get { return currentTurn; }
	}

	[SerializeField]
	private TurnPhase phase = TurnPhase.BeginningPhase;
	public TurnPhase Phase {
		get { return phase; }
	}

	private Dictionary<TurnPhase, TurnPhase> phaseProgression;

	void Awake() {
		phaseProgression = new Dictionary<TurnPhase, TurnPhase>(4) {
			{ TurnPhase.BeginningPhase, TurnPhase.PreCombatPhase },
			{ TurnPhase.PreCombatPhase, TurnPhase.CombatPhase },
			{ TurnPhase.CombatPhase, TurnPhase.EndCombatPhase },
			{ TurnPhase.EndCombatPhase, TurnPhase.BeginningPhase }
		};
	}

	public void NextPhase() {
		if(phase.Equals(TurnPhase.EndCombatPhase)) {
			currentTurn++;
		}

		phase = phaseProgression[phase];
	}

	public bool IsBeginningPhase() {
		return phase.Equals (TurnPhase.BeginningPhase);
	}

	public bool IsPreCombatPhase() {
		return phase.Equals (TurnPhase.PreCombatPhase);
	}

	public bool IsCombatPhase() {
		return phase.Equals (TurnPhase.CombatPhase);
	}

	public bool IsEndCombatPhase() {
		return phase.Equals (TurnPhase.EndCombatPhase);
	}
}