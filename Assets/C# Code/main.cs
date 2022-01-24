using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text;


public class main : MonoBehaviour {
	private float TBound, BBound, RBound, LBound, YPoint;
	[Tooltip("DO NOT EDIT UNLESS YOU KNOW EXACTLY WHAT THESE ARE")]
	public textBehaviour text;
	public float iceTruckRate = 1;
	public float goldVehicleRate = 25;
	public float tireTruckRate = 4;
	public float swatCarRate = 5;
	private int swatLimit = 1;
	
	[HideInInspector]
	public int scoreTotal = 0, tireCount = 0;
	private int countComparison = 0;
	[Tooltip("Amount of Tires needed to be able to use the powerup")]
	public int maxMeter = 50;
	[Tooltip("Rate at which the time increases upon vehicle explosion. The result is this number multiplied by the combo")]
	public float timeIncrease = 0.4f;
	[Tooltip("Time subtracted for every vehicle missed.")]
	public float timePenalty = 1.0f;
	[Tooltip("% at which a gold tire pops out of the tire truck")]
	public float tireCarGoldTireRate = 10;


	public int maxVehicles = 12;
	public int minVehicles = 7;
	private int waveVehicles;
	//private Vector3[] positionList;
	[HideInInspector]
	public vehicleList[] vList;
	[HideInInspector]
	public protoMvmt[] activeWave;
	[HideInInspector]
	public bool ended = false;
	private bool stopped = false;
	private int endingDelay = 200;
	private int endLag;
	

	//Score UI
	private Text[] scoreBoard, timerInt, timerDec;
	[Tooltip("DO NOT EDIT UNLESS YOU KNOW EXACTLY WHAT THESE ARE")]
	public GameObject tireMeter, timeMeter, gameScene;
	private Material trMeter;
	private Material tmMeter;
	private GameObject waveText;
	private GameObject waveBg;
	private GameObject uInter, upperInter;
	private Animator upperAnim;
	[HideInInspector]
	public int waveCount;
	[HideInInspector]
	public bool initCollision;
	private GameObject pauseUI;
	private GameObject pauseBtn;
	private GameObject confirmUI;
	private Text[] confirmReturn;
	[Tooltip("DO NOT EDIT UNLESS YOU KNOW EXACTLY WHAT THESE ARE")]
	public Text resume, quit, yesText, noText;


	//Object Pool
	[Tooltip("DO NOT EDIT UNLESS YOU KNOW EXACTLY WHAT THESE ARE")]
	public protoMvmt vehicle1, vehicle2, vehicle3, vehicle4, goldCar, goldCarChild;
	[Tooltip("DO NOT EDIT UNLESS YOU KNOW EXACTLY WHAT THESE ARE")]
	public vehicleParts tire1, tire2;
	[Tooltip("DO NOT EDIT UNLESS YOU KNOW EXACTLY WHAT THESE ARE")]
	public explosion explosion;
	[Tooltip("DO NOT EDIT UNLESS YOU KNOW EXACTLY WHAT THESE ARE")]
	public sparks sparks;
	[Tooltip("DO NOT EDIT UNLESS YOU KNOW EXACTLY WHAT THESE ARE")]
	public iceCreamMvmt iceCream;
	[Tooltip("DO NOT EDIT UNLESS YOU KNOW EXACTLY WHAT THESE ARE")]
	public GameObject shockWave;
	

	//Speed/Plate Control
	[HideInInspector]
	public bool vehicleReached = false;
	[HideInInspector]
	public int carID = 0;
	[HideInInspector]
	public bool disable = false;

	//Timers
	private float seconds;
	private int slowDown = 0;
	private float maxDelay = 0.8f;//0.75f;
	public float delaySeconds, delayRatio;
	[HideInInspector]
	public bool delay, tPause, moveAgain;
	[HideInInspector]
	public float initColSec;
	private bool pause = false;
	[HideInInspector]
	public bool endGame = false;

	//Tire Meter
	private bool inUse = false;
	private float useTime;

	//Roulette
	private int total = 0;
	[Tooltip("The items in the roulette. To add a object, increase the size and drag & drop the desired object into the elements. Objects MUST be present in the hierarchy and there cannot be any empty elements.")]
	public powerUps[] roulette;
	private int current;
	[Tooltip("The time the roulette is spinning at full speed (seconds)")]
	public float rouletteSpinTime = 1.5f;
	[Tooltip("The slowest spin interval at which the roulette decellerates to (seconds)")]
	public float rouletteDecelCap = 0.3f;
	[Tooltip("The interval between the item switches in the roulette (seconds)")]
	public float rouletteIntervalRate = 0.05f;
	private float rDelay;
	private Button pUpBtn;
	[Tooltip("DO NOT EDIT UNLESS YOU KNOW EXACTLY WHAT THESE ARE")]
	public Texture meterOg, meterGr;

	//Access
	private touchScreen touchScript;
	private controlTower ct;
	private transitions tr;
	private pauseAnim pa;
	
	//Others
	private int mode;
	private StringBuilder strb;
	
	//Result Variables
	private int totalTires, powerUps, vd, maxChain, missed;

	//PowerUp Related
	public int scoreMultiplier = 2;
	private bool startSpin = false;
	private int scoreMult = 1;
	private float timeMult = 1;

	//Audio
	sfxManager sfx;

	//Debuggings
	private int rounds = 0;
	// Use this for initialization
	void Awake () {
		ct = gameObject.GetComponent<controlTower>();
		tr = gameObject.GetComponent<transitions>();
		sfx = gameObject.GetComponent<sfxManager>();
		confirmReturn = GameObject.Find("ConfirmReturn").GetComponentsInChildren<Text>();
		touchScript = GameObject.FindWithTag("MainCamera").GetComponent<touchScreen>();
		uInter = GameObject.Find("PlayCanvas");
		upperInter = GameObject.Find("UpperUI");
		upperAnim = upperInter.GetComponent<Animator>();
		waveText = GameObject.Find("WaveText");
		waveBg = GameObject.Find("WaveTextBG");
		vList = new vehicleList[7];
		sparks.CreatePool(10);
		vehicle1.CreatePool(8);
		vehicle2.CreatePool(3);
		vehicle3.CreatePool(5);
		vehicle4.CreatePool(4);
		iceCream.CreatePool(15);
		goldCar.CreatePool(2);
		goldCarChild.CreatePool(4);
		tire1.CreatePool(28);
		tire2.CreatePool(20);
		explosion.CreatePool(10);
		TBound = GameObject.Find("StartFR").transform.position.z;
		BBound = GameObject.Find("StartBR").transform.position.z;
		RBound = GameObject.Find("StartBR").transform.position.x;
		LBound = GameObject.Find("StartFL").transform.position.x;
		YPoint = GameObject.Find("SpawnArea").transform.position.y;
		trMeter = tireMeter.GetComponent<Renderer>().materials[1];
		tmMeter = timeMeter.GetComponent<Renderer>().materials[1];
		Transform uiTime = GameObject.Find("Timer").transform;
		timerInt = uiTime.GetChild(0).GetComponentsInChildren<Text>();
		timerDec = uiTime.GetChild(1).GetComponentsInChildren<Text>();
		scoreBoard = GameObject.Find("ScoreBoard").GetComponentsInChildren<Text>();
		pauseBtn = GameObject.Find("PauseBtn");
		pauseUI = GameObject.Find("PauseUI");
		pa = pauseUI.GetComponent<pauseAnim>();
		confirmUI = GameObject.Find("ConfirmUI");
		pUpBtn = GameObject.Find("TireMeterButton").GetComponent<Button>();
		for (int i = 0; i < roulette.Length; i++) {
			total += roulette[i].rate;
			roulette[i].icon.SetActive(false);
		}
		confirmUI.SetActive(false);
		pauseUI.SetActive(false);
		uInter.SetActive(false);
		gameScene.SetActive(false);
		this.enabled = false;
	}
	public void StartWaves() {
		activeWave = new protoMvmt[maxVehicles];
		swatLimit = 0;
		mode = ct.gameMode;
		touchScript.ResetPlates();
		switch (mode) {
			case 0: //Arcade
				seconds = 20.00f;
				//print("Arcade");
				break;
			case 1: //Classic
				seconds = 40.00f;
				//print("Classic");
				break;
			default:
				break;
		}
		slowDown = 0;
		maxChain = 0;
		powerUps = 0;
		vd = 0;
		useTime = 0;
		initCollision = false;
		tPause = true;
		pause = false;
		waveCount = 0;
		inUse = false;
		pUpBtn.interactable = false;
		totalTires = 0;
		missed = 0;
		delay = false;
		ended = false;
		endLag = endingDelay;
		stopped = false;
		scoreTotal = 0;
		trMeter.mainTexture = meterOg;
		for (int i = 0; i < scoreBoard.Length; i++) {
			scoreBoard[i].text = "" + scoreTotal;
		}
		delaySeconds = 0;
		delayRatio = delaySeconds / maxDelay * 0.6f + 0.4f;
		tmMeter.SetFloat("_Cutoff", delayRatio);
		tireCount = 0;
		trMeter.SetFloat("_Cutoff", 1 - (float)tireCount / maxMeter * 0.6f);
		countComparison = 0;
		scoreMult = 1;
		startSpin = true;
		pauseBtn.SetActive(true);
		confirmUI.SetActive(false);
		pauseUI.SetActive(false);
		uInter.SetActive(true);
		upperInter.SetActive(true);
		upperAnim.Play("UpperUI_In");
		StartCoroutine(TimerCount());
	}

	void AddWave() {
		waveCount++;
		carID = 0;
		waveVehicles = Random.Range(minVehicles, maxVehicles);
		//print("Vehicles" + waveCount + ": " + waveVehicles);
		vList = new vehicleList[waveVehicles];
		
		waveText.SetActive(true);
		waveBg.SetActive(true);
		vehicleReached = false;
		initCollision = false;
		initColSec = 0;
		moveAgain = false;
		disable = false;
		GenerateVehicles();
		
	}
	
	// Update is called once per frame
	void Update () {
		if(waveCount == 0){
			AddWave();
		}
		if (!tPause) {//timeMeter delay
			if ((delaySeconds >= maxDelay && !ended) || !CheckActive() || touchScript.DashPanelPlaced()) {
				delaySeconds = 0;
				for (int i = 0; i < waveVehicles; i++) {
					vList[i].vehicleDat.run = true;
					//vehicleGroup[i].run = true;
				}
				tPause = true;
				AddWave();
			} else if (delaySeconds < maxDelay) {
				delaySeconds += Time.deltaTime;
			}
			delayRatio = delaySeconds / maxDelay * 0.6f + 0.4f;
			tmMeter.SetFloat("_Cutoff", delayRatio);
		} else if (tPause && !CheckActive()) {
			AddWave();
		}

		if (seconds <= 0) {
			if (!ended) {
				StopCoroutine(TimerCount());
				for (int i = 0; i < 2; i++) {
					timerInt[i].text = "0:";
					timerDec[i].text = "00";
				}
				waveText.SetActive(false);
				waveBg.SetActive(false);
				ended = true;
				waveText.SetActive(true);
				waveBg.SetActive(true);
			}
			SlowDown();
		}
		if (inUse) {//Tire Meter
			trMeter.SetFloat("_Cutoff", 1 - (float)useTime / roulette[current].duration * 0.6f);
			useTime -= Time.deltaTime;
			if (useTime <= 0) {
				startSpin = true;
				trMeter.mainTexture = meterOg;
				touchScript.PowerUp(false);
				ScoreMultiply(false);
				TimeMultiply(false);
				SlowTime(false);
				inUse = false;
			}
		} else {
			if (tireCount != countComparison) {//Tire Meter increase
				if (tireCount < maxMeter) {
					sfx.Play("TireMeterFilling.mp3");
					shockWave.SetActive(false);
					shockWave.SetActive(true);
				}
				trMeter.SetFloat("_Cutoff", 1 - (float)tireCount / maxMeter * 0.6f);
				totalTires += tireCount - countComparison;
				countComparison = tireCount;
				if (tireCount > maxMeter && startSpin) {
					startSpin = false;
					sfx.Play("RouletteSFX.mp3");
					StartCoroutine(Spin());
				}
			}
		}
		if (!CheckActive()) {
			tPause = false;
		}
		/*if (initCollision) {//invisible countdown timer
			initColSec += Time.deltaTime;
			if (initColSec >= 1.5f) {
				moveAgain = true;
				initCollision = false;
			}
		}*/
		if (stopped) {
			endLag--;
			if (endLag == 0) {
				ShowResults();
			}
		}
	}//End of Update

	void ScoreMultiply(bool active) {
		scoreMult = (active ? scoreMultiplier : 1);
	}

	public void StartDecel() {
		
		if (!disable) {
			for (int i = 0; i < waveVehicles; i++) {
				vList[i].vehicleDat.InsertData(i);
				if (!vList[i].vehicleDat.Check()) {
					StartCoroutine(vList[i].vehicleDat.Decelerate());
					//StartCoroutine(vehicleGroup[i].Decelerate());
				}
			}
			disable = true;
		}
	}
	void VehicleUnlock(int index) {
		if (!tr.vehicleList[index].unlocked) {
			tr.vehicleList[index].unlocked = true;
			sfx.Play("GalleryUnlock.mp3");
		}
	}
	public int GetLimit(){
		return waveVehicles;
	}
	void GenerateVehicles() {
		if (waveCount < 13) {
			if (waveCount == 4) {
				swatLimit = 1;
			} else if (waveCount == 8) {
				swatLimit = 2;
			} else if (waveCount == 12) {
				swatLimit = 3;
			}
		}
		if (Random.Range(0, 100) < goldVehicleRate && waveCount > 2) {
			goldCar.Spawn(new Vector3(UnityEngine.Random.Range(RBound, LBound), YPoint, TBound - 0.5f), Quaternion.Euler(0, 180, 0));
			VehicleUnlock(2);
		}
		for (int i = 0; i < waveVehicles; i++) {
			if (waveCount < 3) {
				vList[i].vehicle = vehicle1;
				VehicleUnlock(1);
			} else {
				if (i < swatLimit && Random.Range(0, 100) < swatCarRate) {
					vList[i].vehicle = vehicle4;
					VehicleUnlock(4);
				} else if (Random.Range(0, 100) < tireTruckRate) {
					vList[i].vehicle = vehicle3;
					VehicleUnlock(3);
				} else if (Random.Range(0, 100) < iceTruckRate) {
					vList[i].vehicle = vehicle2;
					VehicleUnlock(5);
				} else {
					vList[i].vehicle = vehicle1;
				}
			}
		}
		for (int i = 0; i < waveVehicles; i++) {//repositions until all vehicles are a safe distance away from each other
			vList[i].position = new Vector3(UnityEngine.Random.Range(RBound, LBound), YPoint, UnityEngine.Random.Range(TBound, BBound));
			while (CheckCollisions(i)) {
				vList[i].position = new Vector3(UnityEngine.Random.Range(RBound, LBound), YPoint, UnityEngine.Random.Range(TBound, BBound));
			}
			vList[i].vehicle.Spawn(vList[i].position, Quaternion.Euler(0, 180, 0));
			carID++;
		}
		//swatNum = 0;
	}
	bool CheckCollisions(int a) {
		if (vList[a].vehicle == vehicle4) {
			return Physics.CheckCapsule(vList[a].position - new Vector3(0, 0, 0.32f), vList[a].position + new Vector3(0, 0, 0.32f), 0.12f, ~(1 << 11));
		} else {
			return Physics.CheckSphere(vList[a].position, 0.17f, ~(1 << 11));
		}
	}

	/*float VehicleException(int a, int b) {
		if (vList[a].vehicle == vehicle4 || vList[b].vehicle == vehicle4) {
			if(vList[a].vehicle == vehicle4 && vList[b].vehicle == vehicle4){
				Vector3 temp1, temp2;
				temp1 = vList[a].position - new Vector3(0, 0, 0.3f);
				temp2 = vList[b].position - new Vector3(0, 0, 0.3f);
				if (Vector3.Distance(temp1, vList[b].position + new Vector3(0, 0, 0.3f)) > 0.26f &&
					Vector3.Distance(vList[a].position + new Vector3(0, 0, 0.3f), temp2) > 0.26f &&
					Vector3.Distance(temp1, vList[b].position) > 0.26f &&
					Vector3.Distance(temp2, vList[a].position) > 0.26f &&
					Vector3.Distance(vList[a].position, vList[b].position) > 0.26f) {
					return 1;
				} else {
					return 0;
				}
			}else if(vList[a].vehicle == vehicle4){
				return SwatCheck(a, b);
			}else {
				return SwatCheck(b, a);
			}
		} else {
			return (Physics.CheckSphere(transform.position, 0.15f, ~(1<<11)) ? 0 : 1);
			//return Vector3.Distance(vList[a].position, vList[b].position);
		}
	}
	float SwatCheck(int a, int b) {
		if (Physics.CheckCapsule(vList[a].position - new Vector3(0, 0, 0.3f), vList[a].position + new Vector3(0, 0, 0.3f), 0.003f, ~(1<<11))) {
			print("colliding");
			return 0;
		} else {
			return 1;
		}
	}/**/
	public void DisplayScore(string label, int pts, int chain, int basePt ,Vector3 pos) {
		if (!ended && pts != 0 && chain != 0) {
			//seconds += 0.5f;
			TextMesh tempT = text.GetComponent<TextMesh>();
			if (pts * basePt > 0) {
				tempT.color = Color.white;
			} else{
				tempT.color = Color.red;
			}
			if (pts < 0) {
				tempT.text = (pts * basePt * scoreMult).ToString();
				missed++;
				if (label.Equals("IceCar")) {
					seconds += timePenalty;
				} else {
					seconds -= timePenalty;
				}
				
			} else {
				pts = (chain > pts ? chain : pts);
				tempT.text = Concat("x", pts.ToString());
				TimeIncrease(pts);
			}
			scoreTotal += pts * basePt * scoreMult;
			text.Spawn(pos, Quaternion.Euler(90, 0, 0));
			for (int i = 0; i < scoreBoard.Length; i++) {
				scoreBoard[i].text = scoreTotal.ToString();
			}
		}
		if (chain > maxChain) {
			maxChain = chain;
		}
	}
	void TimeIncrease(int num) {
		if (mode == 0) {
			seconds += num * timeIncrease * timeMult;
		}
	}
	IEnumerator TimerCount() {
		while (seconds > 0) {
			//if (!tPause) {
				seconds -= Time.deltaTime;
				if (seconds <= 10) {
					timerInt[1].color = Color.red;
					timerDec[1].color = Color.red;
				} else {
					timerInt[1].color = Color.white;
					timerDec[1].color = Color.white;
				}
				int secondsDec = (int)((seconds - (int)seconds) *100);
				for (int i = 0; i < 2; i++) {
					timerInt[i].text = Concat(((int)seconds).ToString(), ":");//((int)seconds).ToString() + ":";
					if (secondsDec < 10) {
						timerDec[i].text = Concat("0", secondsDec.ToString());//"0" + secondsDec;
					} else {
						timerDec[i].text = secondsDec.ToString();//"" + seconds;
					}
				}
			//}
			yield return 1;
		}
		pauseBtn.SetActive(false);
		touchScript.enabled = false;
	}
	IEnumerator Spin() {
		int temp;
		float lapse = Time.time;
		rDelay = rouletteIntervalRate;
		current = Random.Range(0, roulette.Length);
		while(rDelay < rouletteDecelCap) {
			roulette[current].icon.SetActive(false);
			temp = Random.Range(0, roulette.Length);
			while (temp == current) {
				temp = Random.Range(0, roulette.Length);
			}
			current = temp;
			roulette[current].icon.SetActive(true);
			if (Time.time - lapse > rouletteSpinTime) {
				rDelay += 0.01f;
			}
			yield return new WaitForSeconds(rDelay);
		}
		roulette[current].icon.SetActive(false);
		temp = Random.Range(0, total);
		int add = 0;
		for (int i = 0; i < roulette.Length; i++) {
			if (temp < roulette[i].rate + add) {
				current = i;
				break;
			}
			add += roulette[i].rate;
		}
		roulette[current].icon.SetActive(true);
		pUpBtn.interactable = true;
	}
	void StartSpin() {
		StartCoroutine(Spin());
	}
	void SlowDown() {
		slowDown--;
		if (slowDown <= 0) {
			if (Time.timeScale <= 0.12f) {
				Time.timeScale = 0;
				stopped = true;
			} else if (Time.timeScale <= 0.25f && Time.timeScale > 0.12f) {
				Time.timeScale = 0.12f;
				slowDown = 50;
			} else {
				Time.timeScale = 0.25f;
				slowDown = 50;
			}
			Time.fixedDeltaTime = 0.02f * Time.timeScale;
		}
	}
	void ShowResults() {
		CloseGame(1);
		ct.ResultDisplay(scoreTotal, waveCount, totalTires, powerUps, vd, maxChain, missed);
		
	}
	void CloseGame(int dir) {
		tr.bgm[2].Stop();
		uInter.SetActive(false);
		pauseUI.SetActive(false);
		confirmUI.SetActive(false);
		tr.menuTrack.SetActive(true);
		tr.enabled = true;
		tr.GameToMenu(dir);
	}

	public bool CheckActive() {//check if any vehicles are active from the wave. true if yes.
		for (int i = 0; i < waveVehicles; i++) {
			if (vList[i].vehicleDat != null && vList[i].vehicleDat.isActiveAndEnabled && !vList[i].vehicleDat.CheckSpd()) {
				return true;
			}
		}
		tPause = true;
		delaySeconds = 0;
		tmMeter.SetFloat("_Cutoff", 0);
		return false;
	}
	public bool CheckPaused() {
		return pause;
	}
	public void Pause() {
		pause = !pause;
		if (pause) {
			tr.PlaySFX(true);
			tr.bgm[2].volume = 0.5f;
			pauseBtn.SetActive(false);
			pa.ClearButton(false);
			pauseUI.SetActive(true);
			touchScript.enabled = false;
			Time.timeScale = 0;
			Time.fixedDeltaTime = 0.02f * Time.timeScale;
		} else {
			tr.PlaySFX(false);
			tr.bgm[2].volume = 1.0f;
			pauseUI.SetActive(false);
			pauseBtn.SetActive(true);
			touchScript.enabled = true;
			Time.timeScale = 1;
			Time.fixedDeltaTime = 0.02f * Time.timeScale;
		}
	}
	public void FinishGame() {
		Time.timeScale = 1;
		Time.fixedDeltaTime = 0.02f * Time.timeScale;
		this.enabled = false;
		StopAllCoroutines();
		vehicle1.RecycleAll();
		vehicle2.RecycleAll();
		vehicle3.RecycleAll();
		vehicle4.RecycleAll();
		goldCar.RecycleAll();
		goldCarChild.RecycleAll();
		tire1.RecycleAll();
		tire2.RecycleAll();
	}
	public void UseMeter() {
		if (tireCount >= maxMeter) {
			tireCount = 0;
			powerUps++;
			pUpBtn.interactable = false;
			useTime = roulette[current].duration;
			trMeter.mainTexture = meterGr;
			switch (current) {
				case 0: //dashx2
					touchScript.PowerUp(true);
					break;
				case 1: //Score Bonus
					ScoreMultiply(true);
					break;
				case 2: //Time bonus
					TimeMultiply(true);
					break;
				case 3: //slowmo
					SlowTime(true);
					break;
				default:
					break;
			}
			inUse = true;
		} 
	}
	string Concat(string alpha, string beta) {
		strb = new StringBuilder(alpha);
		strb.Append(beta);
		return strb.ToString();
	}
	
	public void Quit(int choice) {
		if (choice == 0) {//quit
			tr.PlaySFX(true);
			pa.ClearButton(true);
			confirmUI.SetActive(true);
		} else if (choice == 1) {//confirm yes
			tr.PlaySFX(true);
			tr.ActivateTitleButton(true);
			CloseGame(0);
		} else if (choice == 2){//confirm no
			tr.PlaySFX(false);
			pa.ClearButton(false);
			confirmUI.SetActive(false);
		}
	}
	public void DestructionData() {
		vd++;
	}
	void TimeMultiply(bool active) {
		timeMult = (active ? 1.5f : 1);
	}
	void SlowTime(bool active) {
		if (active) {
			Time.timeScale = 0.5f;
		} else {
			Time.timeScale = 1.0f;
		}
		Time.fixedDeltaTime = 0.02f * Time.timeScale;
	}
	public void TransformAllIceCream() {
		for (int i = 0; i < activeWave.Length; i++) {
			if (activeWave[i] != null) {
				activeWave[i].TransformToIceCream();
			}
		}
	}
	public void ChangeUILang(int lang) {
		for (int i = 0; i < 2; i++) {
			confirmReturn[i].text = ct.system[lang].confirmReturn;
			
		}
		yesText.text = ct.system[lang].yes;
		noText.text = ct.system[lang].no;
		quit.text = ct.system[lang].quit;
		resume.text = ct.system[lang].resume;
	}
}
