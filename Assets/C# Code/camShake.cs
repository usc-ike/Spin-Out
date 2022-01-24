using UnityEngine;
using System.Collections;

public class camShake : MonoBehaviour {

	private Transform camTrans;
	public int shakeTime = 50;
	private float magnitude = 0.05f; //Adjust this number to increase/decrease magnitude of the shaking
	private float mag;
	public Vector3 origin;
	private camShake camshake;
	

	// Use this for initialization
	void Awake() {
		camTrans = gameObject.transform;
		origin = new Vector3(0.006231219f, 3.04f, -20.83135f);
	}
	void OnEnable() {
		shakeTime = 50;
		mag = magnitude;
	}
	
	// Update is called once per frame
	void Update () {
		if (shakeTime > 0) {
			mag -= 0.001f;
			camTrans.localPosition = origin + Random.insideUnitSphere * mag;
			shakeTime--;
		} else if (shakeTime <= 0) {
			camTrans.position = origin;
			this.enabled = false;
		}
	}
}
