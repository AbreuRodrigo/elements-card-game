using UnityEngine;
using System.Collections;

public class Controle : MonoBehaviour {

	public GameObject prefab;

	void Update() {
		if(Input.GetKeyDown(KeyCode.P)) {
			if(prefab != null) {
				GameObject.Instantiate (prefab);
			}
		}
	}
}