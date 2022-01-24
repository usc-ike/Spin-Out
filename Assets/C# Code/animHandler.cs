using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class animHandler : MonoBehaviour {

	private Animator anim;
	private main main;
	private transitions tr;
	private controlTower ct;
	private Button[] buttons;

	sfxManager sfx;

	void Awake() {
		sfx = GameObject.FindWithTag("ControlTower").GetComponent<sfxManager>();
		ct = GameObject.FindWithTag("ControlTower").GetComponent<controlTower>();
		tr = GameObject.FindWithTag("ControlTower").GetComponent<transitions>();
		main = GameObject.FindWithTag("ControlTower").GetComponent<main>();
		buttons = GameObject.Find("Canvas-MainMenu").GetComponentsInChildren<Button>();
		anim = gameObject.GetComponent<Animator>();
		this.enabled = false;
	}

	void OnEnable() {
		for (int i = 0; i < buttons.Length; i++) {
			buttons[i].interactable = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (tr.creditsScene.activeInHierarchy && anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") && ct.state == ct.Menu) {
			tr.creditsScene.SetActive(false);
			Disable(true);
		} else if (tr.garageScene.activeInHierarchy && anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") && ct.state == ct.Menu) {
			tr.garageScene.SetActive(false);
			Disable(true);
		} else if (tr.startScreen.activeInHierarchy && anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") && ct.state == ct.Menu) {
			tr.startScreen.SetActive(false);
			Disable(true);
		} else if (tr.menuTrack.activeInHierarchy && anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") && ct.state == ct.Play) {
			tr.DeactivateResults();
			tr.enabled = false;
			tr.menuTrack.SetActive(false);
			this.enabled = false;
		} else if(main.gameScene.activeInHierarchy && anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") && ct.state == ct.Menu){
			tr.GameFinish();
			Disable(false);
		} else if(tr.garageScene.activeInHierarchy && anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") && ct.state == ct.Garage) {
			tr.menuTrack.SetActive(false);
			Disable(false);
		}
	}
	void Disable(bool title) {
		for (int i = 0; i < buttons.Length; i++) {
			buttons[i].interactable = true;
			tr.ActivateTitleButton(title);
			tr.DeactivateNxt();
		}
		this.enabled = false;
	}
}
