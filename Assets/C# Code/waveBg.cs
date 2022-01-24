using UnityEngine;
using System.Collections;

public class waveBg : MonoBehaviour {

	private main main;
	private Animator anim;

	void Awake() {
		main = GameObject.FindWithTag("ControlTower").GetComponent<main>();
		anim = gameObject.GetComponent<Animator>();
	}
	void OnEnable() {
		if (!main.ended) {
			anim.updateMode = AnimatorUpdateMode.Normal;
			anim.Play("Slide");
		} else {
			anim.updateMode = AnimatorUpdateMode.UnscaledTime;
			anim.Play("SlideFinal");
		}
		
	}
	void Update() {
		if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle")) {
			gameObject.SetActive(false);
		}
	}
}
