using UnityEngine;
using System.Collections;

public class pauseAnim : MonoBehaviour {
	private GameObject background, resume, quit;
	private Animator[] anims;

	// Use this for initialization
	void Awake () {
		anims = gameObject.GetComponentsInChildren<Animator>();
		background = anims[0].gameObject;
		resume = anims[1].gameObject;
		quit = anims[2].gameObject;
	}

	void OnEnable() {
		for (int i = 0; i < anims.Length; i++) {
			anims[i].Play("pauseIn");
		}
	}
	public void ClearButton(bool condition) {
		resume.SetActive(condition ? false:true);
		quit.SetActive(condition ? false:true);
	}
}
