using UnityEngine;
using System.Collections;

public class StaticBuff : BuffDebuff {

	public StaticBuff() : base(BuffDebuffType.Static) {
	}

	public override void ExecuteBuffDebuff(Player host) {
		if(IsActive && host != null) {
			DecreaseRemainingTurn ();
			host.DecreaseStaticBuff ();
			ElapsedTurns++;
			host.HideStaticDebuffOnZeroTurnCounters (RemainingTurns);
		}
	}
}