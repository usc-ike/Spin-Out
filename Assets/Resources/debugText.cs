using UnityEngine;
using System.Collections;

public class debugText : MonoBehaviour {
	int i = 50;
	// Use this for initialization
	void OnEnable() {
		StartCoroutine(move());
	}
	void OnDisable() {
		StopAllCoroutines();
	}

	IEnumerator move() {
		while (i>0) {
			transform.Translate(Vector3.up * 0.2f * Time.deltaTime, Space.Self);
			i--;
			yield return 0;
		}
		gameObject.Recycle();
	}
}
