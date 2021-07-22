using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelect : MonoBehaviour {
	public void GoToScene (string Level) {
		SceneManager.LoadScene (Level);
	}
}