using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpellManager : MonoBehaviour {

	private BloodSpell bloodSpell;

	Dictionary<CardElement, SpellBase> spellByElement;

	void Awake() {
		bloodSpell = new BloodSpell ();

		spellByElement = new Dictionary<CardElement, SpellBase> (10) {
			{ CardElement.Blood, bloodSpell }
		};
	}

	public void CastSpell(CardElement element, SpellSelection selection, Player target, Player source) {
		spellByElement [element].CastSpell (selection, target, source);
	}
}