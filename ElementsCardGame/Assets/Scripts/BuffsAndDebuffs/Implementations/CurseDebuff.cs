using UnityEngine;
using System.Collections;

public class CurseDebuff : BuffDebuff {

	public CurseDebuff() : base(BuffDebuffType.Curse) {

	}

	public override void ExecuteBuffDebuff(Player host) {
		if(IsActive && host != null) {
			if(HasCounter) {
				DecreaseRemainingTurn ();
				host.DecreaseCurseDebuff ();
			}
			ElapsedTurns++;

			if (HasCounter) {
				host.HideCurseDebuffOnZeroTurnCounters (RemainingTurns);
			}
		}
	}
}