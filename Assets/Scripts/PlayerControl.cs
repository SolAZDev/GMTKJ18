using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent (typeof (Rigidbody))]
public class PlayerControl : MonoBehaviour {
	public float Speed = 12;
	public
	Rigidbody rigid;
	Transform cam;
	Vector3 moveDir, CamForward;

	// Use this for initialization
	void Start () {
		rigid = GetComponent<Rigidbody> ();
		cam = Camera.main.transform;
	}

	// Update is called once per frame
	void FixedUpdate () {
		//Initial Movement.
		CamForward = Vector3.Scale (cam.forward, new Vector3 (1, 0, 1)).normalized;
		if (moveDir.magnitude >= 0) {
			rigid.velocity = moveDir * Speed;
		}
#if UNITY_EDITOR || UNITY_WEBGL || UNITY_STANDALONE
		MoveJoy (new Vector2 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical")));
		//rigid.AddRelativeTorque (new Vector3 (Random.Range (.01f, .1f), Random.Range (.01f, .1f), Random.Range (.01f, .1f)));
#endif
	}
	public void MoveJoy (Vector2 Joy) {
		moveDir = Joy.y * CamForward + Joy.x * cam.right;
	}

	private void OnCollisionEnter (Collision other) {
		if (other.transform.tag == "crashable") {
			float rnd = Random.Range (8, 15);
			Vector3 hit = (other.contacts[0].normal + Vector3.up) * rnd;
			print (other.contacts[0].point + " vs " + other.contacts[0].normal + " >> " + hit);
			other.transform.GetComponent<Rigidbody> ().AddExplosionForce (rnd, other.contacts[0].point, 1, Random.Range (.1f, 1f), ForceMode.Impulse);
		}
	}
}