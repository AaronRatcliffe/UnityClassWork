using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(ColliderCheck))]
public class DeadlyBumper : MonoBehaviour 
{

	#region Fields

    private ColliderCheck colliderCheck;
	
	#endregion
	
	
	#region Constants
	
	
	
	#endregion
	
	
	#region Properties
	
	
	
	#endregion
	

	#region Behaviours

	private void Awake()
	{
        colliderCheck = GetComponent<ColliderCheck>();	
	}
	
	private void Start() 
	{
		
	}	
	
	private void Update() 
	{
		
	}

    private void StepProgress(float progress)
    {
        if (colliderCheck.IsColliding)
        {
            Game.Fail();
        }
    }

    private void Step()
    {
        if (colliderCheck.IsColliding)
        {
            Game.Fail();
        }
    }

	#endregion
	
	
	#region Public Methods
	
	
	
	#endregion
	
	
	#region Private Methods
	
	
	
	#endregion
	
}
