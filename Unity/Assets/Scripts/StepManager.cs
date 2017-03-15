using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class StepManager : MonoBehaviour 
{

	#region Fields

    public static Action OnEndOfStep;

    public float StepDuration = .5f;
    public bool isPlaying = false;
    public static bool canEditCommands;

    private static StepManager Current;

    private float StepTimer;
    private int stepCount;
	
	#endregion
	
	
	#region Constants
	
	
	
	#endregion
	
	
	#region Properties
	
	
	
	#endregion
	

	#region Behaviours

	private void Awake()
	{
        Current = this;
	}
	
	private void Start() 
	{
        stop();
	}	
	
	private void Update() 
	{
        if (!isPlaying) return;

        StepTimer += Timekeeper.DeltaTimeMain;
        while (StepTimer >= StepDuration)
        {
            stepCount++;
            BroadcastMessage("StepProgress", 1);
            BroadcastMessage("Step");
            if (OnEndOfStep != null)
            {
                OnEndOfStep();
                OnEndOfStep = null;
            }
            StepTimer -= StepDuration;
        }

        BroadcastMessage("StepProgress", StepTimer / StepDuration);
	}
	
	#endregion
	
	
	#region Public Methods

    public void play()
    {
        if (isPlaying) return;

        Current.isPlaying = true;

        if (Current.stepCount ==0 && Current.StepTimer==0)
            Current.BroadcastMessage("Restart");
        Current.BroadcastMessage("Play");
        Time.timeScale = 1 / StepDuration;
        canEditCommands = false;
    }

    public void pause()
    {
        if (!isPlaying) return;

        Current.isPlaying = false;
        Current.BroadcastMessage("Pause");
        Time.timeScale = 1;
        canEditCommands = false;
    }

    public void stop()
    {
        Game.IsGameOver = false;
        GameObject.Find("Popup").GetComponent<Text>().text = "";
        Current.isPlaying = false;
        Current.BroadcastMessage("Restart");
        Current.stepCount = 0;
        Current.StepTimer = 0;
        Time.timeScale = 1;
        canEditCommands = true;
    }

    public static void Pause()
    {
        Current.pause();
    }

    public void changeSpeed(float f)
    {
        StepDuration = f + .25f;
        Time.timeScale = 1 / StepDuration;
    }

	#endregion
	
	
	#region Private Methods
	
	#endregion
	
}
