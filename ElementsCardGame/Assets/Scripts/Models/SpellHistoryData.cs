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

	private SpellType spellType;
	public SpellType SpellType {
		get { return spellType; }
	} 

	public SpellHistoryData(CardElement element, SpellSelection selectedSpell, SpellType spellType) {
		Reset (element, selectedSpell, spellType);
	}

	public void Reset(CardElement element, SpellSelection selectedSpell, SpellType spellType) {
		this.element = element;
		this.selectedSpell = selectedSpell;
		this.spellType = spellType;
	}
}