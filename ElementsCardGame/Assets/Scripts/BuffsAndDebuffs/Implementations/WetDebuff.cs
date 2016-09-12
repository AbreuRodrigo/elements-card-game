using UnityEngine;
using System.Collections;

public class WetDebuff : BuffDebuff {

	public WetDebuff() : base(BuffDebuffType.Wet) {
	}

	public override void ExecuteBuffDebuff(Player host) {
		if(IsActive && host != null) {
			if (HasCounter) {
				DecreaseRemainingTurn ();
				host.DecreaseWetBuff ();
				host.HideWetDebuffOnZeroTurnCounters (RemainingTurns);
			}
		}
	}
}
