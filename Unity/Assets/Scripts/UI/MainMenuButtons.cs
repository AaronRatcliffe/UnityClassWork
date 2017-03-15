using UnityEngine;
using System.Collections;

public class MainMenuButtons : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void LoadLevelSelection(){
		Application.LoadLevel ("LevelSelect");
	}

	public void Exit(){
		Application.Quit ();
	}
}
