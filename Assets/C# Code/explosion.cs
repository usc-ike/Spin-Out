using UnityEngine;
using System.Collections;

public class explosion : MonoBehaviour {

	public ParticleSystem[] exParts;
	private int particleCount = 6;

	void Awake() {
		exParts = new ParticleSystem[particleCount];
		exParts = gameObject.GetComponentsInChildren<ParticleSystem>();
	}

	void OnEnable() {
		for (int i = 0; i < particleCount; i++) {
			exParts[i].Play();
		}
	}
	void Update() {
		if (!Activity()) {
			this.Recycle();
		}
	}

	bool Activity() {
		for (int i = 0; i < particleCount; i++) {
			if (exParts[i].IsAlive()) {
				return true;
			}
		}
		return false;
	}
}
