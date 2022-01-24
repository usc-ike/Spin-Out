using UnityEngine;
using System.Collections;

public class testAnimation : MonoBehaviour {

	public void Move(){
		Animator anim = GameObject.Find("Cube").GetComponent<Animator>();
		anim.runtimeAnimatorController = Resources.Load("Cube") as RuntimeAnimatorController;
	}
}
