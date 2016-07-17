using UnityEngine;

public class SpellHistoryData {
	private CardElement element;
	public CardElement Element {
		get { return element; }	
	}

	private SpellSelection selectedSpell;
	public SpellSelection SelectedSpell {
		get { return selectedSpell; }
	}

	public SpellHistoryData(CardElement element, SpellSelection selectedSpell) {
		Reset (element, selectedSpell);
	}

	public void Reset(CardElement element, SpellSelection selectedSpell) {
		this.element = element;
		this.selectedSpell = selectedSpell;
	}
}