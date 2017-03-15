using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TogglePattern : MonoBehaviour 
{

	#region Fields

    public bool toggled = false;
    public int onSteps = 1;
    public int offSteps = 1;

    private int stepCount;
    private bool startToggled;

	#endregion
	

	#region Behaviours

	private void Start()
	{
        SendMessage("SetToggle", toggled);
        startToggled = toggled;
	}

    private void Step()
    {
        stepCount++;

        if (stepCount >= ((toggled) ? onSteps : offSteps))
        {
            toggled = !toggled;
            stepCount=0;
            SendMessage("SetToggle", toggled);
        }
    }

    private void Restart()
    {
        stepCount = 0;
        toggled = startToggled;
        Debug.Log("test");
    }
	
	#endregion
	
}
