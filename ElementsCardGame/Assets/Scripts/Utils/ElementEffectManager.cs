using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ElementEffectManager : MonoBehaviour {
	public static ElementEffectManager instance;

	[Header("Blood Spell Effects")]
	public GameObject tinyBloodSpell;
	public GameObject smallBloodSpell;
	public GameObject normalBloodSpell;
	public GameObject megaBloodSpell;

	[Header("Dark Spell Effects")]
	public GameObject tinyDarkSpell;
	public GameObject smallDarkSpell;
	public GameObject normalDarkSpell;
	public GameObject megaDarkSpell;

	[Header("Earth Spell Effects")]
	public GameObject tinyEarthSpell;
	public GameObject smallEarthSpell;
	public GameObject normalEarthSpell;
	public GameObject megaEarthSpell;
	public GameObject wallEarthSpell;

	[Header("Fire Spell Effects")]
	public GameObject tinyFireSpell;
	public GameObject smallFireSpell;
	public GameObject normalFireSpell;
	public GameObject megaFireSpell;
	public GameObject auraFireSpell;

	[Header("Ice Spell Effects")]
	public GameObject tinyIceSpell;
	public GameObject smallIceSpell;
	public GameObject normalIceSpell;
	public GameObject megaIceSpell;
	public GameObject wallIceSpell;

	[Header("Light Spell Effects")]
	public GameObject tinyLightSpell;
	public GameObject smallLightSpell;
	public GameObject normalLightSpell;
	public GameObject megalLightSpell;
	public GameObject auraLightSpell;

	[Header("Lightning Spell Effects")]
	public GameObject tinyLightningSpell;
	public GameObject smallLightiningSpell;
	public GameObject normalLightningSpell;
	public GameObject megaLightningSpell;
	public GameObject auraLightningSpell;

	[Header("Nature Spell Effects")]
	public GameObject tinyNatureSpell;
	public GameObject smallNatureSpell;
	public GameObject normalNatureSpell;
	public GameObject megaNatureSpell;
	public GameObject auraNatureSpell;

	[Header("Shadow Spell Effects")]
	public GameObject tinyShadowSpell;
	public GameObject smallShadowSpell;
	public GameObject normalShadowSpell;
	public GameObject megaShadowSpell;
	public GameObject auraShadowSpell;

	[Header("Water Spell Effects")]
	public GameObject tinyWaterSpell;
	public GameObject smallWaterSpell;
	public GameObject normalWaterSpell;
	public GameObject megaWaterSpell;
	public GameObject auraWaterSpell;

	[Header("Zombie Spell Effects")]
	public GameObject infectedBite;
	public GameObject canibalize;

	[Header("Mixed Magic Circles")]
	public GameObject magmaCircleEffect;
	public GameObject chaosCircleEffect;
	public GameObject zombieCircleEffect;

	public Transform spawnPosition;
	public float speed = 1000;

	private GameObject currentSpell;
	private Dictionary<string, GameObject> effectByElementAndSelection;
	private Dictionary<string, System.Action> effectByMixedRequirements;
	private Rigidbody currentRigidBody;
	private ProjectileScript projectileScript;

	void Awake() {
		if(instance == null) {
			instance = this;
		}
	}

	void Start() {
		effectByElementAndSelection = new Dictionary<string, GameObject> (40) {
			{"Blood - Circle", smallBloodSpell},
			{"Blood - Square", tinyBloodSpell},
			{"Blood - Rhombus", normalBloodSpell},
			{"Chaos - Circle", normalFireSpell},
			{"Chaos - Square", normalShadowSpell},
			{"Dark - Circle", tinyDarkSpell},
			{"Dark - Square", null},
			{"Dark - Rhombus", null},
			{"Earth - Circle", megaEarthSpell},
			{"Earth - Square", wallEarthSpell},
			{"Earth - Rhombus", normalEarthSpell},
			{"Fire - Circle", smallFireSpell},
			{"Fire - Square", megaFireSpell},
			{"Fire - Rhombus", auraFireSpell},
			{"Ice - Circle", normalIceSpell},
			{"Ice - Square", wallIceSpell},
			{"Ice - Rhombus", smallIceSpell},
			{"Light - Circle", normalLightSpell},
			{"Light - Square", auraLightSpell},
			{"Light - Rhombus", auraLightSpell},
			{"Lightning - Circle", normalLightningSpell},
			{"Lightning - Square", normalLightningSpell},
			{"Lightning - Rhombus", auraLightningSpell},
			{"Magma - Circle", megaFireSpell},
			{"Magma - Square", megaEarthSpell},
			{"Nature - Circle", normalNatureSpell},
			{"Nature - Square", smallNatureSpell},
			{"Nature - Rhombus", auraNatureSpell},
			{"Shadow - Circle", normalShadowSpell},
			{"Shadow - Square", smallShadowSpell},
			{"Shadow - Rhombus", auraShadowSpell},
			{"Water - Circle", normalWaterSpell},
			{"Water - Square", auraWaterSpell},
			{"Water - Rhombus", megaWaterSpell},
			{"Zombie - Circle", infectedBite},
			{"Zombie - Square", canibalize}
		};

		effectByMixedRequirements = new Dictionary<string, System.Action> (6) {
			{"Fire - Earth", InvokeMagmaEffect},
			{"Earth - Fire", InvokeMagmaEffect},
			{"Fire - Shadow", InvokeChaosEffect},
			{"Shadow - Fire", InvokeChaosEffect},
			{"Blood - Shadow", InvokeZombieEffect},
			{"Shadow - Blood", InvokeZombieEffect}
		};
	}

	public GameObject ThrowElementEffect(CardElement element, SpellSelection selection, 
		Player sourcePlayer, Vector3 source, Vector3 target) {

		currentSpell = effectByElementAndSelection [element + " - " + selection];

		if(currentSpell != null) {
			if ("Wall".Equals (currentSpell.tag)) {
				InvokeWallEffectByCardElementAndSelection (currentSpell, source, sourcePlayer);
			} else if ("Aura".Equals (currentSpell.tag)) {
				InvokeAuraEffectByCardElementAndSelection (currentSpell, source);
			}else {
				ThrowProjectileEffectByCardElementAndSelection (currentSpell, source, target);
			}
		}

		return currentSpell;
	}

	public void InvokeMixedEffectByRequirements(CardElement requirement1, CardElement requirement2) {
		effectByMixedRequirements [requirement1 + " - " + requirement2].Invoke();
	}

	private void InvokeMagmaEffect() {
		if(magmaCircleEffect != null) {
			Instantiate (magmaCircleEffect);
			CameraManager.instance.Shake ();
			SoundManager.instance.PlayFireImpact ();
			SoundManager.instance.PlayEarthImpact ();
		}
	}

	private void InvokeChaosEffect() {
		if(chaosCircleEffect != null) {
			Instantiate (chaosCircleEffect);
			CameraManager.instance.Shake ();
			SoundManager.instance.PlayFireImpact ();
			SoundManager.instance.PlayShadowImpact ();
		}
	}

	private void InvokeZombieEffect() {
		if(zombieCircleEffect != null) {
			Instantiate (zombieCircleEffect);
			CameraManager.instance.Shake ();
			SoundManager.instance.PlayBloodImpact ();
			SoundManager.instance.PlayShadowImpact ();
		}
	}

	void ThrowProjectileEffectByCardElementAndSelection (GameObject effectObject, Vector3 source, Vector3 target) {
		Vector3 origin = source;
		origin.z = target.z;

		GameObject projectile = Instantiate(effectObject, origin, Quaternion.identity) as GameObject;
		projectile.transform.LookAt(target);
		currentRigidBody = projectile.GetComponent<Rigidbody> ();
		projectileScript = projectile.GetComponent<ProjectileScript> ();

		if(currentRigidBody != null) {
			currentRigidBody.AddForce(projectile.transform.forward * speed);
		}
		if (projectileScript != null) {
			projectile.GetComponent<ProjectileScript> ().impactNormal = target.normalized * -1;
		}
	}

	private void InvokeAuraEffectByCardElementAndSelection(GameObject effectObject, Vector3 source) {
		Vector3 origin = source;
		origin.z = source.z;

		GameObject aura = Instantiate(effectObject, origin, Quaternion.identity) as GameObject;
		SoundManager.instance.PlayCardMoveSound ();

		GameObject.Destroy (aura, 3);
	}

	private void InvokeWallEffectByCardElementAndSelection(GameObject effectObject, Vector3 source, Player sourcePlayer) {
		int m = source.x > 0 ? -1 : 1;

		Quaternion q = new Quaternion ();
		q.eulerAngles = new Vector3 (0, 0, m * 90);

		GameObject wall = Instantiate(effectObject, new Vector3(m * 0.85f, 0, 87), q)  as GameObject;
		sourcePlayer.attackProtection = wall;

		GameObject.Destroy (wall, 3);
	}
}