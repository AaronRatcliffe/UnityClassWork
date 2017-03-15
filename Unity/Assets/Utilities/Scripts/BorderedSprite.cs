using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BorderedSprite : MonoBehaviour 
{

	#region Fields

    public GameObject topBorder;
    public GameObject topRightBorder;
	
	#endregion
	
	
	#region Constants
	
	
	
	#endregion
	
	
	#region Properties
	
	
	
	#endregion
	

	#region Behaviours

	private void Awake()
	{
		
	}
	
	private void Start() 
	{
        ApplyBorders();
        Destroy(this);
	}	
	
	private void Update() 
	{
		
	}
	
	#endregion
	
	
	#region Public Methods
	
	
	
	#endregion
	
	
	#region Private MethodsF

    private void ApplyBorders()
    {
        int layer = 1 << gameObject.layer;

        GameObject borderGroup = new GameObject("Borders");
        borderGroup.SetParentObject(transform);
        borderGroup.transform.localPosition = Vector2.zero;

        bool up = !Physics2D.OverlapPointAll(transform.position2D() + Vector2.up, layer)
                               .Any(c => c.gameObject.name == name);
        bool down = !Physics2D.OverlapPointAll(transform.position2D() - Vector2.up, layer)
                               .Any(c => c.gameObject.name == name);
        bool left = !Physics2D.OverlapPointAll(transform.position2D() - Vector2.right, layer)
                               .Any(c => c.gameObject.name == name);
        bool right = !Physics2D.OverlapPointAll(transform.position2D() + Vector2.right, layer)
                               .Any(c => c.gameObject.name == name);

        bool upRight = up || right || !Physics2D.OverlapPointAll(transform.position2D() + Vector2.up + Vector2.right, layer)
                                                    .Any(c => c.gameObject.name == name);
        bool upLeft = up || left || !Physics2D.OverlapPointAll(transform.position2D() + Vector2.up - Vector2.right, layer)
                                                    .Any(c => c.gameObject.name == name);
        bool downRight = down || right || !Physics2D.OverlapPointAll(transform.position2D() - Vector2.up + Vector2.right, layer)
                                                    .Any(c => c.gameObject.name == name);
        bool downLeft = down || left || !Physics2D.OverlapPointAll(transform.position2D() - Vector2.up - Vector2.right, layer)
                                                    .Any(c => c.gameObject.name == name);

        if (up) GameObject2.Instantiate(topBorder, borderGroup.transform, Vector2.zero, Quaternion.identity);
        if (left) GameObject2.Instantiate(topBorder, borderGroup.transform, Vector2.zero, Quaternion.Euler(0, 0, 90));
        if (down) GameObject2.Instantiate(topBorder, borderGroup.transform, Vector2.zero, Quaternion.Euler(0, 0, 180));
        if (right) GameObject2.Instantiate(topBorder, borderGroup.transform, Vector2.zero, Quaternion.Euler(0, 0, 270));
        if (upRight) GameObject2.Instantiate(topRightBorder, borderGroup.transform, Vector2.zero, Quaternion.identity);
        if (upLeft) GameObject2.Instantiate(topRightBorder, borderGroup.transform, Vector2.zero, Quaternion.Euler(0, 0, 90));
        if (downLeft) GameObject2.Instantiate(topRightBorder, borderGroup.transform, Vector2.zero, Quaternion.Euler(0, 0, 180));
        if (downRight) GameObject2.Instantiate(topRightBorder, borderGroup.transform, Vector2.zero, Quaternion.Euler(0, 0, 270));
    }
	
	#endregion
	
}
