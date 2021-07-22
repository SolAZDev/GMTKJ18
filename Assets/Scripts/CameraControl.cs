using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {
	public Vector3 Offset, Velocity = Vector3.one;
	public float Speed = 8, WaitTime = 3;
	public RtPlayer Player;
	bool shown = false;

	// Use this for initialization
	IEnumerator Start () {
		yield return new WaitForSeconds (WaitTime);
		shown = true;
	}

	// Update is called once per frame
	void Update () {
		if (!shown && Player.moveDir.magnitude > 0) { shown = true; }
		if (shown) {
			transform.position = Vector3.Slerp (transform.position, Player.transform.position + Offset, Time.deltaTime * Speed);
			//transform.position = Vector3.SmoothDamp (transform.position, Player.transform.position + Offset, ref Velocity, Speed * Time.deltaTime, Speed * 2);
		}
	}
}