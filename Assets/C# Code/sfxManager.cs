using UnityEngine;
using System.Collections;

public class sfxManager : MonoBehaviour {

	[Tooltip("List all the SFX that are going to be in use here. Make sure to add the file type.")]
	public string[] sfxFiles;
	[Tooltip("How many SFX is the game allowed to play at the same time?")]
	public int maxSounds = 10;

	private float def_Volume = 1.0f;
	private int def_Priority = 1;
	private int def_Loop = 0;
	private float def_Rate = 1.0f;
	private int bgmStream;
	

	private AndroidJavaObject sfxUnityContext;
	private AndroidJavaObject sfxUnitySounds;

	// Use this for initialization
	void Awake () {
		if (Application.platform == RuntimePlatform.Android && sfxUnitySounds == null) {
			using (AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
				sfxUnityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
			}
			using (AndroidJavaClass pluginClass = new AndroidJavaClass("com.sfx.unityplugin.UnitySfx")) {
				sfxUnitySounds = pluginClass.CallStatic<AndroidJavaObject>("SetUp", maxSounds, sfxUnityContext);
			}
		} else {
			Debug.LogWarning("UnitySFX only works on an Android device");
		}
		for (int i = 0; i < sfxFiles.Length; i++) {
			SfxRegister(sfxFiles[i]);
		}
	}

	//Play Sounds From SoundPool
	public int Play(int idx, float lVolume, float rVolume, int priority, int loop, float rate) {
		if (Application.platform == RuntimePlatform.Android) {
			if (rate > 2.0f) {
				rate = 2.0f;
			} else if (rate < 0.5f) {
				rate = 0.5f;
			}
			if (lVolume > 1.0f) {
				lVolume = 1.0f;
			} else if (lVolume < 0.0f) {
				lVolume = 0.0f;
			}
			if (rVolume > 1.0f) {
				rVolume = 1.0f;
			} else if (rVolume < 0.0f) {
				rVolume = 0.0f;
			}
			return sfxUnitySounds.Call<int> ("Play", idx + 1, lVolume, rVolume, priority, loop, rate);
		}
		return 0;
	}

	public void PlayBG(int idx, float volume, int loop) {
		bgmStream = Play(idx, volume, volume, 100, loop, 1.0f);
	}

	public void Play(int idx) {
		Play(idx, def_Volume, def_Volume, def_Priority, def_Loop, def_Rate);
	}

	public void PlayMono(int idx, float volume) {
		Play(idx, volume, volume, def_Priority, def_Loop, def_Rate);
	}

	public void PlayMono(int idx, float volume, int loop) {
		Play(idx, volume, volume, def_Priority, loop, def_Rate);
	}

	public void PlayMono(int idx, float volume, int priority, int loop, float rate) {
		Play(idx, volume, volume, priority, loop, rate);
	}

	//Stereo
	public void PlayStereo(int idx, float lVolume, float rVolume) {
		Play(idx, lVolume, rVolume, def_Priority, def_Loop, def_Rate);
	}

	public void PlayStereo(int idx, float lVolume, float rVolume, int loop) {
		Play(idx, lVolume, rVolume, def_Priority, loop, def_Rate);
	}

	public void PlayStereo(int idx, float lVolume, float rVolume, int loop, int rate) {
		Play(idx, lVolume, rVolume, def_Priority, loop, rate);
	}

	//Get By Name
	public void Play(string name, float lVolume, float rVolume, int priority, int loop, float rate) {
		for (int i = 0; i < sfxFiles.Length; i++) {
			if (string.CompareOrdinal(sfxFiles[i], name) == 0) {
				Play(i, lVolume, rVolume, priority, loop, rate);
			}
		}
	}

	int PlayByString(string name, float lVolume, float rVolume, int priority, int loop, float rate) {
		for (int i = 0; i < sfxFiles.Length; i++) {
			if (string.CompareOrdinal(sfxFiles[i], name) == 0) {
				return Play(i, lVolume, rVolume, priority, loop, rate);
			}
		}
		return 0;
	}

	public void PlayBG(string name, float volume, int loop) {
		bgmStream = PlayByString(name, volume, volume, 100, loop, 1.0f);
	}

	public void Play(string name) {
		PlayByString(name, def_Volume, def_Volume, def_Priority, def_Loop, def_Rate);
	}

	public void PlayMono(string name, float volume) {
		PlayByString(name, volume, volume, def_Priority, def_Loop, def_Rate);
	}

	public void PlayStereo(string name, float lVolume, float rVolume) {
		PlayByString(name, lVolume, rVolume, def_Priority, def_Loop, def_Rate);
	}

	//Play According to Position
	/*public int PlayFromPosition(int idx, Vector3 position, int priority, int loop) {
		float width = Screen.width;

		return sfxUnitySounds.Call<int>("Play", idx, lVolume, rVolume, priority, loop, 1);
	}/**/

	//Register Sounds On SoundPool
	public int SfxRegister(string fName) {
		if (sfxUnitySounds != null) {
			return sfxUnitySounds.Call<int>("Register", fName);
		}
		return -1;
	}
	//Pause Specific Sound
	public void Pause(int id) {
		sfxUnitySounds.Call("Pause", id);
	}
	//Resumes Specific Sound
	public void Resume(int id) {
		sfxUnitySounds.Call("Resume", id);
	}
	//Stops Sound
	public void Stop(int id) {
		sfxUnitySounds.Call("Stop", id);
	}
	//Stops BGM
	public void StopBG() {
		Stop(bgmStream);
	}
	//Sets Rate
	public void ChangeRate(int id, float rate) {
		sfxUnitySounds.Call("SetRate", id, rate);
	}
	//Change Volume
	public void ChangeStereoVolume(int id, float lVolume, float rVolume) {
		sfxUnitySounds.Call("SetVolume", id, lVolume, rVolume);
	}

	public void ChangeMonoVolume(int id, float volume) {
		ChangeStereoVolume(id, volume, volume);
	}
	//Changes Loop
	public void ChangeLoop(int id, int loop) {
		sfxUnitySounds.Call("SetLoop", id, loop);
	}
	//Pause All Sounds
	public void PauseAll() {
		sfxUnitySounds.Call("PauseAll");
	}

	//Resume All Sounds
	public void ResumeAll() {
		sfxUnitySounds.Call("ResumeAll");
	}
}
