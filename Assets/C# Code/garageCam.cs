using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class garageCam : MonoBehaviour {
	
	public LayerMask layerMask;
	
	private RaycastHit hit;
	public transitions controlTower;

	#if UNITY_EDITOR
	public Vector3 current, previous, deltaMouse;

	#endif

	void Awake() {
		controlTower = GameObject.FindWithTag("ControlTower").GetComponent<transitions>();
	}


	void Update() {
		#if UNITY_EDITOR // stuff that works in unity editor screen. since obviously, theres no touch screens on the laptop (k. i know. some do these days....)
		if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0)) {

			//foreach
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			Debug.DrawRay(ray.origin, ray.direction, Color.red);

			if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)) {
				//Debug.Log(hit.collider.name);
				GameObject recipient = hit.transform.gameObject;
				//touchList.Add(recipient);
				if (Input.GetMouseButtonDown(0)) {
					previous = Input.mousePosition;
					current = Input.mousePosition;
					if (hit.collider.CompareTag("Vehicle")) {
						controlTower.Initialize();
					} else if (hit.collider.CompareTag("Backplate")) {
						controlTower.ChangeStart(Input.mousePosition.x);
					}
				} else if (Input.GetMouseButton(0)) {
					current = Input.mousePosition;
					deltaMouse = current - previous;
					previous = current;
					controlTower.Drag(deltaMouse);
					controlTower.Change(Input.mousePosition.x);
				} else if (Input.GetMouseButtonUp(0)) {
					controlTower.Lift();
					if (controlTower.change) {
						controlTower.ChangeEnd(Input.mousePosition.x);
					}
					deltaMouse = Vector3.zero;
				}
			}
		}
		#endif

		if (Input.touchCount > 0) {
			/*touchesOld = new GameObject[touchList.Count];
			touchList.CopyTo(touchesOld);
			touchList.Clear();*/

			/////Method1
			Touch touch = Input.GetTouch(0);
			
			//foreach(Touch touch in Input.touches){
			Ray ray = Camera.main.ScreenPointToRay(touch.position);

			if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)) {

				//GameObject recipient = hit.transform.gameObject;
				//touchList.Add(recipient);
				if (touch.phase == TouchPhase.Began) {
					if (hit.collider.CompareTag("Vehicle")) {
						controlTower.Initialize();
					} else if (hit.collider.CompareTag("Backplate")) {
						controlTower.ChangeStart(touch.position.x);
					}
				} else if (touch.phase == TouchPhase.Moved) {
					controlTower.Drag(touch.deltaPosition);
					controlTower.Change(touch.position.x);
				} else if (touch.phase == TouchPhase.Ended) {
					controlTower.Lift();
					if (controlTower.change) {
						controlTower.ChangeEnd(Input.mousePosition.x);
					}
				}
			}
		}

		//Applied to both

	}

}
