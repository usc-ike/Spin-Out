using UnityEngine;
using System.Collections;

public class shockWaveAnim : MonoBehaviour {

	private Animator shockAnim;

	// Use this for initialization
	void Awake() {
		shockAnim = gameObject.GetComponent<Animator>();
	}

	void OnEnable() {
		shockAnim.Play("TireMeterIncreaseAnim_1");
	}
	
	// Update is called once per frame
	void Update () {
		if (shockAnim.GetCurrentAnimatorStateInfo(0).IsName("Idle")) {
			gameObject.SetActive(false);
		}
	}
}
