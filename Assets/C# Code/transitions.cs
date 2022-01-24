using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;
using System.Collections;

public class transitions : MonoBehaviour {

	readonly int Menu = 0;
	readonly int Play = 1;
	readonly int Garage = 2;
	readonly int Credits = 3;
	readonly int Language = 4;
	readonly int PlayOptions = 5;
	readonly int IntroLogo = 6;
	readonly int Home = 7;
	readonly int HighScore = 8;

	//Canvas (Description)
	private TextMesh title;
	private Text description;
	//[HideInInspector]
	public museum[] vehicleList;
	
	
	//Animations
	[Tooltip("DO NOT EDIT UNLESS YOU KNOW EXACTLY WHAT THESE ARE")]
	public GameObject garageScene, creditsScene, creditsPanel, menuButtons, playOptions, languageOptions, menuTrack, highScore, silverOverLeft, silverOverRight, trackQuad;
	private garageCam garageCam;
	private Animator silverL, silverR, trackQ, credits, play, lang, menu, hsAnim;
	//public RuntimeAnimatorController[] silverLeft, silverRight, trackQuad, creditsA;
	private animHandler backTrack;

	//Vehicle Manipulation
	[HideInInspector]
	public GameObject[] storage;
	private int vehicleId = 1;
	private int vNumber = 6;
	private GameObject activeVehicle;
	[HideInInspector]
	public bool drag, change;
	private float start, end;
	[HideInInspector]
	public bool[] accessed;

	//Logo and Start screen Buttons
	private Animator teamAnim, startAnim;
	[Tooltip("DO NOT EDIT UNLESS YOU KNOW EXACTLY WHAT THESE ARE")]
	public GameObject startBar, startButton, teamName, startScreen;
	private Button startBtn, toTitleBtn;

	//Language Settings and Texts
	[HideInInspector]
	public TextMesh titleText;

	//I/O
	private string filename = "vehicleDescription";
	private TextAsset texts;
	private string line;
	private string[] splitter1 = new string[] { Environment.NewLine + Environment.NewLine + "::" + Environment.NewLine + Environment.NewLine };
	private string[] splitter2 = new string[] { Environment.NewLine + Environment.NewLine };
	private string[] splitter3 = new string[] { ":" + Environment.NewLine };
	private string[] result1, result2, result3;

	//Texts
	private int rSwitch;
	private TextMesh[] bPlay, bGallery, bRegion, bCredits;
	private TextMesh[] lSettings;
	private TextMesh[] cHeader, cBody;
	private TextMesh[] bArcade, bInfinite, bHelp, bHigh, bStartGame;
	private TextMesh[] tTP;
	private TextMesh logoWord;
	private TextMesh completion;
	

	//Results
	[Tooltip("DO NOT EDIT UNLESS YOU KNOW EXACTLY WHAT THESE ARE")]
	public GameObject results, highCanvas, resultCanvas;
	private Animator resultsAnim;
	[HideInInspector]
	public TextMesh[] resultsTitle, resultsText;
	private TextMesh[,] leaderText;
	private Animator highCAnim, resultCAnim;
	private TextMesh[] highResultsText;
	private Button nxtBtn, reRecordBtn;
	private Text[] playAgainBtn, toMenuBtn;
	private Text nextBtnText;
	private Animator[] highScoreTextAnim;

	//Audio
	public AudioSource[] bgm;

	//Access
	controlTower ct;
	textData td;
	main main;
	camBehaviour cam;
	sfxManager sfx;

	void Awake() {
		AudioListener.volume = 0.2f;
		cam = GameObject.FindWithTag("MainCamera").GetComponent<camBehaviour>();
		main = gameObject.GetComponent<main>();
		bgm = GameObject.FindWithTag("MainCamera").GetComponents<AudioSource>();
		sfx = gameObject.GetComponent<sfxManager>();//<-the second line.
		garageCam = GameObject.FindWithTag("MainCamera").GetComponent<garageCam>();
		ct = gameObject.GetComponent<controlTower>();
		td = gameObject.GetComponent<textData>();
		accessed = new bool[4];
		for (int i = 0; i < 4; i++) {
			accessed[i] = false;
		}
		ct.langId = 1;
		titleText = GameObject.Find("TitleName").GetComponent<TextMesh>();
		backTrack = trackQuad.GetComponent<animHandler>();
		silverL = silverOverLeft.GetComponent<Animator>();
		silverR = silverOverRight.GetComponent<Animator>();
		trackQ = trackQuad.GetComponent<Animator>();
		teamAnim = teamName.GetComponent<Animator>();

		title = GameObject.Find("VehicleName").GetComponent<TextMesh>();
		description = GameObject.Find("Description").GetComponent<Text>();

		startBtn = startScreen.GetComponent<Button>();
		startAnim = startBar.GetComponent<Animator>();
		menu = menuButtons.GetComponent<Animator>();
		play = playOptions.GetComponent<Animator>();
		lang = languageOptions.GetComponent<Animator>();
		credits = creditsPanel.GetComponent<Animator>();
		hsAnim = highScore.GetComponent<Animator>();

		//texts
		completion = GameObject.Find("Completion").GetComponent<TextMesh>();
		tTP = GameObject.Find("TapScrnTxt_White").GetComponentsInChildren<TextMesh>();
		logoWord = GameObject.Find("Game Name Txt_Team").GetComponent<TextMesh>();
		Transform menuTemp = menuButtons.transform;
		bPlay = menuTemp.FindChild("PlayButton").GetComponentsInChildren<TextMesh>();
		bGallery = menuTemp.FindChild("GalleryButton").GetComponentsInChildren<TextMesh>();
		bRegion = menuTemp.FindChild("LanguageButton").GetComponentsInChildren<TextMesh>();
		bCredits = menuTemp.FindChild("CreditsButton").GetComponentsInChildren<TextMesh>();
		lSettings = GameObject.Find("LSettingsTitle").GetComponentsInChildren<TextMesh>();
		cHeader = GameObject.Find("Credits Header").GetComponentsInChildren<TextMesh>();
		cBody = GameObject.Find("Credits_Default").GetComponentsInChildren<TextMesh>();
		Transform optionTemp = playOptions.transform;
		//bArcade = optionTemp.FindChild("ArcadeModeBtn").GetComponentInChildren<TextMesh>();
		//bInfinite = optionTemp.FindChild("InfiniteModeButton").GetComponentInChildren<TextMesh>();
		bHelp = optionTemp.FindChild("HowToPlayBtn").GetComponentsInChildren<TextMesh>();
		bHigh = optionTemp.FindChild("HighScoreBtn").GetComponentsInChildren<TextMesh>();
		bStartGame = optionTemp.FindChild ("StartGameBtn").GetComponentsInChildren<TextMesh>();

		//Result Screens
		resultsAnim = results.GetComponent<Animator>();
		highCAnim = highCanvas.GetComponent<Animator>();
		resultCAnim = resultCanvas.GetComponent<Animator>();
		highResultsText = GameObject.Find("HighResultsBody").GetComponentsInChildren<TextMesh>();
		nxtBtn = GameObject.Find("Next").GetComponent<Button>();
		toTitleBtn = GameObject.Find("BackToTitle").GetComponent<Button>();
		reRecordBtn = GameObject.Find("RecordScore").GetComponent<Button>();
		toMenuBtn = GameObject.Find("ToMenu").GetComponentsInChildren<Text>();
		playAgainBtn = GameObject.Find("PlayAgainBtn").GetComponentsInChildren<Text>();
		nextBtnText = GameObject.Find("NextTxt").GetComponent<Text>();
		highScoreTextAnim = new Animator[2];
		highScoreTextAnim[0] = GameObject.Find("HighScoreTxt").GetComponent<Animator>();
		highScoreTextAnim[1] = GameObject.Find("HighScoreTxt 1").GetComponent<Animator>();
		resultsTitle = GameObject.Find("ResultsTitle").GetComponentsInChildren<TextMesh>();
		resultsText = GameObject.Find("ResultsBody").GetComponentsInChildren<TextMesh>();
		TextMesh[] lTemp = highScore.GetComponentsInChildren<TextMesh>();
		leaderText = new TextMesh[10, 2];
		for (int i = 0; i < 10; i++) {
			for (int j = 0; j < 2; j++) {
				leaderText[i, j] = lTemp[i*2 + j];
			}
		}
		//End of Result Screens
		ReadFile();
		toTitleBtn.interactable = false;
		nxtBtn.interactable = false;

		drag = false;
		change = false;

		highCanvas.SetActive(false);
		resultCanvas.SetActive(false);
		results.SetActive(false);
		highScore.SetActive(false);
		startBar.SetActive(false);
		menuTrack.SetActive(false);
		startButton.SetActive(false);
		//startScreen.SetActive(false);
		menuButtons.SetActive(false);
		garageScene.SetActive(false);
		playOptions.SetActive(false);
		creditsScene.SetActive(false);
		languageOptions.SetActive(false);
		garageCam.enabled = false;
		teamAnim.Play("FullThingTEST");
	}
	// Update is called once per frame
	void Update () {
		if (teamName.activeInHierarchy && teamAnim.GetCurrentAnimatorStateInfo(0).IsName("Off")) {
			startBar.SetActive(true);
			bgm[0].Play();
			ct.LoadAll();
			ct.ChangeLanguage(ct.LoadLang());
			teamName.SetActive(false);
		}
		if (startBar.activeInHierarchy && startAnim.GetCurrentAnimatorStateInfo(0).IsName("Idle")) {
			startBtn.interactable = true;
			startButton.SetActive(true);
		}
		if (ct.state == Menu && Time.frameCount%200== 0) {
			bRegion[0].text = ct.system[rSwitch].region;
			bRegion[1].text = ct.system[rSwitch].region;
			if (rSwitch < ct.langOptions-1) {
				rSwitch++;
			} else {
				rSwitch = 0;
			}
		}
	}

	public void MenuToCredits(bool forward) {
		PlaySFX(forward);
		if (forward) {
			bgm[1].Stop();
			bgm[3].PlayDelayed(1);
			Open();
			ct.state = Credits;
			creditsScene.SetActive(true);
			credits.Play("In");
			//PlayerPrefs.DeleteAll();
			accessed[3] = true;
		} else {
			bgm[3].Stop();
			bgm[1].PlayDelayed(1);
			backTrack.enabled = true;
			Close();
			ct.state = Menu;
			credits.Play("Out");
		}
		
	}
	public void CtMFinished() {
		creditsScene.SetActive(false);
	}
	public void MenuToGarage(bool forward) {
		PlaySFX(forward);
		if (forward) {
			bgm[1].Stop();
			bgm[3].PlayDelayed(1);
			ct.state = Garage;
			ct.LoadUnlocks();
			accessed[1] = true;
			garageScene.SetActive(true);
			garageCam.enabled = true;
			CheckAccessed();
			CheckUnlocked();
			int numTemp = 0;
			for (int i = 0; i < vNumber; i++) {
				if (vehicleList[i].unlocked) {
					numTemp++;
				}
			}
			completion.text = numTemp + "/" + vNumber;
			backTrack.enabled = true;
			Open();
		} else {
			bgm[3].Stop();
			bgm[1].PlayDelayed(1);
			menuTrack.SetActive(true);
			backTrack.enabled = true;
			Close();
			ct.state = Menu;
			garageCam.enabled = false;
			change = false;
		}
		
	}

	public void StartToMenu(bool forward) {
		PlaySFX(forward);
		if (forward) {
			bgm[0].Stop();
			if (!bgm[1].isPlaying) {
				bgm[1].PlayDelayed(0.8f);
			}
			Change(0);
			startBtn.interactable = false;
			backTrack.enabled = true;
			ct.state = Menu;
			menuTrack.SetActive(true);
			menuButtons.SetActive(true);
			titleText.text = ct.system[ct.langId].name;
			ct.LoadUnlocks();
			Close();
		} else {
			ActivateTitleButton(true);
			startScreen.SetActive(true);
			startBtn.interactable = false;
			ct.state = IntroLogo;
			Open();
		}
		
	}

	public void Initialize() {
		drag = true;
	}

	public void MenuToPlay(bool forward) {
		PlaySFX(forward);
		if (forward) {
			toTitleBtn.interactable = false;
			ct.state = PlayOptions;
			playOptions.SetActive(true);
			//menuButtons.SetActive(false);
			menu.Play("Open");
			play.Play("Close");
			accessed[0] = true;
		} else {
			ActivateTitleButton(true);
			ct.state = Menu;
			//playOptions.SetActive(false);
			menuButtons.SetActive(true);
			play.Play("Open");
			menu.Play("Close");
		}
	}
	public void PlayToHighScore(bool state) {
		PlaySFX(state);
		if (state) {
			UpdateHighScore();
			ct.state = HighScore;
			highScore.SetActive(true);
			hsAnim.Play("Close");
			play.Play("Open");
		} else {
			ct.state = PlayOptions;
			playOptions.SetActive(true);
			play.Play("Close");
			hsAnim.Play("Open");
		}
	}
	public void TurnResults(bool on){
		if (on) {
			menuButtons.SetActive(false);
			results.SetActive(true);
			resultsAnim.Play("Close");
		} else {
			resultsAnim.Play("Open");
		}
	}
	public void MenuToLang(bool forward) {
		PlaySFX(forward);
		if (forward) {
			toTitleBtn.interactable = false;
			ct.state = Language;
			menu.Play("Open");
			languageOptions.SetActive(true);
			lang.Play("Close");
			accessed[2] = true;
		} else {
			ActivateTitleButton(true);
			ct.state = Menu;
			menu.Play("Close");
			lang.Play("Open");
		}
		
	}
	void Open() {
		//sfx.PlayMono(5, 0.5f, 1, 0, 2.0f);
		silverL.Play("Open");
		silverR.Play("Open");
		menu.Play("Open");
		trackQ.Play("Open");
	}
	void Close() {
		//sfx.PlayMono(6, 0.5f, 1, 0, 2.0f);
		silverL.Play("Close");
		silverR.Play("Close");
		menu.Play("Close");
		trackQ.Play("Close");
	}
	public void ChangeStart(float x) {
		start = x;
		change = true;
	}
	public void ChangeEnd(float x) {
		change = false;
	}
	public void Change(float x) {//vehicle swap
		if (change) {
			if (x < start - 50) {// to the left
				if (vehicleId == vNumber - 1) {//end of list
					if (CheckAccessed()) {//prototype is unlocked
						vehicleId = 0;
					} else {
						vehicleId = 1;
					}
				}else {
					vehicleId++;
				}
				change = false;
			} else if (x > start + 50) {// to the right
				if (vehicleId == 1) {
					if (CheckAccessed()) {
						vehicleId = 0;
					} else {
						vehicleId = vNumber - 1;
					}
				} else if(vehicleId == 0){
					vehicleId = vNumber - 1;
				} else {
					vehicleId--;
				}
				change = false;
			}
			CheckUnlocked();
		}
	}
	bool CheckAccessed() {
		for (int i = 0; i < 4; i++) {
			if (!accessed[i]) {
				return false;
			}
		}
		vehicleList[0].unlocked = true; //Unlocks hidden vehicle
		return true;
	}
	void CheckUnlocked() {
		if (!storage[vehicleId].activeInHierarchy) {
			if (activeVehicle != null) {
				activeVehicle.SetActive(false);
			}
			activeVehicle = storage[vehicleId];
		}
		activeVehicle.SetActive(true);
		if (vehicleList[vehicleId].unlocked) {
			if (vehicleId != 0) {
				vehicleList[vehicleId].vScript.Unlock(true);
			}
			title.text = vehicleList[vehicleId].name[ct.langId];
			description.text = vehicleList[vehicleId].description[ct.langId];
		} else {
			if (vehicleId != 0) {
				vehicleList[vehicleId].vScript.Unlock(false);
			}
			title.text = td.locked[0, ct.langId];
			description.text = td.locked[1, ct.langId];
		}
	}

	void ReadFile() {
		storage = new GameObject[vNumber];
			//filepath = Application.dataPath + "/GarageDescription.dat";//change to filepath = Application.persistentDataPath + "/GarageDescription.dat";
		vehicleList = new museum[vNumber];

		for (int i = 0; i < vNumber; i++) {
			try {
				vehicleList[i].vehicle = Resources.Load("GarageVehicles/Vehicle" + i.ToString()) as GameObject;
				GameObject temp = vehicleList[i].vehicle;
				storage[i] = Instantiate(temp, temp.transform.position, temp.transform.rotation) as GameObject;
				storage[i].transform.parent = garageScene.transform;
				vehicleList[i].name = new string[ct.langOptions];
				vehicleList[i].description = new string[ct.langOptions];
				//vehicleList[i].unlocked = false;
				vehicleList[i].vScript = storage[i].GetComponent<garageVehicle>();
			} catch (MissingReferenceException) {
				//Debug.LogError("Missing Prefab for Vehicle" + i);
			}
			storage[i].SetActive(false);
		}
		vehicleId = 1;
		ct.langId = 0;

		td.GetVehicleData(vNumber, ct.langOptions);
		//File Reader
		/*texts = Resources.Load(filename) as TextAsset;
		if (texts == null) {
			Debug.LogError("Failed to load file. Things may go funky from this point on.");
		} else {
			line = texts.text;
			if (line != null) {
				result1 = line.Split(splitter1, StringSplitOptions.None);
				for (int i = 0; i < vNumber; i++) {
					try {
						result2 = result1[i].Split(splitter2, StringSplitOptions.None);
						for (int j = 0; j < ct.langOptions; j++) {
							try {
								result3 = result2[j].Split(splitter3, StringSplitOptions.None);
								vehicleList[i].name[j] = result3[0];
								vehicleList[i].description[j] = result3[1];
							} catch (IndexOutOfRangeException) {
								Debug.LogError("You are either missing a name and/or description for one or more of the languages for Vehicle" + i + " or you have improper file formatting.");
							}

						}
					} catch (IndexOutOfRangeException) {
						Debug.LogError("Vehicle" + i + " description is missing or the txt file is formatted incorrectly.");
					}

				}
			}
		}/**/
	}
	public GameObject ActiveVehicle() {
		return storage[vehicleId];
	}
	
	public void Drag(Vector3 pos) {
		if (drag) {
			if(storage[vehicleId].activeInHierarchy){
				storage[vehicleId].transform.Rotate(pos.y * 1, pos.x * -1, 0, Space.World);
			}
		}
	}
	public void Lift() {
		if (drag) {
			drag = false;
		}
	}
	public void SystemLang(int id) {
		rSwitch = id;
		for (int i = 0; i < 2; i++) {
			tTP[i].text = ct.system[id].tapToStart;
			logoWord.text = ct.system[id].team;
			bPlay[i].text = ct.system[id].play;
			bGallery[i].text = ct.system[id].gallery;
			bRegion[i].text = ct.system[id].region;
			bCredits[i].text = ct.system[id].credits;
			lSettings[i].text = ct.system[id].languageSettings;
			cHeader[i].text = ct.system[id].credits;
			cBody[i].text = ct.system[id].PJL + Environment.NewLine + 
				"	-" + ct.system[id].RD + Environment.NewLine + Environment.NewLine +
				ct.system[id].LP + Environment.NewLine + 
				"	-" + ct.system[id].RD + Environment.NewLine + Environment.NewLine + 
				ct.system[id].ADL + Environment.NewLine + 
				"	-" + ct.system[id].SQ + Environment.NewLine + Environment.NewLine +
				ct.system[id].GDT + Environment.NewLine + 
				"	-" + ct.system[id].RD + Environment.NewLine + 
				"	-" + ct.system[id].SQ + Environment.NewLine + 
				"	-" + ct.system[id].SC + Environment.NewLine + Environment.NewLine + Environment.NewLine +
				ct.system[id].ST;
			bStartGame[i].text = ct.system[id].arcade;
			bHigh[i].text = ct.system[id].highScores;
			bHelp[i].text = ct.system[id].help;
			playAgainBtn[i].text = ct.system[id].playAgain;
			toMenuBtn[i].text = ct.system[id].toMenu;
		}
		nextBtnText.text = ct.system[id].next;
		
		main.ChangeUILang(id);
		//bArcade.text = ct.system[id].arcade;
		//bInfinite.text = ct.system[id].infinite;
		
	}
	public void Replay() {
		bgm[3].Stop();
		GameOn(ct.gameMode);
	}
	public void GameOn(int mode) {
		PlaySFX(true);
		bgm[1].Stop();
		ct.gameMode = mode;
		backTrack.enabled = true;
		ct.state = Play;
		main.gameScene.SetActive(true);
		if (playOptions.activeInHierarchy) {
			play.Play("Open");
		}
		if (results.activeInHierarchy) {
			resultsAnim.Play("Open");
		}
		Open();
		main.enabled = true;
		cam.ToGame();
		bgm[2].volume = 1.0f;
		bgm[2].PlayDelayed(0.5f);
		main.StartWaves();
	}
	public void ResultToMenu() {
		ActivateTitleButton(true);
		PlaySFX(true);
		bgm[3].Stop();
		resultsAnim.Play("Open");
		menuButtons.SetActive(true);
		menu.Play("Close");
		bgm[1].PlayDelayed(1);
	}
	public void GameToMenu(int dir) {//Initial finishgame
		toTitleBtn.interactable = false;
		if (dir == 0) {
			PlaySFX(true);
			bgm[1].PlayDelayed(1);
			menuButtons.SetActive(true);
			menu.Play("Close");
		}
		backTrack.enabled = true;
		ct.state = Menu;
		Close();
	}
	public void GameFinish() {
		cam.ToMenu();
		main.FinishGame();
		main.gameScene.SetActive(false);
		main.enabled = false;
	}
	public void UpdateHighScore() {
		for (int i = 0; i < 10; i++) {
			for (int j = 0; j < 2; j++) {
				leaderText[i, j].text = (i + 1) + ": " + ct.leaderboard[i].name + " " + ct.leaderboard[i].score;
				if (i == 0 && ct.leaderboard[i].score > 0 && ct.leaderboard[i].name.Equals("---")) {
					leaderText[i, j].text = (i + 1) + ": 『   』 " + ct.leaderboard[i].score;
				}
			}
		}
	}
	public void RecordScore() {
		StartCoroutine(InputTime(ct.GetRankIdx(), ct.GetScore(), false));
	}
	public void Eligible(bool high) {
		if (high) {
			highCanvas.SetActive(true);
			reRecordBtn.interactable = false;
			nxtBtn.interactable = false;
			highCAnim.Play("Close");
			highScoreTextAnim[0].Play("RainbowTxt_Looping");
			highScoreTextAnim[1].Play("RainbowTxt_Looping-Reverse");
			sfx.PlayMono("Fanfare.mp3", 1.0f);
			bgm[3].PlayDelayed(4);
			for (int i = 0; i < highResultsText.Length; i++) {
				highResultsText[i].text = (ct.GetRankIdx()+1) + ": " + Environment.NewLine +
					Environment.NewLine +
					ct.GetScore();
			}
			StartCoroutine(InputTime(ct.GetRankIdx(), ct.GetScore(), true));
		} else {
			bgm[3].Play();
			resultCanvas.SetActive(true);
			resultCAnim.Play("Close");
		}
	}
	IEnumerator InputTime(int idx, int sc, bool first){
		if (first) {
			yield return new WaitForSeconds(1);
		}
		string tempString = "";
		TouchScreenKeyboard keyboard;
		keyboard = TouchScreenKeyboard.Open(tempString, TouchScreenKeyboardType.Default, false, false, false, false, "---");
		while (true) {
			if (keyboard.done) {
				break;
			}
			yield return 1;
		}
		if (keyboard.text == "") {
			tempString = "---";
		} else if (keyboard.text.Length > 3) {
			tempString = keyboard.text.Substring(0, 3);
		} else {
			tempString = keyboard.text;
		}
		ct.OverWrite(idx, sc, tempString, first);
		if (idx == 0 && tempString.Equals("")) {
			tempString = "『   』";
		} 
		for (int i = 0; i < highResultsText.Length; i++) {
			highResultsText[i].text = (ct.GetRankIdx()+1) + ":  " + tempString + Environment.NewLine +
				Environment.NewLine +
				ct.GetScore();
		}
		UpdateHighScore();
		if (first) {
			yield return new WaitForSeconds(0.5f);
			reRecordBtn.interactable = true;
			nxtBtn.interactable = true;
		}
		
	}
	public void ToNextResult() {
		ct.SaveAll();
		PlaySFX(true);
		nxtBtn.interactable = false;
		highCAnim.Play("Open");
		resultCanvas.SetActive(true);
		resultCAnim.Play("Close");
	}
	public void DeactivateResults() {
		highCanvas.SetActive(false);
		resultCanvas.SetActive(false);
	}
	public void PlayBGM(int idx) {
		bgm[idx].Play();
	}
	public void PlaySFX(bool forward) {
		sfx.PlayMono((forward ? "Menu-Forward.mp3" : "Menu-Backward.mp3"), (forward ? 1.0f : 0.9f));
	}
	public void DisplayVideo() {
		PlaySFX(true);
		Handheld.PlayFullScreenMovie(ct.langId.ToString() + "Video.mp4", Color.black, FullScreenMovieControlMode.CancelOnInput, FullScreenMovieScalingMode.AspectFit);
	}
	public void ActivateTitleButton(bool activate) {
		toTitleBtn.interactable = activate;
	}
	public void DeactivateNxt() {
		nxtBtn.interactable = false;
		reRecordBtn.interactable = false;
	}
}
