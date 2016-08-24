using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpellManager : MonoBehaviour {
	private BloodSpell bloodSpell;
	private DarkSpell darkSpell;
	private EarthSpell earthSpell;
	private FireSpell fireSpell;
	private IceSpell iceSpell;
	private LightiningSpell lightiningSpell;
	private LightSpell lightSpell;
	private NatureSpell natureSpell;
	private ShadowSpell shadowSpell;
	private WaterSpell waterSpell;
	private MagmaSpell magmaSpell;
	private ChaosSpell chaosSpell;
	private ZombieSpell zombieSpell;

	private Dictionary<CardElement, SpellBase> spellByElement;

	void Awake() {
		bloodSpell = new BloodSpell ();
		darkSpell = new DarkSpell ();
		earthSpell = new EarthSpell ();
		fireSpell = new FireSpell ();
		iceSpell = new IceSpell ();
		lightiningSpell = new LightiningSpell ();
		lightSpell = new LightSpell ();
		natureSpell = new NatureSpell ();
		shadowSpell = new ShadowSpell ();
		waterSpell = new WaterSpell ();
		magmaSpell = new MagmaSpell ();
		chaosSpell = new ChaosSpell ();
		zombieSpell = new ZombieSpell ();

		spellByElement = new Dictionary<CardElement, SpellBase> (12) {
			{ CardElement.Blood, bloodSpell },
			{ CardElement.Dark, darkSpell },
			{ CardElement.Earth, earthSpell },
			{ CardElement.Fire, fireSpell },
			{ CardElement.Ice, iceSpell },
			{ CardElement.Lightning, lightiningSpell },
			{ CardElement.Light, lightSpell },
			{ CardElement.Nature, natureSpell },
			{ CardElement.Shadow, shadowSpell },
			{ CardElement.Water, waterSpell },
			{ CardElement.Magma, magmaSpell },
			{ CardElement.Chaos, chaosSpell },
			{ CardElement.Zombie, zombieSpell }
		};
	}

	public SpellResponse PreviewSpell(Card card) {
		return PreviewSpell (card.element, card.selectedSpell);
	}

	public SpellResponse PreviewSpell(CardElement element, SpellSelection selection) {
		SpellBase spell = spellByElement [element];
		return spell.PreviewSpell (selection);
	}

	public void CastSpell(CardElement element, SpellSelection selection, Player target, Player source) {
		spellByElement [element].CastSpell (selection, target, source);
	}
}