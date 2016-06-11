using UnityEngine;
using System.Collections;

public class ParticlesWhisps : MonoBehaviour {
	private static ParticlesWhisps instance;

	void Awake() {
		if(instance == null) {
			instance = this;
			DontDestroyOnLoad (gameObject);
		}
	}
}
