using UnityEngine;
using System.Collections;

public class BleedDebuff : BuffDebuff {

	public BleedDebuff() : base(BuffDebuffType.Bleed) {
	}

	public override void ExecuteBuffDebuff(Player host) {
		if(IsActive && host != null) {
			host.DecreaseHP (1);

			if (HasCounter) {
				DecreaseRemainingTurn ();
				host.DecreaseBleedDebuff ();
				host.HideBleedDebuffOnZeroTurnCounters (RemainingTurns);
			}

			ElapsedTurns++;
		}
	}
}