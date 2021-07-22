using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class RTSwitch : MonoBehaviour {
	public bool Deactivate = false;
	public float DeactivateTime = 10;
	public UnityEvent OnActivate, OnDeactivate;
	AudioSource click;
	bool Usable = true;
	// Use this for initialization
	void Start () {
		click = GetComponent<AudioSource> ();
	}

	// Update is called once per frame
	void Update () { }

	public void ActivateSwitch () {
		if (!Usable) { return; }
		Usable = false;
		OnActivate.Invoke ();
		click.Play ();
		transform.GetChild (0).transform.localRotation = Quaternion.Euler (0, 180, 180);
		if (Deactivate) { StartCoroutine (Timer ()); }
	}

	public IEnumerator Timer () {
		yield return new WaitForSeconds (DeactivateTime);
		OnDeactivate.Invoke ();
		click.Play ();
		transform.GetChild (0).transform.localRotation = Quaternion.Euler (0, 180, 0);
		Usable = true;
	}
}