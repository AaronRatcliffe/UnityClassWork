using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SimpleColorCycle : MonoBehaviour 
{

	#region Fields

    public float cycleSpeed = 1;
    public float T;
    public float Brightness = 1f;
    public float Ambiance = 0;

    private SpriteRenderer spriteRenderer;
	
	#endregion
		

	#region Behaviours
    	
	private void Start() 
	{
        spriteRenderer = GetComponent<SpriteRenderer>();
	}	
	
	private void Update() 
	{
        T += cycleSpeed * Time.deltaTime;
        T %= 3f;

        float r = Mathf.Clamp01(Mathf.Abs(T - 1.5f) - 0.5f);
        float g = Mathf.Clamp01(1 - Mathf.Abs(T - 1));
        float b = Mathf.Clamp01(1 - Mathf.Abs(T - 2));

        spriteRenderer.color = (Color.white*Ambiance + new Color(r, g, b)) * Brightness;
	}
	
	#endregion
	
}
