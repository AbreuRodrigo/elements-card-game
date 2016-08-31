using UnityEngine;
using System.Collections;

public class BurnDebuff : BuffDebuff {

	public BurnDebuff() : base(BuffDebuffType.Burn) {

	}

	public override void ExecuteBuffDebuff(Player host) {
		if(IsActive && host != null) {
			if (HasCounter) {
				DecreaseRemainingTurn ();
				host.DecreaseBurnDebuff ();
			}

			host.DecreaseHP (2);
			ElapsedTurns++;

			if (HasCounter) {
				host.HideBurnDebuffOnZeroTurnCounters (RemainingTurns);
			}
		}
	}
}