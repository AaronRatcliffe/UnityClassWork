using UnityEngine;
using System.Collections;

public class LevelButtons : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void LoadLevel(GameObject Puzzle){
        Game.NextPuzzle = Puzzle;
		Application.LoadLevel ("Game");
	}
}
