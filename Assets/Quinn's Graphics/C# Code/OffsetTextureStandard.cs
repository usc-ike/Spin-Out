using UnityEngine;
using System.Collections;

public class OffsetTextureStandard: MonoBehaviour {
	
	public float Speed = 0.4f;
	private Material mat;
	private float offsetX;

	void Awake(){
		mat = GetComponent<Renderer>().material;
	}

	void Update () {
		offsetX += Time.deltaTime * Speed;
		mat.mainTextureOffset = new Vector2(offsetX%1,0);
	}

	public void Initiate(float x) {
		offsetX = x;
	}
}
