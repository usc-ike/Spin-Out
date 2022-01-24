using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public struct Plate {
	public GameObject plate;
	public plateData pScript;
	public GameObject boundary;
	public MeshFilter bLine;
};

public class touchScreen : MonoBehaviour {

	private int plate_limit = 1;

	public LayerMask touchInputMask;

	private int index = 0;
	private RaycastHit hit;
	private float radius;
	//private Renderer bounds;
	private Vector3 center;
	private bool attempt;
	private bool placing = false;
	private int placed = 0;

	//Object Pool
	public Plate[] plate;
	//private GameObject boundaryCircle;
	//private plateData data;

	private List<GameObject> touchList = new List<GameObject>();
	private GameObject[] touchesOld;

	void Awake() {
		plate = new Plate[plate_limit * 2];
		GameObject platePrefab = Resources.Load("PlateF") as GameObject;
		for (int i = 0; i < plate.Length; i++) {
			plate[i].plate = Instantiate(platePrefab);
			plate[i].pScript = plate[i].plate.GetComponent<plateData>();
			plate[i].plate.SetActive(false);
		}/**/
		Renderer rendTemp = platePrefab.transform.GetChild(1).GetComponent<Renderer>();
		radius = rendTemp.bounds.extents.magnitude;

	}

	// Update is called once per frame
	void Update() {

#if UNITY_EDITOR
		if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0)) {

			//foreach
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			Debug.DrawRay(ray.origin, ray.direction, Color.red);

			if (Physics.Raycast(ray, out hit, Mathf.Infinity, touchInputMask)) {

				GameObject recipient = hit.transform.gameObject;
				touchList.Add(recipient);
				if (Input.GetMouseButtonDown(0)) {
					if (!hit.collider.CompareTag("UI") && placed < plate_limit) {
						Down();
					}
				} else if (Input.GetMouseButton(0)) {
					if (!hit.collider.CompareTag("UI") && placing) {
						Drag();
					}
				} else if (Input.GetMouseButtonUp(0)) {
					Up();
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

			if (Physics.Raycast(ray, out hit,Mathf.Infinity, touchInputMask)) {

				GameObject recipient = hit.transform.gameObject;
				touchList.Add(recipient);
				if (!hit.collider.CompareTag("UI")) {
					if (touch.phase == TouchPhase.Began) {
						if (!hit.collider.CompareTag("UI") && placed < plate_limit) {
							print(hit.collider.name);
							Down();
						}
					} else if (touch.phase == TouchPhase.Moved) {
						if (!hit.collider.CompareTag("UI") && placing) {
							Drag();
						}
					} else if (touch.phase == TouchPhase.Ended) {
						Up();
					}
				}
			}
		}

		//Applied to both

	}

	void Down() {
		center = hit.point;
		attempt = true;
		if (!plate[index].plate.activeInHierarchy) {
			plate[index].plate.SetActive(true);
			placing = true;
			Move(center);
		} else {
			//Move(center);
		}
	}

	void Drag() {
		Rotate();
		if (Vector3.Distance(center, hit.point) >= radius) {
			plate[index].pScript.Reached();
		} else {
			plate[index].pScript.Back();
		}
	}

	void Up() {
		//Rotate();
		if (Vector3.Distance(center, hit.point) < radius) {
			plate[index].plate.SetActive(false);
		} else {
			plate[index].pScript.Activated();
			if (index < plate_limit - 1) {
				index++;
			} else {
				index = 0;
			}
			if (placing) {
				placed++;
			}
			placing = false;
		}
	}

	void Move(Vector3 location) {
		plate[index].plate.transform.position = location;
	}

	void Rotate() {
		if (plate[index].plate.activeInHierarchy) {
			plate[index].pScript.Turn(hit.point);
		}
	}
	public void ResetPlates() {
		for (int i = 0; i < plate.Length; i++) {//removes plates from screen
			plate[i].plate.SetActive(false);
			placed = 0;
		}
	}
	public int PLimit() {
		return plate_limit;
	}
	public void ResetAttempt() {
		attempt = false;
	}
	public bool CheckAttempt() {
		return attempt;
	}
	public void AllowPlacement(){
		placed = 0;
	}
	public void PowerUp(bool effective) {
		plate_limit = (effective ? 2 : 1);
	}
	public bool DashPanelPlaced() {
		if (placed == plate_limit) {
			return true;
		} else {
			return false;
		}
	}
}
