using UnityEngine;
using System.Collections;

public class SpellResponse {
	public string spellName;
	public SpellType spellType;
	public bool mockEffect;
	public bool bypass;

	public SpellResponse(string spellName, SpellType spellType, bool mockEffect = false, bool bypass = false) {
		this.spellName = spellName;
		this.spellType = spellType;
		this.mockEffect = mockEffect;
		this.bypass = bypass;
	}
}