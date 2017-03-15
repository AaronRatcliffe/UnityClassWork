using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProximityGrid  
{

	#region Fields

    private List<GameObject> xList;
    private List<GameObject> yList;

    private float shortestDistance;
	
	#endregion
	

	#region Constructors

    public ProximityGrid(IEnumerable<GameObject> gameObjects, float shortestDistance)
    {
        xList = gameObjects.OrderBy(g => g.transform.position.x).ToList();
        yList = gameObjects.OrderBy(g => g.transform.position.y).ToList();

        this.shortestDistance = shortestDistance;
    }
	
	#endregion
		
	
	#region Public Methods

    public GameObject[] FindNearby(Vector2 point, float distance)
    {
        var nearX = xList.SkipWhile(g => g.transform.position.x < point.x - distance)
                         .TakeWhile(g => g.transform.position.x < point.x + distance);

        var nearY = yList.SkipWhile(g => g.transform.position.y < point.y - distance)
                         .TakeWhile(g => g.transform.position.y < point.y + distance);
        
        return nearX.Intersect(nearY).ToArray();
    }

    public GameObject GetNearest(Vector2 point)
    {
        var nearby = FindNearby(point, shortestDistance);

        return nearby.MinBy(g => (g.transform.position2D() - point).sqrMagnitude);
    }
	
	#endregion
	
}
