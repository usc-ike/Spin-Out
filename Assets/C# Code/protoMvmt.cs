using UnityEngine;
using System.Collections;

public class protoMvmt : MonoBehaviour {

	//Adjustable Stats
	[Tooltip("DO NOT EDIT UNLESS YOU KNOW EXACTLY WHAT THESE ARE")]
	public string label;
	public int HP;
	public float speedMax = 2.5f;
	public float speedMin = 0; //0.2f;
	public float spinSpd = 2.5f;
	public int rps = 3;
	public float exRadius = 0.25f;
	public int basePoints = 20;
	public int penaltyPoints = 20;
	
	//car properties
	private int lag = 100;
	private Rigidbody car, wRigid;
	private plateData facing;
	private Renderer[] mat;
	//public GameObject StartLine;
	[HideInInspector]
	public GameObject wind;
	private Transform vTrans, wTrans;
	private Vector3 previous;
	private Transform[] tPos;
	[HideInInspector]
	public int id, wId;
	
	//Car Stats
	private float speed;
	
	private int hp;
	private bool ready = false;
	private bool spinning = false;
	private int combo = 1;
	private int chainCombo = 1;
	private ConstantForce vForce;
	[HideInInspector]
	public bool run = false;
	//private bool position;

	//test
	private string collided;

	//color
	//public Material Body;

	//accessing
	private main main;
	private touchScreen ts;

	//Object Pooling
	[Tooltip("DO NOT EDIT UNLESS YOU KNOW EXACTLY WHAT THESE ARE")]
	public vehicleParts tires;
	private IEnumerator bomb;

	//Collisions
	private GameObject cItem;
	private bool goldDeducted = false;


	//Audio
	sfxManager sfx;
	//private float yPosition;
	private camShake camshake;
	private Animator[] flash;

	void Awake() {
		speed = speedMax;
		sfx = GameObject.FindWithTag("ControlTower").GetComponent<sfxManager>();
		camshake = Camera.main.GetComponent<camShake>();
		main = GameObject.FindWithTag("ControlTower").GetComponent<main>();
		ts = GameObject.FindWithTag("MainCamera").GetComponent<touchScreen>();
		vTrans = gameObject.transform;
		wind = vTrans.GetChild(0).gameObject;
		wTrans = wind.transform;
		wRigid = wind.GetComponent<Rigidbody>();
		vForce = gameObject.GetComponent<ConstantForce>();
		mat = gameObject.GetComponentsInChildren<Renderer>(true);
		tPos = gameObject.GetComponentsInChildren<Transform>(true);
		flash = gameObject.GetComponentsInChildren<Animator>(true);
		car = gameObject.GetComponent<Rigidbody>();
		if (label.Equals("GoldCar")) {
			wId = -1;
		}
		wind.SetActive(false);
	}
	void Reset() {
		collided = null;
		tPos[2].localPosition = Vector3.zero;
		run = false;
		if (label.Equals("GoldCar")) {
			lag = 10;
		} else {
			lag = 50;
		}
		goldDeducted = false;
		hp = HP;
		speed = speedMax;
		car.rotation = Quaternion.Euler(0,180,0);
		car.velocity = Vector3.zero;
		wRigid.velocity = Vector3.zero;
		car.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX;
		car.maxAngularVelocity = 50;
		wind.SetActive(false);
		ready = false;
		spinning = false;
		chainCombo = 1;
		combo = 1;
		vForce.relativeTorque = Vector3.zero;
		vForce.force = Vector3.zero;
		vForce.torque = Vector3.zero;
		vForce.force = Vector3.zero;
	}
	// Use this for initialization
	void OnEnable() {
		Reset();
		/*int num = Random.Range(0, 1);
		if (num < 0.5f) {
			position = true;
		} else {
			tPos[2].localPosition = Vector3.up * 0.005f;
			position = false;
		}/**/
		id = main.carID;
		wId = main.waveCount;
		/*if (label.Equals("SwatCar")) {
			for (int i = 0; i < mat.Length; i++) {
				print(mat[i].bounds.size);
			}
		}/**/
		if (!label.Equals("GoldCar") && !label.Equals("GoldCarChild")) {
			main.vList[id % main.GetLimit()].vehicleDat = this;
		}
		//main.vehicleGroup[id%main.maxVehicles] = gameObject.GetComponent<protoMvmt>();
		StartCoroutine(Move());
	}
	
	void OnDisable() {
		StopAllCoroutines();
	}

	IEnumerator Move() {
		if (!label.Equals("GoldCarChild")) {
			while (!ready) {
				yield return new WaitForFixedUpdate();
			}
			while (lag > 0) {
				lag--;
				if (spinning) {
					this.Recycle();
				}
				yield return new WaitForFixedUpdate();
			}
			car.velocity = Vector3.zero;
		} else {
			spinning = true;
			vForce.force = Vector3.back;
			car.constraints = RigidbodyConstraints.None;
			car.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
			car.velocity = car.velocity.normalized * spinSpd;
			vForce.relativeTorque = Vector3.down * rps;
			WindTrail();
		}
		vForce.force = Vector3.back;
		while (true) {
			/*if (label.Equals("SwatCar")) {
				Debug.DrawLine(vTrans.position - new Vector3(0, -0.06f, 0.3f), vTrans.position + new Vector3(0, 0.06f, 0.3f), Color.green);
			}/**/
			
			if (!spinning) {
				/*position = !position;
				if (position) {
					tPos[2].localPosition = Vector3.up * 0.005f;
				} else {
					tPos[2].localPosition = Vector3.up * -0.005f;
				}/**/
				if (label.Equals("GoldCar")) {
					car.velocity = vForce.force * speedMax;
				} else {
					car.velocity = vForce.force * speed; //car.velocity.normalized
				}
			} else if (spinning) {
				wTrans.position = previous;
			}
			previous = vTrans.position;
			
			if (car.velocity.magnitude >= spinSpd && spinning) {
				car.velocity = car.velocity.normalized * spinSpd;
			}
			yield return new WaitForFixedUpdate();
		}
	}

	void OnCollisionEnter(Collision item){
		cItem = item.gameObject;
		if(!ready){
			ready = true;
		}
		if(!cItem.CompareTag("Track") && !cItem.CompareTag("Tire") && cItem.name != collided && hp > 0){
			if (gameObject.CompareTag("Barrier") && !spinning) {
				//Do nothing
			} else {
				collided = cItem.name;
				if (!cItem.CompareTag("Vehicle")) {
					sfx.Play("Spark.mp3");
				}
				main.sparks.Spawn(vTrans.position);
				hp--;
				if (wId == main.waveCount) {
					main.initCollision = true;
				}
				//main.initColSec = 0;
				if (cItem.CompareTag("Vehicle")) {
					if (label.Equals("IceCar")) {
						Explode();
						main.TransformAllIceCream();
					}else if (!spinning && !label.Equals("SwatCar")) {
						spinning = true;
						vForce.force = Vector3.forward * 0.5f;
						car.constraints = RigidbodyConstraints.None;
						car.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
						car.velocity = car.velocity.normalized * spinSpd;
						vForce.relativeTorque = Vector3.down * rps;
						WindTrail();
					} else {
						combo++;
					}
					cItem.GetComponent<protoMvmt>().Spark(id);
					chainCombo++;
					cItem.GetComponent<protoMvmt>().Chain(chainCombo);
				}
				if (label.Equals("TireCar")) {
					for (int i = Random.Range(1, 6); i >= 0; i--) {
						if (Random.Range(0, 100) < main.tireCarGoldTireRate){
							main.tire2.Spawn(tPos[9].position, Quaternion.Euler(Random.Range(1, 90), 0, 0));
						} else {
							main.tire1.Spawn(tPos[9].position, Quaternion.Euler(Random.Range(1, 90), 0, 0));
						}
					}
				}
				if (hp <= 0) {
					if(label.Equals("GoldCar")){
						BreakDown(3, exRadius);
					}
					Explode();
					main.DisplayScore(label, combo, chainCombo, basePoints, vTrans.position);
				} else {
					if (gameObject.activeInHierarchy) {//uncomment the first line, and comment out the second line to test out the animation
						if (label.Equals("Standard")) {
							flash[2].Play("Flash");
						} else {
							flash[1].Play("Flash");
						}
					}
				}
			}
		}
	}

	void OnTriggerEnter(Collider item){
		if (item.CompareTag("StartLine") && !label.Equals("GoldCar") && !label.Equals("GoldCarChild")) {
			if (main.waveCount == wId) {
				main.StartDecel();
			}
		} else if (item.CompareTag("Boundary") && label.Equals("GoldCar")) {
			ts.AllowPlacement();
			ts.ResetAttempt();
		}
		if(item.CompareTag("Plate") && !label.Equals("SwatCar")){
			//main.delaySeconds = 0;
			spinning = true;
			if (wId == main.waveCount) {
				main.initCollision = true;
			}
			//main.initColSec = 0;
			car.constraints = RigidbodyConstraints.None;
			car.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
			facing = item.GetComponent<plateData>();
			car.velocity = facing.direction.normalized * spinSpd;
			vForce.relativeTorque = Vector3.down * rps;
			WindTrail();
			ts.ResetAttempt();
			item.gameObject.SetActive(false);
		}
	}

	void OnTriggerExit(Collider item){
		if (item.CompareTag("Boundary")) {
			if (vTrans.position.z <= -17 && !spinning) {
				if (!label.Equals("SwatCar")) {
					if (!label.Equals("GoldCar") || (label.Equals("GoldCar") && ts.CheckAttempt() && !goldDeducted)) {
						main.DisplayScore(label, -1, -1, penaltyPoints, vTrans.position + Vector3.forward);
						goldDeducted = true;
					}
				}
			}
			if (label.Equals("Standard")) {
				flash[2].Play("Idle");
			} else {
				flash[1].Play("Idle");
			}
			if (!label.Equals("GoldCar")) {
				gameObject.Recycle();
			}
		} else if(item.name.Equals("GoldLine")){
			gameObject.Recycle();
		}
	}

	IEnumerator CountDown(float time){
		yield return new WaitForSeconds(time);
		Explode();
	}

	void Explode(){
		//main.delaySeconds = 0;
		Vector3 epicenter = vTrans.position;
		gameObject.Recycle();
		main.DestructionData();
		sfx.Play("Explosion.mp3");
		main.explosion.Spawn(epicenter);
		camshake.enabled = true;
		for (int i = 3; i < tPos.Length; i++) {
			tires.Spawn(tPos[i].position, Quaternion.Euler(Random.Range(1, 90), 0, 0));
		}
		foreach (Collider col in Physics.OverlapSphere(epicenter, exRadius)) {
			if (col.attachedRigidbody != null && !col.CompareTag("Plate")) {
				if (col.CompareTag("Tire")) {
					col.attachedRigidbody.AddExplosionForce(0.16f, epicenter, exRadius, 0.1f, ForceMode.Impulse);
				} else {
					col.attachedRigidbody.AddExplosionForce(1f, epicenter, exRadius, 0, ForceMode.Impulse);
				}
			}
		}
	}
	//from here
	/*IEnumerator Flash(float time){ //this is a coroutine.
		ChangeColor();
		yield return new WaitForSeconds(time);
		ChangeBack();
	}
	void ChangeColor(){
		print("used1");
		mat[0].material.color = Color.red;
	}
	void ChangeBack(){
		print("used2");
		mat[0].material.color = Color.white;
		collided = null;
	}*/
	//all the way to here. 
	/*void OnDrawGizmos() {
		if (label.Equals("SwatCar")) {
			Gizmos.color = new Color(0, 0, 1, 1);
			Gizmos.DrawWireSphere(vTrans.position - new Vector3(0, 0, 0.32f), 0.12f);
			Gizmos.DrawWireSphere(vTrans.position + new Vector3(0, 0, 0.32f), 0.12f);
		} else {
			Gizmos.color = new Color(0, 0, 1, 1);
			Gizmos.DrawWireSphere(vTrans.position, 0.17f);
		}
	}/**/
	public IEnumerator Decelerate() {
		while (speed > speedMin && !spinning) {
			if (!main.CheckPaused()) {
				speed -= 0.06f;
			}
			yield return new WaitForFixedUpdate();
		}
		if (this != null && !spinning) {//Check if vehicles alive(?)
			speed = speedMin;
			vForce.force = Vector3.zero;
			car.velocity = vForce.force * speedMin;
			main.tPause = false;
		}
		while (this != null) {
			if (spinning) {
				break;
			}
			if (run) {
				car.velocity = Vector3.zero;
				vForce.force = Vector3.back;
				run = false;
				break;
			}
			yield return 1;
		}
		if (!spinning) {
			while (speed < speedMax) {
				if (!main.CheckPaused()) {
					speed += 0.06f;
				}
				yield return new WaitForFixedUpdate();
			}
		}/**/
	}

	public bool Check() {
		return spinning;
	}
	public bool CheckSpd() {
		if (spinning && car.velocity.magnitude <= 1) {
			return true;
		}
		return false;
	}
	void WindTrail() {
		wind.SetActive(true);
	}
	public void Chain(int c) {
		if (c > chainCombo) {
			chainCombo = c;
		}
	}
	public void Spark(int cid) {
		if (id > cid) {
			sfx.Play("Spark.mp3");
		}
	}
	void BreakDown(int num, float maxDist) {
		for (int i = 0; i < num; i++) {
			main.goldCarChild.Spawn(vTrans.position + new Vector3(Random.Range(-maxDist, maxDist), 0, Random.Range(-maxDist, maxDist)), Quaternion.Euler(0, 180, 0));
		}
	}
	public void InsertData(int i) {
		main.activeWave[i] = this;
	}
	public void TransformToIceCream() {
		if (gameObject.activeInHierarchy) {
			gameObject.Recycle();
			main.iceCream.Spawn(vTrans.position, Quaternion.Euler(0, Random.Range(1, 90), 0));
		}
	}
}
