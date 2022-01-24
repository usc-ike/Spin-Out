using UnityEngine;
using System.Collections;

public class textBehaviour : MonoBehaviour {
	public TextMesh pText;
	public TextMesh[] cText;

	void Awake() {
		pText = gameObject.GetComponent<TextMesh>();
		cText = gameObject.GetComponentsInChildren<TextMesh>();
	}
	// Update is called once per frame
	void OnEnable() {
		cText[1].text = pText.text;
		StartCoroutine(move());
	}
	void OnDisable() {
		StopAllCoroutines();
	}

	IEnumerator move() {
		while (true) {
			transform.Translate(Vector3.back * Time.deltaTime, Space.Self);
			if (gameObject.transform.position.y >= 1.5) {
				break;
			}
			yield return 0;

		}
		gameObject.Recycle();
	}
}
