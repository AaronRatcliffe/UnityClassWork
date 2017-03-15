using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SimpleRotator : MonoBehaviour 
{

	#region Fields

    public float DegreesPerSecond;
	
	#endregion
	
	
	#region Behaviours

	private void Update() 
	{
        transform.Rotate2D(DegreesPerSecond * Timekeeper.DeltaTimeMain);
	}
	
	#endregion
	
}
