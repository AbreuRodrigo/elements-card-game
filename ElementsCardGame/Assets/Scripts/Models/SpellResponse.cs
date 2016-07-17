using UnityEngine;
using System.Collections;

public class SpellResponse {
	public string spellName;
	public SpellType spellType;
	public bool mockEffect;

	public SpellResponse ResetResponse(string spellName, SpellType spellType) {
		this.spellName = spellName;
		this.spellType = spellType;
		this.mockEffect = false;

		return this;
	}
}