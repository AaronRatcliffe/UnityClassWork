using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StockPile : MonoBehaviour, IGrabbable, IDroppable
{

	#region Fields
	
	
	
	#endregion

	#region Behaviours

	private void Awake()
	{
		
	}
	
	private void Start() 
	{
		
	}	
	
	#endregion
	
	
	#region Public Methods

    public bool CanGrab()
    {
        return true;
    }

    public void OnGrab()
    {
        //Do nothing
    }

    public bool CanDrop()
    {
        return true;
    }

    public void OnDrop()
    {
        //do nothing
    }
	
	#endregion
}
