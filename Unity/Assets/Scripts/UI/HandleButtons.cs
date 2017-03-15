using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

enum Directions{
	Left,
	Up,
	Right,
	Cycle
}

[System.Serializable]
public class ButtonAction {
	string type;
	int value;		//Directions enumerator for drop/grab/turn, ints for grab/loop
}

public class HandleButtons : MonoBehaviour {
	public GameObject moveButton;
	public GameObject loopButton;
	public GameObject dropButton;
	public GameObject grabButton;
	public GameObject turnButton;
	public GameObject syncButton;

	// Use this for initialization
	void Start () {
		//Setup buttons that just add a new button onto the action list
		buttonSetup (moveButton.GetComponent<UnityEngine.UI.Button>());
		buttonSetup (loopButton.GetComponent<UnityEngine.UI.Button>());
		buttonSetup (dropButton.GetComponent<UnityEngine.UI.Button>());
		buttonSetup (grabButton.GetComponent<UnityEngine.UI.Button>());
		buttonSetup (turnButton.GetComponent<UnityEngine.UI.Button>());
		buttonSetup (syncButton.GetComponent<UnityEngine.UI.Button>());

		//Setup buttons for when player clicks on delete for a button

		//Setup actions for when player modifies content of action list
			//clicks arrows for grab/drop/turn
			//changes value for move/loop

	}

	void buttonSetup(UnityEngine.UI.Button button) {
		button.onClick.RemoveAllListeners();
		//Add your new event
		button.onClick.AddListener(() => handleButton(button));
	}
	
	void handleButton(UnityEngine.UI.Button b) {
		string btnName = b.name.ToLower ();
		if (btnName.Contains ("move")) {
			//Add a new move item to an internal array of actions robot one is defined to do
			//Instantiate a new "Move Spaces List Item" and set its parent to the Scroll View > Content Panel
            Game.CurrentRobot.instructionSet.Instructions.Add(new Instruction(InstructionType.Move,1));

		}else if(btnName.Contains ("loop")) {
			
		}else if(btnName.Contains ("drop")) {
			
		}else if(btnName.Contains ("grab")) {
			
		}else if(btnName.Contains ("turn")) {
			
		}else if(btnName.Contains ("sync")) {
			
		}
	}

	// Update is called once per frame
	void Update () {
		//Make sure internal array of actions for robot one matches 	
	}
}
