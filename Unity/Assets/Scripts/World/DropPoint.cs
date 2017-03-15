using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DropPoint : MonoBehaviour, IDroppable
{

	#region Behaviours

	private void Awake()
	{
		
	}
	
	private void Start() 
	{
		
	}	
	
	private void Update() 
	{
		
	}
	
	#endregion
	
	
	#region Public Methods
    
    public bool CanDrop()
    {
        return true;
    }

    public void OnDrop()
    {
        Game.Win();
    }
	
	#endregion



}
