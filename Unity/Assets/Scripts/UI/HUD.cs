using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HUD : MonoBehaviour 
{

	#region Fields

    private GameObject AddCover;
    private GameObject CommandCover;
    private GameObject SpeedCover;
	
	#endregion
	
	
	#region Constants
	
	
	
	#endregion
	
	
	#region Properties
	
	
	
	#endregion
	

	#region Behaviours

	private void Awake()
	{
        AddCover = GameObject.Find("AddCover");
        CommandCover = GameObject.Find("CommandCover");
        SpeedCover = GameObject.Find("SpeedCover");
	}
	
	private void Start() 
	{
		
	}	
	
	private void Update() 
	{
        if (StepManager.canEditCommands)
        {
            CommandCover.SetActive(false);

            if (Game.CurrentRobot.instructionSet.Instructions.Count < 16)
            {
                AddCover.SetActive(false);
            }
            else
            {
                AddCover.SetActive(true);
            }
        }
        else
        {
            AddCover.SetActive(true);
            CommandCover.SetActive(true);
        }

        SpeedCover.SetActive(Game.IsGameOver);
	}
	
	#endregion
	
	
	#region Public Methods
	
	
	
	#endregion
	
	
	#region Private Methods
	
	
	
	#endregion
	
}
