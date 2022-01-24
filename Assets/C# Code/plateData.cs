using UnityEngine;
using System.Collections;

public class plateData : MonoBehaviour {
	public Vector3 direction;
	private Transform pTrans;
	private Collider pCollider;
	private GameObject boundary;
	private MeshFilter bRend;
	public Mesh meshD, meshF;
	public MeshRenderer bMeshR;
	private Renderer pRend;
	public Material matD, matF;
	private OffsetTextureStandard ots;
	private Animator pAnim;

	void Awake(){
		pRend = gameObject.GetComponentInChildren<Renderer>();
		ots = gameObject.GetComponentInChildren<OffsetTextureStandard>();
		pTrans = gameObject.transform;
		pCollider = gameObject.GetComponentInChildren<Collider>();
		boundary = pTrans.GetChild(1).gameObject;
		pAnim = boundary.GetComponent<Animator>();
		bRend = boundary.GetComponent<MeshFilter>();
		bMeshR = boundary.GetComponent<MeshRenderer>();
	}

	void OnEnable(){
		boundary.SetActive(true);
		pRend.material.mainTextureOffset = new Vector2(1, 0);
		ots.enabled = false;
		pCollider.enabled = false;
		bRend.mesh = meshD;
		pAnim.enabled = true;
		//pTrans.localScale = new Vector3(1, 1, 1);
	}

	public void Turn(Vector3 target) {
		direction = target - transform.position;
		//direction.y = 0;
		//print (target.position + ", " + transform.position + ", " + direction);
		if(direction != Vector3.zero){
			transform.rotation = Quaternion.LookRotation(direction);
		}
	}
	public void Reached() {
		pAnim.enabled = false;
		bRend.mesh = meshF;
		bMeshR.material = matF;
	}
	public void Back() {
		pAnim.enabled = true;
		bRend.mesh = meshD;
		bMeshR.material = matD;
	}
	public void Activated() {
		boundary.SetActive(false);
		ots.Initiate(1);
		ots.enabled = true;
		pCollider.enabled = true;
	}
}
