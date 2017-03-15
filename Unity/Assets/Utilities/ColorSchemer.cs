using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(SpriteRenderer))]
public class ColorSchemer : MonoBehaviour 
{

	#region Fields

    public ColorScheme Scheme;

    private SpriteRenderer spriteRenderer;
	
	#endregion
	
	
	#region Constants
	
	
	
	#endregion
	
	
	#region Properties
	
	
	
	#endregion
	

	#region Behaviours

	private void Awake()
	{
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (Application.isPlaying)
        {
            spriteRenderer.color = Scheme.Color;
            Destroy(this);
        }
	}

    private void OnRenderObject()
    {
        if (Scheme != null)
        {
            spriteRenderer.color = Scheme.Color;
        }
    }

	#endregion
	
	
	#region Public Methods
	
	
	
	#endregion
	
	
	#region Private Methods
	
	
	
	#endregion
	
}
