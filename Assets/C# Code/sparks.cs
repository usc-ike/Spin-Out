using UnityEngine;
using System.Collections;

public class sparks : MonoBehaviour {

	public ParticleSystem spark;

	void Awake() {
		spark = gameObject.GetComponent<ParticleSystem>();
	}

	void OnEnable() {
		spark.Play();
	}
	void Update() {
		if (!spark.IsAlive()) {
			this.Recycle();
		}
	}
}
