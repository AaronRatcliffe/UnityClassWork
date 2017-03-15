using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Conveyor : MonoBehaviour, IDroppable, IGrabbable
{

	#region Fields

    public bool toggledOn;

    private bool hasRock;
    private Transform Rock;
    private Animator animator;
	
	#endregion
	
	
	#region Constants
	
	
	
	#endregion
	
	
	#region Properties
	
	
	
	#endregion
	

	#region Behaviours

	private void Awake()
	{
        animator = GetComponent<Animator>();
        animator.SetBool("On", false);
	}
	
	#endregion
	
	
	#region Public Methods
    
    public bool CanDrop()
    {
        return !hasRock;
    }

    public void OnDrop()
    {
        hasRock = true;
        Rock = (GameObject.Instantiate(Game.Current.RockPrefab, transform.position, Quaternion.identity) as GameObject).transform;        
    }

    public bool CanGrab()
    {
        return hasRock && !toggledOn;
    }

    public void OnGrab()
    {
        GameObject.Destroy(Rock.gameObject);
    }

    public void Step()
    {
        if (toggledOn && hasRock)
        {
            hasRock = false;
            Vector2 nextPosition = transform.position + transform.up;
            Rock.transform.position = nextPosition;

            Collider2D collide = Physics2D.OverlapPoint(nextPosition);
            if (collide == null)
            {
                Rock.SendMessage("SetSolid", true);
            }
            else
            {
                IDroppable droppable = collide.GetComponent(typeof(IDroppable)) as IDroppable;
                if (droppable != null && droppable.CanDrop())
                {
                    StepManager.OnEndOfStep += () => DropLate(droppable);
                }
                else
                {
                    Game.Fail();
                }
            }
        }
    }

    public void StepProgress(float progress)
    {
        if (toggledOn && hasRock)
        {
            Rock.transform.position = transform.position + transform.up * progress;
        }
    }

    public void SetToggle(bool toggled)
    {
        toggledOn = toggled;
        animator.SetBool("On", toggledOn);
    }

    public void Play()
    {
        animator.SetBool("On", toggledOn);
    }

    public void Pause()
    {
        animator.SetBool("On", false);
    }

    public void Stop()
    {
        animator.SetBool("On", false);
    }

	#endregion
	
	
	#region Private Methods

    private void DropLate(IDroppable droppable)
    {
        GameObject.Destroy(Rock.gameObject);
        droppable.OnDrop();
    }
	
	#endregion


}
