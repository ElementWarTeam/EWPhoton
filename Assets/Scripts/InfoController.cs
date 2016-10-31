using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class InfoController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void BackToLauncherScene() {
		SceneManager.LoadScene ("Launcher");
	}
}
