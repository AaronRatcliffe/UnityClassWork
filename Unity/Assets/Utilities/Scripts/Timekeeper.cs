using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Timekeeper : MonoBehaviour
{
    
	#region Fields

    private static HashSet<TimeFrames> framesEnabled;
    
    private static float lastTime;
    private static float realDeltaTime;

    private static readonly Dictionary<TimeModes, HashSet<TimeFrames>> modeFrames = new Dictionary<TimeModes, HashSet<TimeFrames>>()
    {
        {TimeModes.Normal, new HashSet<TimeFrames> {
            TimeFrames.Any,
            TimeFrames.Native,
            TimeFrames.Main, 
            TimeFrames.Camera
        }},
        {TimeModes.Paused, new HashSet<TimeFrames> {
            TimeFrames.Any,
            TimeFrames.Main, 
            TimeFrames.Camera
        }}
    };

	#endregion
	
	
	#region Constants
	

	
	#endregion
	
	
	#region Properties
       
    public static float DeltaTimeMain
    {
        get
        {
            return DeltaTime(TimeFrames.Main);
        }
    }
	
	#endregion
	

	#region Behaviours
    
    private void Awake()
    {
        SetTimeMode(TimeModes.Normal);
    }

    private void Update()
    {
        realDeltaTime = Time.realtimeSinceStartup - lastTime;
        lastTime = Time.realtimeSinceStartup;

        if (realDeltaTime > .5f) realDeltaTime = .00001f;
    }
	
	#endregion
	
	
	#region Public Methods
        
    public static float DeltaTime(TimeFrames timeFrame)
    {
        return framesEnabled.Contains(timeFrame) ? realDeltaTime : 0f;
    }

    public static void SetTimeMode(TimeModes mode)
    {
        framesEnabled = modeFrames[mode];
        Time.timeScale = (framesEnabled.Contains(TimeFrames.Native)) ? 1f : 0f;
    }

    public static bool IsPaused(TimeFrames timeFrame)
    {
        return !framesEnabled.Contains(timeFrame);
    }
	
	#endregion
	
	
	#region Private Methods

	
	#endregion
	
}


public enum TimeFrames
{
    Any,
    Native,
    Main,
    Camera,
}

public enum TimeModes
{
    Normal,
    Paused,
}