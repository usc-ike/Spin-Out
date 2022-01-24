using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public struct museum {
	public GameObject vehicle;
	public garageVehicle vScript;
	public string[] name;
	public string[] description;
	public bool unlocked;
}
[System.Serializable]
public struct sysLang {
	public string name, team, tapToStart;
	public string language;
	public string play;
	public string gallery;
	public string region;
	public string credits;
	public string languageSettings;
	public string RD, SQ, SC, PJL, LP, ADL, GDT, ST;
	public string arcade, classic, infinite, challenge, help, highScores;
	public string results, score, wavesSurvived, tiresCollected, powerupsUsed;
	public string vDestroyed, vMissed, maxChain, grandPlace, toNext, toNext2;
	public string yes, no, confirmReturn, resume, quit, playAgain, toMenu, next;
}
[System.Serializable]
public struct vehicleList {
	public protoMvmt vehicle;//Vehicle itself
	public protoMvmt vehicleDat;//Script of the specific vehicle
	public Vector3 position;
};
[System.Serializable]
public struct highscores {
	public string name;
	public int score;
}
[System.Serializable]
public struct powerUps {
	public GameObject icon;
	public int rate;
	public float duration;
}

public class controlTower : MonoBehaviour {

	public readonly int Menu = 0;
	public readonly int Play = 1;
	public readonly int Garage = 2;
	public readonly int Credits = 3;
	public readonly int Language = 4;
	public readonly int PlayOptions = 5;
	public readonly int IntroLogo = 6;
	public readonly int Home = 7;

	public readonly int Arcade = 0;
	public readonly int Classic = 1;

	[HideInInspector]
	public int state;
	[HideInInspector]
	public int gameMode;
	[HideInInspector]
	public int langId; //transfer to control tower on fusion
	
	//I/O
	[HideInInspector]
	public highscores[] leaderboard;
	[HideInInspector]
	public sysLang[] system;
	[HideInInspector]
	public int langOptions = 3;
	private string systemname = "system";
	private string[] splitter1 = {"}" + Environment.NewLine};
	private string[] splitter2 = { ";" + Environment.NewLine, "{" + Environment.NewLine};

	private string[] result1, result2;
	
	private TextAsset text;
	private string line;

	//ResultScreen Text
	private string newline = Environment.NewLine;
	private int rank, pScore;
	


	//Access
	private main main;
	private transitions tr;
	private textData td;

	void Awake() {
		main = gameObject.GetComponent<main>();
		td = gameObject.GetComponent<textData>();
		tr = gameObject.GetComponent<transitions>();
		leaderboard = new highscores[10];
		LoadLeaderboard();
		system = new sysLang[langOptions];
		state = IntroLogo;
		td.CreateData();
		
		//ReadSystem();
		
	}

	public int GetState() {
		return state;
	}

	public void ChangeLanguage(string lan) {
		switch (lan) {
			case "English":
				langId = 0;
				break;
			case "Japanese":
				langId = 1;
				break;
			case "German":
				langId = 2;
				break;
			default:
				break;
		}
		SaveLang();
		tr.titleText.text = system[langId].name;
		tr.SystemLang(langId);
	}

	void ReadSystem() {
		text = Resources.Load(systemname) as TextAsset;
		if (text == null) {
			//Debug.LogError(systemname + ".txt was not found.");
		} else {
			line = text.text;
			if (line != null) {
				result1 = line.Split(splitter1, StringSplitOptions.RemoveEmptyEntries);

				for (int i = 0; i < 2; i++) {
					result2 = result1[i].Split(splitter2, StringSplitOptions.RemoveEmptyEntries);
					
					system[i].language = result2[0];
					system[i].play = result2[1];
					system[i].gallery = result2[2];
					system[i].region = result2[3];
					system[i].credits = result2[4];
					system[i].languageSettings = result2[5];
					system[i].RD = result2[6];
					system[i].SQ = result2[7];
					system[i].SC = result2[8];
					system[i].PJL = result2[9];
					system[i].LP = result2[10];
					system[i].ADL = result2[11];
					system[i].GDT = result2[12];
					system[i].ST = result2[13];
					system[i].arcade = result2[14];
					system[i].classic = result2[15];
					system[i].infinite = result2[16];
					system[i].challenge = result2[17];
					system[i].help = result2[18];
				}
			}
		}
	}

	public void SaveAll() {
		//print("saving");
		//language state
		SaveLang();
		//highscore: name + score + game mode
		for (int i = 0; i < leaderboard.Length; i++) {
			PlayerPrefs.SetString("LeaderBoardName" + i.ToString(), leaderboard[i].name);
			PlayerPrefs.SetInt("LeaderBoardScore" + i.ToString(), leaderboard[i].score);
		}
		//unlocked vehicles
		for (int i = 0; i < tr.vehicleList.Length; i++) {
			PlayerPrefs.SetInt("UnlockState" + i.ToString(),
				(tr.vehicleList[i].unlocked ? 1 : 0));
		}
		//unlocked access
		for (int i = 0; i < 4; i++) {
			PlayerPrefs.SetInt("Accessed" + i.ToString(), tr.accessed[i] ? 1 : 0);
		}
	}
	public void LoadAll(){
		//print("loading");
		LoadLang();
		for (int i = 0; i < leaderboard.Length; i++) {
			leaderboard[i].name = PlayerPrefs.GetString("LeaderBoardName" + i.ToString());
			leaderboard[i].score = PlayerPrefs.GetInt("LeaderBoardScore" + i.ToString());
		}
		//unlocked vehicles
		LoadUnlocks();
		//unlocked access
		for (int i = 0; i < 4; i++) {
			tr.accessed[i] = (PlayerPrefs.GetInt("Accessed" + i.ToString()) == 1 ? true : false);
		}
	}
	public void SaveLang() {
		PlayerPrefs.SetInt("Language", (langId+1));
	}
	public string LoadLang() {
		langId = PlayerPrefs.GetInt("Language");
		switch (langId) {
			case 1:
				return "English";
			case 2:
				return "Japanese";
			case 3:
				return "German";
			default:
				return "Japanese";
		}
	}
	public void LoadUnlocks() {
		for (int i = 0; i < tr.vehicleList.Length; i++) {
			tr.vehicleList[i].unlocked = (PlayerPrefs.GetInt("UnlockState" + i.ToString()) == 1? true: false);
		}
	}

	public void ResultDisplay(int st, int wc, int tt, int pu, int vd, int maxChain, int missed) {
		tr.TurnResults(true);
		pScore = st;
		rank = CheckRanking(st);
		string stTemp;
		if(rank == 0){
			stTemp = system[langId].grandPlace;
			tr.Eligible(true);
		}else{
			stTemp = system[langId].toNext + (rank == -100 ? "10" : (rank).ToString()) + system[langId].toNext2 + 
				(rank == -100 ? (leaderboard[9].score-st):(leaderboard[rank-1].score - st));
			tr.Eligible((rank == -100 ? false : true));
		}
		for (int i = 0; i < 2; i++) {
			tr.resultsTitle[i].text = system[langId].results;
			tr.resultsText[i].text =
				system[langId].score + st + newline + 
				stTemp + newline +
				system[langId].wavesSurvived + wc + newline +
				system[langId].tiresCollected + tt + newline +
				system[langId].powerupsUsed + pu + newline +
				system[langId].vDestroyed + vd + newline +
				system[langId].vMissed + missed + newline + 
				system[langId].maxChain + maxChain;
		}
	}
	int CheckRanking(int st) {//returns ranking
		for (int i = 9; i >= 0; i--) {
			if (st < leaderboard[i].score) {
				if (i == 9) {
					return -100;
				} else {
					return (i+1);
				}
			}
		}
		return 0;
	}
	public void OverWrite(int index, int sc, string name, bool overwrite){
		for (int i = 0; i < leaderboard.Length; i++) {
			leaderboard[i].name = PlayerPrefs.GetString("LeaderBoardName" + i.ToString());
			leaderboard[i].score = PlayerPrefs.GetInt("LeaderBoardScore" + i.ToString());
		}
		if (overwrite) {
			for (int i = 8; i >= index; i--) {
				leaderboard[i + 1].name = leaderboard[i].name;
				leaderboard[i + 1].score = leaderboard[i].score;
			}
		}
		leaderboard[index].name = name;
		leaderboard[index].score = sc;
	}
	public void LoadLeaderboard() {
		for (int i = 0; i < 10; i++) {
			leaderboard[i].name = PlayerPrefs.GetString("Name" + i.ToString()) != "" ? PlayerPrefs.GetString("Name" + i.ToString()): "---";
			leaderboard[i].score = PlayerPrefs.GetInt("Score" + i.ToString());
		}
		tr.UpdateHighScore();
	}
	public int GetRankIdx() {
		return rank;
	}
	public int GetScore() {
		return pScore;
	}
}