using UnityEngine;
using System.Collections;

public class garageVehicle : MonoBehaviour {
	private Vector3 startupPos;
	public Quaternion startupRot;
	private Transform gvTrans;
	public Material shade, tire, tireAlt, body;
	public MeshRenderer[] vMesh;

	void Awake() {
		gvTrans = transform;
		startupPos = gvTrans.position;
		startupRot = gvTrans.rotation;
		vMesh = gameObject.GetComponentsInChildren<MeshRenderer>();
	}

	void OnEnable() {
		gvTrans.localPosition = startupPos;
		gvTrans.localRotation = startupRot;
	}

	public void Unlock(bool ul) {
		if (ul) {
			vMesh[0].material = body;
			for (int i = 1; i < vMesh.Length; i++) {//7, 9. 11
				if (i > 6 && i % 2 != 0) {
					vMesh[i].material = tireAlt;
				} else {
					vMesh[i].material = tire;
				}
			}
		} else {
			for (int i = 0; i < vMesh.Length; i++) {
				vMesh[i].material = shade;
			}
		}
	}
}
