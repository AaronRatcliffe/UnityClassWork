using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FPSLogger : MonoBehaviour 
{

	#region Fields

    private float updateRate;
    private float fixedUpdateRate;

    private float prevUpdateTime;
    private float prevFixedUpdateTime;

	#endregion
	
	
	#region Constants
	
	
	
	#endregion
	
	
	#region Properties
	
	
	
	#endregion
	

	#region Behaviours

	private void Awake()
	{
        prevUpdateTime = prevFixedUpdateTime = Time.realtimeSinceStartup;
	}
	
	private void Update() 
	{
        float time = Time.realtimeSinceStartup;
        float delta = time - prevUpdateTime;

        updateRate = 1 / delta;
        prevUpdateTime = time;
	}

    private void FixedUpdate()
    {
        float time = Time.realtimeSinceStartup;
        float delta = time - prevFixedUpdateTime;

        fixedUpdateRate = 1 / delta;
        prevFixedUpdateTime = time;
    }

    private void OnGUI()
    {

        GUI.Box(new Rect(Screen.width - 100, 0, 100, 40), "");
        GUI.Label(new Rect(Screen.width - 90, 0, 90, 20), "Upd: " + updateRate.ToString());
        GUI.Label(new Rect(Screen.width - 90, 20, 90, 20), "Fix: " + fixedUpdateRate.ToString());
    }
	
	#endregion
	
	
	#region Public Methods
	
	
	
	#endregion
	
	
	#region Private Methods
	
	
	
	#endregion
	
}
