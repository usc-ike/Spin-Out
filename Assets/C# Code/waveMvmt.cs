using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class waveMvmt : MonoBehaviour {

	private main main;
	private touchScreen ts;
	private Animator anim;
	public TextMesh[] waveTxt;

	void Awake() {
		main = GameObject.FindWithTag("ControlTower").GetComponent<main>();
		ts = GameObject.FindWithTag("MainCamera").GetComponent<touchScreen>();
		anim = gameObject.GetComponent<Animator>();
		waveTxt = gameObject.GetComponentsInChildren<TextMesh>();
	}
	void Start() {
		gameObject.SetActive(false);
	}
	// Use this for initialization
	void OnEnable() {
		for (int i = 0; i < waveTxt.Length; i++) {
			if (!main.ended) {
				anim.updateMode = AnimatorUpdateMode.Normal;
				waveTxt[i].text = "Wave " + (main.waveCount == 0 ? 1:main.waveCount);
				anim.Play("Slide");
			} else {
				anim.updateMode = AnimatorUpdateMode.UnscaledTime;
				waveTxt[i].text = "TIME!";
				anim.Play("SlideFinal");
			}
		}
		
	}
	void Update() {
		if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle")) {
			ts.ResetPlates();
			if (main.waveCount == 1) {
				//main.StartSpin();
			}
			gameObject.SetActive(false);
		}
	}
	
}
