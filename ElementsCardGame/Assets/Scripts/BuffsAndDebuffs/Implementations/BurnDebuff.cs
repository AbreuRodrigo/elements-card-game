using UnityEngine;
using System.Collections;

public class BurnDebuff : BuffDebuff {

	public BurnDebuff() : base(BuffDebuffType.Burn) {
	}

	public override void ExecuteBuffDebuff(Player host) {
		if(IsActive && host != null) {
			host.DecreaseHP (2);

			if (HasCounter) {
				DecreaseRemainingTurn ();
				host.DecreaseBurnDebuff ();
				host.HideBurnDebuffOnZeroTurnCounters (RemainingTurns);
			}
		}
	}
}