using UnityEngine;
using System.Collections;

public class textData : MonoBehaviour {

	private controlTower ct;
	private transitions tr;

	public string[,] locked;

	public void CreateData() {
		ct = gameObject.GetComponent<controlTower>();

		ct.system[0].language = "English";
		ct.system[0].name = "Spin-Out";
		ct.system[0].team = "Team";
		ct.system[0].tapToStart = "Tap the screen to start!";
		ct.system[0].play = "Play";
		ct.system[0].gallery = "Gallery";
		ct.system[0].region = "Region";
		ct.system[0].credits = "Credits";
		ct.system[0].languageSettings = "Language Settings";
		ct.system[0].RD = "Ryota Dan";
		ct.system[0].SQ = "Sterling Quinn";
		ct.system[0].SC = "Spaulding Cummings";
		ct.system[0].PJL = "Project Lead";
		ct.system[0].LP = "Lead Programmer";
		ct.system[0].ADL = "Art Design Lead";
		ct.system[0].GDT = "Game Design Team";
		ct.system[0].ST = "Original Soundtrack composed by Kanki Suzuki";
		ct.system[0].arcade = "Start Game";
		ct.system[0].classic = "Classic";
		ct.system[0].infinite = "Infinite";
		ct.system[0].challenge = "Challenge";
		ct.system[0].help = "How to Play";
		ct.system[0].highScores = "High Scores";
		ct.system[0].results = "Results";
		ct.system[0].score = "Total Score: ";
		ct.system[0].wavesSurvived = "Waves Survived: ";
		ct.system[0].tiresCollected = "Tires Collected: ";
		ct.system[0].powerupsUsed = "Power-Ups Used: ";
		ct.system[0].vDestroyed = "Vehicles Destroyed: ";
		ct.system[0].vMissed = "Vehicles Missed: ";
		ct.system[0].maxChain = "Max Chain Combo: ";
		ct.system[0].grandPlace = @" 	^^ #1 All-Star!!! \(^0^)/";
		ct.system[0].toNext = "To Rank ";
		ct.system[0].toNext2 = ": ";
		ct.system[0].yes = "Yes";
		ct.system[0].no = "No";
		ct.system[0].confirmReturn = "Return to Main Menu?";
		ct.system[0].resume = "Resume";
		ct.system[0].quit = "Quit";
		ct.system[0].playAgain = "Replay";
		ct.system[0].toMenu = "To Menu";
		ct.system[0].next = "Next";
		
		ct.system[1].language = "Japanese";
		ct.system[1].name = "スピン・アウト";
		ct.system[1].team = "チーム";
		ct.system[1].tapToStart = "タップしてスタート！";
		ct.system[1].play = "プレイ";
		ct.system[1].gallery = "ギャラリー";
		ct.system[1].region = "言語";
		ct.system[1].credits = "スタッフ";
		ct.system[1].languageSettings = "言語設定";
		ct.system[1].RD = "段亮太";
		ct.system[1].SQ = "Sterling Quinn";
		ct.system[1].SC = "Spaulding Cummings";
		ct.system[1].PJL = "プロジェクトリーダー";
		ct.system[1].LP = "リードプログラマー";
		ct.system[1].ADL = "リードグラフィックアーティスト";
		ct.system[1].GDT = "コンセプト開発チーム";
		ct.system[1].ST = "オリジナルサウンドトラック　鈴木歓喜";
		ct.system[1].arcade = "スタート";
		ct.system[1].classic = "クラシック";
		ct.system[1].infinite = "インフィニティー";
		ct.system[1].challenge = "チャレンジ";
		ct.system[1].help = "遊び方";
		ct.system[1].highScores = "ハイスコア";
		ct.system[1].results = "結果";
		ct.system[1].score = "総合結果: ";
		ct.system[1].wavesSurvived = "生存段数: ";
		ct.system[1].tiresCollected = "集めたタイヤ: ";
		ct.system[1].powerupsUsed = "特殊能力使用回数: ";
		ct.system[1].vDestroyed = "車破壊数: ";
		ct.system[1].vMissed = "逃走車数: ";
		ct.system[1].maxChain = "最大チェインコンボ: ";
		ct.system[1].grandPlace = @" 	^^ 最高順位!!! \(^0^)/";
		ct.system[1].toNext = "";
		ct.system[1].toNext2 = "位まで: ";
		ct.system[1].yes = "はい";
		ct.system[1].no = "いいえ";
		ct.system[1].confirmReturn = "メニューに戻りますか？";
		ct.system[1].resume = "続ける";
		ct.system[1].quit = "やめる";
		ct.system[1].playAgain = "再プレイ";
		ct.system[1].toMenu = "メニューへ";
		ct.system[1].next = "次へ";

		ct.system[2].language = "German";//editing this line has no effect whatsoever
		ct.system[2].name = "Spin-Out";
		ct.system[2].team = "Team";
		ct.system[2].tapToStart = "Berühre den Bildschirm\n um das Spiel zu starten!";
		ct.system[2].play = "Spiel";
		ct.system[2].gallery = "Gallerie";
		ct.system[2].region = "Sprache";
		ct.system[2].credits = "Kredite";
		ct.system[2].languageSettings = "Spracheinstellungen";
		ct.system[2].RD = "Ryota Dan";
		ct.system[2].SQ = "Sterling Quinn";
		ct.system[2].SC = "Spaulding Cummings";
		ct.system[2].PJL = "Projektleiter";
		ct.system[2].LP = "Leiter Programmierung";
		ct.system[2].ADL = "Leiter Grafische Gestaltung";
		ct.system[2].GDT = "Leiter Spielgestaltung";
		ct.system[2].ST = "Original Soundtrack komponiert von Kanki Suzuki";
		ct.system[2].arcade = "Spiel starten";
		ct.system[2].classic = "Klassisch";
		ct.system[2].infinite = "Unendlich";
		ct.system[2].challenge = "Herausforderung";
		ct.system[2].help = "Spielanleitung";
		ct.system[2].highScores = "Höchststand";
		ct.system[2].results = "Resultate";
		ct.system[2].score = "Totaler Spielstand: ";
		ct.system[2].wavesSurvived = "Ueberstandene Wellen: ";
		ct.system[2].tiresCollected = "Gesammelte Reifen : ";
		ct.system[2].powerupsUsed = "Genutzte Energieschübe: ";
		ct.system[2].vDestroyed = "Zerstörte Fahrzeuge: ";
		ct.system[2].vMissed = "Verfehlte Fahrzeuge: ";
		ct.system[2].maxChain = "Maximale Kettenkombination: ";
		ct.system[2].grandPlace = @" 	^^ Bester Spieler!!! \(^0^)/";
		ct.system[2].toNext = "Plazieren";
		ct.system[2].toNext2 = ": ";
		ct.system[2].yes = "Ja";
		ct.system[2].no = "Nein";
		ct.system[2].confirmReturn = "Zum Hauptmenü?";
		ct.system[2].resume = "Weiterspielen";
		ct.system[2].quit = "Beenden";
		ct.system[2].playAgain = "Nochmals \nspielen";
		ct.system[2].toMenu = "Zum Menü";
		ct.system[2].next = "Nächster";
	}

	public void GetVehicleData(int num, int opt) {
		string[,] temp = {
			{"Welcome to the Gallery!", "ギャラリーへようこそ！", "Willkommen in der Gallerie!"},
			{"You can view cars and things you encounter in the game here!","ここでは今までに遭遇した車などを観覧できます！","Hier kannst Du alle Autos und Fahrzeuge sehen, die im Spiel vorkommen!"}
						 };
		locked = temp;
		tr = gameObject.GetComponent<transitions>();
		string[,] names = {//Edit these for Vehical Names
						  {"Spin-Out! Prototype","プロトタイプ", "Spin-Out! Prototyp"},
						  {"Spin-Out! Classic","Spin-Out! クラシック", "Spin-Out! Klassisch"},
						  {"Golden Classic Ltd.", "クラシック（黄金）", "Golden Classic Ltd."},
						  {"Tire Truck", "タイヤ輸送車", "Lastwagen mit Reifen beladen"},
						  {"S.W.A.T. Bus", "S.W.A.T. バス", "S.W.A.T. Bus"},
						  {"Ice Cream Truck", "アイスクリームトラック", "Eiscreme Wagen"}
						  };
		for (int i = 0; i < num; i++) {
			for (int j = 0; j < opt; j++) {
				tr.vehicleList[i].name[j] = names[i, j];
			}
		}
		string[,] descriptions = {//Edit these for Vehical Names
									 {
										 "Congratulations! You’ve visited every menu in the game!\n\nAs a reward, this is the original test car we used during the first few weeks of development on Spin-Out!", 
										 "ゲーム開発時にテスト用として使われていたプロトタイプ。メニュー内全てのオプションを散策した報酬。", 
										 "Super! Du hast alle Menüs im Spiel besucht!\n\nAls Belohnung, hier ist das original Testfahrzeug, das wir während den ersten Wochen der Spielentwicklung benützt haben."
									 },
									 {
										 "One can never go wrong with this classic racing machine! \n\nThis car is loosely based on vintage F-1 race cars. (With a paint job reminiscent of a certain Speedy Racer)", 
										 "標準的な車。とある高速レーサーの愛車を基にしたF-1用のレーシングカー。", 
										 "Dieses klassische Rennauto ist nie die falsche Wahl. Das Design ist von alten Rennwagen abgeleitet (mit einer Bemalung die an einen bestimmten rasenden Wagen erinnert)."
									 },
									 {
										 "Truly the shiniest of cars!\nLike the standard Classic but entirely golden.\n\nAn elusive vehicle, causing it to spin-out yields three times the points! When it first collides, it breaks into three spinning cars!", 
										 "黄金化されたクラシックカー。破壊成功時には通常の3倍のポイントが稼げる上、さらに中から同車が3台出てくる。", 
										 "Ganz sicher das glänzendste Auto! Wie der klassische Standard, aber ganz in Gold. Ein sehr schnelles Auto; Du erhältst die dreifache Punktezahl, wenn Du es von der Strasse abdrängst. Bei der ersten Kollision teilt es sich in drei Fahrzeuge."
									 },
									 {
										 "Based on the 1975 Mercedes Open-Bed Truck, this handy vehicle carries Tires-a-plenty!\n\nSpin this car out to rapidly rack up Tires fast!", 
										 "1975年に作られたドイツ製トラック。他車にぶつけることによって運ばれているタイヤが落ちることがある。運がよければ黄金タイヤが落ちることも？", 
										 "Basierend auf einem offenen Mercedes Lieferwagen von 1975, dieses praktische Fahrzeug ist mit unzähligen Reifen beladen! Dräng dieses Fahrzeug von der Strasse ab und Du erhältst viele Reifen."
									 },
									 {
										 "You don’t want to see this thing outside your house! This incredible behemoth is the only line of order on the roadway! However, ramming cars into a bus or placing a dash panel below one will move the bus up or down slightly, so plan accordingly! Consider it a wall; A giant, fast, moving wall. On wheels.", 
										 "特殊火器戦術部隊のバス。その巨体から予測できる通り他の車と衝突してもほとんど影響を受けない。繰り返し車をぶつけることで若干動く可能性あり。なお、この車が家の前に止まっているときは相当の裁きを受ける前触れであることをお忘れなく。", 
										 "Dieses Ding willst Du nicht vor Deinem Haus sehen! Dieser riesige Bus ist das grösste Fahrzeug auf der Strasse. Wenn Du ihn rammst oder eine Beschleunigungsrampe unter ihn legst, wird er sich leicht nach vorne oder hinten bewegen. Also pass’ auf! Dies ist eine Wand; eine riesige, sich schnell bewegende Wand. Auf Raedern."
									 },
									 {
										"Ice cream can be a tasty treat, but too much ice cream can be a hassle! Letting anything touch this car will turn every car on-screen into an ice cream cone! Unfortunately, you can’t get points or time from ice cream cones so avoid letting one of these get touched at all costs!",
										"アイスクリームは確かにおいしいが食べ過ぎに注意！　このトラックを破壊してしまうと全ての車がアイスクリームと化してしまう。なお、アイスクリーム化してしまった車からは一切時間やポイントは稼げないので要注意！",
										"Eiscreme schmeckt gut, aber zuviel kann auch schaden! Alle Autos werden in Eistüten verwandelt wenn etwas diesen Wagen berührt. Leider bekommst Du keine Punkte von Eistüten, deshalb solltest Du es vermeiden, diesen Wagen zu berühren."
									 }
									 
								 };
		for (int i = 0; i < num; i++) {
			for (int j = 0; j < opt; j++) {
				tr.vehicleList[i].description[j] = descriptions[i, j];
			}
		}
	}
}
