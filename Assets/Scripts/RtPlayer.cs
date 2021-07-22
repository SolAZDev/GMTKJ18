using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RtPlayer : MonoBehaviour {
	public GUI GameCanvas;
	public float Speed = 12, RotationRatio = .8f;
	public Transform rig;
	Rigidbody2D rigid;
	Transform cam;
	public Vector2 moveDir;
	public Vector3 CamForward;
	bool GotKey = false;
	RTSwitch tSwitch;
	public Animator anim, cAnim;
	public ParticleSystem effect;
	public bool Rotating = false, Accelerating = false, Moving = false;
	public LevelRotator Level;
	Vector3 mDir = Vector3.zero;
	// Use this for initialization
	void Start () {
		rigid = GetComponent<Rigidbody2D> ();
		cam = Camera.main.transform;
		Level = GameObject.FindObjectOfType<LevelRotator> ();
		effect.Play ();
		if (GameObject.FindObjectOfType<GUI> () == null) {
			Instantiate (GameCanvas.gameObject);
		}
	}

	// Update is called once per frame
	void Update () {
		//transform.rotation = Quaternion.Euler (0, 0, Mathf.Atan2 (Physics2D.gravity.x, -Physics2D.gravity.y));
		//transform.rotation = Quaternion.Euler (0, 0, -Level.GravityAngle);
		//print ("2D.Gravity's Angles(Asin) " + Mathf.Asin (Physics2D.gravity.y) + " (Acos) " + Mathf.Acos (Physics2D.gravity.x) + " VS Level.GravityAngle" + Level.GravityAngle);
		//cam.rotation = Quaternion.Euler (cam.rotation.eulerAngles.x, cam.rotation.eulerAngles.y, Level.GravityAngle);
		CamForward = Vector3.Scale (cam.forward, new Vector3 (1, 0, 1)).normalized;
#if UNITY_PLAYER || UNITY_STANDALONE || UNITY_WEBPLAYER
		MoveJoy (new Vector2 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical")));
		if (Input.GetKeyDown (KeyCode.W)) { ActivateSwitch (); }
		if (Input.GetKeyDown (KeyCode.Backspace)) { RestartLevel (); }
		if (Input.GetKeyDown (KeyCode.Return)) { GoToLevel ("Stage0"); }
		Accelerating = (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift)) ? true : false;
		if (Input.GetKeyDown (KeyCode.E)) {
			Rotating = true;
			Level.RotateToSide (-RotationRatio);
		}
		if (Input.GetKeyDown (KeyCode.Q)) {
			Rotating = true;
			Level.RotateToSide (RotationRatio);
		}
		if (Input.GetKeyUp (KeyCode.E) || Input.GetKeyUp (KeyCode.Q)) {
			Level.RotateToSide (0);
			Rotating = false;
		}
		if (Input.GetKeyDown (KeyCode.Tab)) { ActivateZoom (); }
#endif

		var em = effect.emission;
		em.enabled = (rigid.velocity.magnitude >.05f && !Rotating) ? true : false;

		rig.localRotation = Quaternion.Slerp (rig.localRotation, Quaternion.Euler (0, 180 + (45 * -moveDir.x), 0), Time.deltaTime * 4);
		/*if (rigid.velocity.x > 0) { rig.localRotation = Quaternion.Slerp (rig.localRotation, Quaternion.Euler (0, 180 - 45, 0), Time.deltaTime);; }
		if (rigid.velocity.x < 0) { rig.localRotation = Quaternion.Slerp (rig.localRotation, Quaternion.Euler (0, 180 + 45, 0), Time.deltaTime); }
		if (rigid.velocity.x == 0) { rig.localRotation = Quaternion.Slerp (rig.localRotation, Quaternion.Euler (0, 180, 0), Time.deltaTime); }*/

		cAnim.SetBool ("Moving", Moving);
		if (moveDir.magnitude > 0) {
			Moving = true;
		}

		//print ("Rigid:" + rigid.velocity.magnitude + "\nmvDir:" + moveDir.magnitude);
	}

	private void FixedUpdate () {
		print (moveDir + "(" + (moveDir.magnitude > 0) + ")::" + mDir + "::" + rigid.velocity);
		//Actual Movements
		//if (moveDir.magnitude > 0) {
		anim.SetBool ("MoveLR", moveDir.magnitude > 0);
		mDir = moveDir * (Accelerating ? Speed * 2 : Speed);
		mDir.y = (rigid.velocity.y > 0 ? 0 : rigid.velocity.y);
		rigid.velocity = mDir;

		//transform.position = Vector3.MoveTowards (transform.position, transform.position + mDir, Time.deltaTime * Speed);

		//}

	}
	public void MoveJoy (Vector2 Joy) {
		moveDir = Joy.y * CamForward + Joy.x * cam.right;
	}

	private void OnTriggerEnter2D (Collider2D other) {
		if (other.tag == "Key") { other.gameObject.SetActive (false); GotKey = true; }
		if (other.tag == "Door" && GotKey) {
			print ("Level Finished");
			SceneManager.LoadScene ("Stage0");

		}
		if (other.tag == "Spikes") {
			RestartLevel ();
		}
		if (other.tag == "Switch") {
			tSwitch = other.GetComponent<RTSwitch> ();
			if (Input.GetKey (KeyCode.W)) {
				tSwitch.ActivateSwitch ();
			}
		}
	}

	private void OnTriggerExit2D (Collider2D other) {
		if (other.tag == "Switch") { tSwitch = null; }
	}
	/*void OnCollisionEnter2D (Collision2D other) {
		if (other.gameObject.name == "Grid") {
			rigid.isKinematic = true;
		}
	}
	void OnCollisionExit2D (Collision2D other) {
		if (other.gameObject.name == "Grid") {
			rigid.isKinematic = false;
		}
	}*/

	public void RestartLevel () {
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);

	}
	public void GoToLevel (string level) {
		SceneManager.LoadScene (level);
	}

	public void ActivateZoom () {
		Moving = false;
	}
	public void DectivateZoom () {
		Moving = true;
	}
	public void ActivateSwitch () {
		if (tSwitch != null) { tSwitch.ActivateSwitch (); }
	}
	void OnGUI () {
		//GUI.Label (new Rect (25, 25, 200, 76), "<b>BackSpc</b> - Restart\n<b>Return</b> - Exit Level\n<b>W</b> - Activate");
	}
}