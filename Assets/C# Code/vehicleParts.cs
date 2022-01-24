using UnityEngine;
using System.Collections;

public class vehicleParts : MonoBehaviour {

	public int tireWorth = 1;
	private Collider tCollider;
	private Rigidbody tire;
	private ConstantForce tForce;
	private Transform tTrans;
	private main main;
	private Vector3 dest;

	// Use this for initialization
	void Awake() {
		//dest = GameObject.FindWithTag("Destination").transform.position;
		dest = GameObject.FindWithTag("Destination").transform.position;
		tCollider = gameObject.GetComponent<Collider>();
		tire = gameObject.GetComponent<Rigidbody>();
		tForce = gameObject.GetComponent<ConstantForce>();
		tTrans = gameObject.GetComponent<Transform>();
		main = GameObject.FindWithTag("ControlTower").GetComponent<main>();
	}
	void Reset() {
		tCollider.isTrigger = false;
		tire.useGravity = true;
		tire.velocity = Vector3.zero;
		tForce.relativeTorque = Vector3.zero;
		tForce.relativeForce = Vector3.zero;
		tForce.torque = Vector3.zero;
		tForce.force = Vector3.zero;
	}
	void OnEnable() {
		//gameObject.GetComponent<Collider>().enabled = false;
		StartCoroutine(Debris());
	}
	void OnDisable() {
		Reset();
		StopAllCoroutines();
	}
	public IEnumerator Debris() {
		/*for (int i = 0; i < 10; i++) {
			yield return 0;
		}
		gameObject.GetComponent<Collider>().enabled = true;*/
		yield return new WaitForSeconds(1.5f);
		tCollider.isTrigger = true;
		tire.useGravity = false;
		//tire.velocity = new Vector3(-0.361f, 1.55f, -14.744f) - tTrans.position;
		tire.velocity = Vector3.zero;
		tForce.force = dest - tTrans.position;//new Vector3(-0.361f, 1.55f, -14.744f) - tTrans.position;
	}
	void OnTriggerExit(Collider item) {
		if (item.CompareTag("Boundary")) {
			main.tireCount+=tireWorth;
			this.Recycle();
		}
	}
}
