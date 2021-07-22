using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelRotator : MonoBehaviour {
	// Use this for initialization
	public float RotateSetting = 15;
	public bool UseRigidbody = true;
	Rigidbody2D rigid;
	public AudioSource src;
	float RotateSide = 0;
	RtPlayer Player;
	public float GravityAngle = 0, inDir = 0;
	void Start () {
		rigid = GetComponent<Rigidbody2D> ();
		if (src != null) {
			src.Play ();
			src.Pause ();
		}
		Player = GameObject.FindWithTag ("Player").GetComponent<RtPlayer> ();
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (Player != null) {
			//RIGIGIDBODY KNOWN TO STUTTER
			if (Player.Rotating) {
				if (UseRigidbody) {
					rigid.MoveRotation (rigid.rotation + (Input.GetKey (KeyCode.LeftShift) ? RotateSide * RotateSetting * 1.5f : RotateSide * RotateSetting) * Time.deltaTime);
					//P/hysics2D.gravity = new Vector2 (Physics2D.gravity.x + ((Player.Accelerating) ? RotateSide * RotateSetting * 1.5f : RotateSide * RotateSetting), Physics2D.gravity.y);
					//inDir += RotateSide;
					//GravityAngle += inDir * (Player.Accelerating ? 1.5f : 1f) * Time.deltaTime;
					//Physics2D.gravity = RotateGravity (GravityAngle);
					//Physics2D.gravity = new Vector2 (Mathf.Cos (GravityAngle), Mathf.Sin (GravityAngle));
				} else {
					transform.rotation = Quaternion.Slerp (transform.rotation, transform.rotation * Quaternion.Euler (0, 0, (Player.Accelerating ? RotateSide * RotateSetting * 1.5f : RotateSide * RotateSetting)), Time.deltaTime);
				}
				if (src != null) {
					if (!src.isPlaying) { src.Play (); }
				}
			} else {
				if (src != null) {
					if (src.isPlaying) { src.Stop (); }
				}
			}
		}
		//Old Code
		/*if (Input.GetKey (KeyCode.Q)) {
			rigid.MoveRotation (rigid.rotation + (Input.GetKey (KeyCode.LeftShift) ? RotateSetting * 1.5f : RotateSetting) * Time.deltaTime);
			//transform.rotation = Quaternion.Slerp (transform.rotation, transform.rotation * Quaternion.Euler (0, 0, (Input.GetKey (KeyCode.LeftShift) ? RotateSetting * 1.5f : RotateSetting)), Time.deltaTime);
			if (!src.isPlaying) { src.Play (); }
		} else
		if (Input.GetKey (KeyCode.E)) {
			rigid.MoveRotation (rigid.rotation + -(Input.GetKey (KeyCode.LeftShift) ? RotateSetting * 1.5f : RotateSetting) * Time.deltaTime);
			//transform.rotation = Quaternion.Slerp (transform.rotation, transform.rotation * Quaternion.Euler (0, 0, (Input.GetKey (KeyCode.LeftShift) ? -RotateSetting * 1.5f : -RotateSetting)), Time.deltaTime);
			if (!src.isPlaying) { src.Play (); }
		} else {
			if (src.isPlaying) { src.Pause (); }
		}*/
	}

	public void RotateToSide (float RotateTo = 0) {
		RotateSide = RotateTo;
	}

	Vector2 RotateGravity (float angle) { return new Vector2 (Mathf.Cos (angle), Mathf.Sin (angle)); }

	Vector2 RotateGravity2 (float angle) { return Quaternion.Euler (0, 0, angle) * Physics2D.gravity; }

}