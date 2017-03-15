using UnityEngine;
using System.Collections;

public static class InputUtilities 
{
    public static Vector2 MouseWorldPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
