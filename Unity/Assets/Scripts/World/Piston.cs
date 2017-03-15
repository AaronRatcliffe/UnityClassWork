using UnityEngine;
using System.Collections;

public class Piston : MonoBehaviour 
{

    private Collider2D col;
    private Animator animator;

    public bool toggledOff;

	void Awake() 
    {
		col = GetComponent<Collider2D> ();	//Changed ColliderCheck to Collider2D
        animator = GetComponent<Animator>();
        animator.SetBool("On", !toggledOff);
        animator.SetBool("Paused", true);
	}

	void SetToggle(bool isToggled){
        if (isToggled)
        {
            col.enabled = false;
        }
        else
        {
            col.enabled = true;
        }

        toggledOff = isToggled;
        animator.SetBool("On", !toggledOff);
	}

    private void Play()
    {
        animator.SetBool("On", !toggledOff);
        animator.SetBool("Paused", false);
    }

    private void Restart()
    {
        animator.SetBool("On", !toggledOff);
        animator.SetBool("Paused", true);
    }

    private void Pause()
    {
        animator.SetBool("On", !toggledOff);
        animator.SetBool("Paused", true);
    }
}
