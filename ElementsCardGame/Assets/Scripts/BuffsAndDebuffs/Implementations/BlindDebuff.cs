﻿using UnityEngine;
using System.Collections;

public class BlindDebuff : BuffDebuff {

	public BlindDebuff() : base(BuffDebuffType.Blind) {	
	}

	public override void ExecuteBuffDebuff(Player host) {
		if(IsActive && host != null) {
			if (HasCounter) {
				DecreaseRemainingTurn ();
				host.DecreaseBlindBuff ();
				host.HideBlindDebuffOnZeroTurnCounters (RemainingTurns);
			}
		}
	}
}