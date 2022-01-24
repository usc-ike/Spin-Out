using UnityEngine;
using System.Collections;

public class camBehaviour : MonoBehaviour {

	private Transform cTrans;
	private Camera cam;
	private touchScreen ts;
	private camShake cs;

	void Awake() {
		cTrans = gameObject.transform;
		cam = gameObject.GetComponent<Camera>();
		ts = gameObject.GetComponent<touchScreen>();
	}

	public void ToGame() {
		cTrans.position = new Vector3(0.006231219f, 3.04f, -20.83135f);
		cTrans.rotation = Quaternion.Euler(90, 0, 0);
		cTrans.localScale = new Vector3(1, 1.120749f, 1.120749f);
		//cam.fieldOfView = 75;
		ts.enabled = true;
	}
	public void ToMenu() {
		cTrans.position = new Vector3(0.006231219f, -2.508774f, -0.6498127f);
		cTrans.rotation = Quaternion.Euler(351.5045f, 0, 0);
		cTrans.localScale = new Vector3(1, 1, 1);
		//cam.fieldOfView = 60;
		ts.enabled = false;
	}
}
