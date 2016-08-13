using UnityEngine;
using System.Collections;

public class PoisonDebuff : BuffDebuff {

	public PoisonDebuff() : base(BuffDebuffType.Poison) {
	}

	public override void ExecuteBuffDebuff(Player host) {
		if(IsActive && host != null) {
			host.DecreaseHP (3);
			ElapsedTurns++;
		}
	}
}
