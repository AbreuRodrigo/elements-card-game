using UnityEngine;
using System.Collections;

public class BleedDebuff : Debuff {

	public BleedDebuff() : base(DebuffType.Bleed) {		
	}

	public override void ExecuteDebuff(Player host) {
		if(IsActive && host != null && RemainingTurns > 0) {
			host.DecreaseHP (1);
			ElapsedTurns++;
		}
	}
}