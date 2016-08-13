using UnityEngine;
using System.Collections;

public class RefreshBuff : BuffDebuff {

	public RefreshBuff() : base(BuffDebuffType.Refresh) {
	}

	public override void ExecuteBuffDebuff(Player host) {
		if(IsActive && host != null) {
			int heal = 2;

			if(host.IsWet()) {
				heal += 1;
			}
			if(host.IsBurned()) {
				host.Debuffs.RemoveBurn ();
			}

			host.IncreaseHP (heal);
		}
	}
}
