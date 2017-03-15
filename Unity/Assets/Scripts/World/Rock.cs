using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Rock : MonoBehaviour, IGrabbable
{

    #region Fields

    private Collider2D col;

    #endregion


    #region Behaviours

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        col.enabled = false;
        transform.parent = Game.World;
    }

    #endregion


    #region Public Methods

    public bool CanGrab()
    {
        return true;
    }
    
    public void OnGrab()
    {
        GameObject.Destroy(gameObject);
    }

    public void SetSolid(bool solid)
    {
        col.enabled = solid;
    }

    public void Restart()
    {
        GameObject.Destroy(gameObject);
    }

	#endregion
	

}
