using UnityEngine;
using System.Collections;

public class TestBehaviour : MonoBehaviour {

	public bool validate;

	void Start () {
		StartCoroutine (Test ());
	}

	public void Validate() {
		validate = true;
	}
	
	IEnumerator Test() {
		while(!validate) {
			yield return null;
		}

		Debug.Log ("Finished");

		yield return new WaitForSeconds (1);
	}
}
