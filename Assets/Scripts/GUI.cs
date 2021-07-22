using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI : MonoBehaviour {
	RtPlayer player;
	LevelRotator level;
	public GameObject TextView, MobileView;
	// Use this for initialization
	void Start () {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
		TextView.SetActive (true);
		MobileView.SetActive (false);
#elif UNITY_ANDROID || UNITY_TIZEN || UNITY_IOS
		TextView.SetActive (false);
		MobileView.SetActive (true);
#endif

		player = GameObject.FindObjectOfType<RtPlayer> ();
		level = GameObject.FindObjectOfType<LevelRotator> ();
	}

	// Update is called once per frame
	void Update () {

	}

	public void MovePlayer (float side) {
		player.MoveJoy (new Vector2 (side, 0));
	}
	public void Restart () {
		player.RestartLevel ();
	}

	public void TriggerAcceleration (bool acc) {
		player.Accelerating = acc;
	}

	public void ZoomOn () {
		player.ActivateZoom ();
	}
	public void ZoomOff () {
		player.DectivateZoom ();
	}

	public void ReturnToStageSelect () {
		player.GoToLevel ("Stage0");
	}

	public void RestartLevel () {
		player.RestartLevel ();
	}

	public void RotateLevel (float rot) {
		player.Rotating = (rot != 0 ? true : false);
		level.RotateToSide (player.RotationRatio * -rot);
	}
	public void ActivateSwitch () {
		player.ActivateSwitch ();
	}
}