using UnityEngine;
using System.Collections;

public class iceCreamMvmt : MonoBehaviour {

	private Transform iTrans;
	private Rigidbody iRigid;
	private ConstantForce iConst;
	[HideInInspector]
	public bool spinning = false;

	// Use this for initialization
	void Awake() {
		iTrans = transform;
		iRigid = GetComponent<Rigidbody>();
		iConst = GetComponent<ConstantForce>();
	}

	void OnEnable() {
		iRigid.velocity = Vector3.zero;
		iRigid.maxAngularVelocity = 50;
		iConst.force = Vector3.zero;
		iRigid.AddExplosionForce(0.16f, iTrans.position + Vector3.forward * 0.1f, 0.25f, 0.1f, ForceMode.Impulse);
		//iRigid.AddForceAtPosition(Vector3.forward * 0.9f, iTrans.position + Random.insideUnitSphere * 0.2f);
		StartCoroutine(Move());
	}

	IEnumerator Move() {
		yield return new WaitForSeconds(0.6f);
		iConst.force = Vector3.forward * 0.6f;
	}
	
	void OnCollisionEnter(Collision item) {
		if (item.collider.CompareTag("Vehicle") || item.collider.CompareTag("VehiclePart")) {
			gameObject.Recycle();
		}
	}

	void OnTriggerExit(Collider item) {
		if (item.CompareTag("Boundary")) {
			gameObject.Recycle();
		}
	}
}
