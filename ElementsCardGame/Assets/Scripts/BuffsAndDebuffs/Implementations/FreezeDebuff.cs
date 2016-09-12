using UnityEngine;
using System.Collections;

public class FreezeDebuff : BuffDebuff {

	public FreezeDebuff() : base(BuffDebuffType.Freeze) {		
	}

	public override void ExecuteBuffDebuff(Player host) {
		if(IsActive && host != null) {
			if (HasCounter) {
				DecreaseRemainingTurn ();
				host.DecreaseFreezeBuff ();
				host.HideFreezeDebuffOnZeroTurnCounters (RemainingTurns);
			}
		}
	}
}