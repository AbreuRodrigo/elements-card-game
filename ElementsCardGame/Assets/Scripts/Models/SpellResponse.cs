using UnityEngine;
using System.Collections;

public class SpellResponse {
	public string spellName;
	public SpellType spellType;
	public bool mockEffect;
	public bool bypass;
	public bool flowIsOnHold;
	public bool alwaysGoesFirst;

    public bool IsMeleeSpell {
        get { return spellType == SpellType.Melee; }
    }

    public bool IsSpecialSpell {
        get { return spellType == SpellType.Special; }
    }

	public SpellResponse(string spellName, SpellType spellType, bool mockEffect = false, bool bypass = false, bool flowIsOnHold = false, bool alwaysGoesFirst = false) {
		this.spellName = spellName;
		this.spellType = spellType;
		this.mockEffect = mockEffect;
		this.bypass = bypass;
		this.flowIsOnHold = flowIsOnHold;
		this.alwaysGoesFirst = alwaysGoesFirst;
	}
}