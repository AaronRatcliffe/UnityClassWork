using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Runtime.Serialization;
using System.Reflection;

[System.Diagnostics.DebuggerNonUserCode]
public static class UnityExtensions 
{
    	
	#region Public Methods

    #region SetPosition2D

    public static void SetPosition2D(this GameObject gameObject, Vector2 position)
    {
        gameObject.transform.position = position;
    }

    public static void SetPosition2D(this GameObject gameObject, float x, float y)
    {
        gameObject.SetPosition2D(new Vector2(x, y));
    }
    
    #endregion
    
    #region SetParent

    public static void SetParentObject(this GameObject gameObject, GameObject parent, bool applyParentScale)
    {
        if (applyParentScale)
        {
            Vector3 parentScale = parent.transform.lossyScale;
            parentScale.z = 1f;
            parent.SetWorldScale(Vector3.one);

            gameObject.transform.parent = parent.transform;

            parent.SetWorldScale(parentScale);
        }
        else
        {
            gameObject.transform.parent = parent.transform;

            Vector3 scale = gameObject.transform.localScale;
            scale.z = 1;
            gameObject.transform.localScale = scale;
        }
    }

    public static void SetParentObject(this GameObject gameObject, GameObject parent)
    {
        gameObject.SetParentObject(parent, false);
    }

    public static void SetParentObject(this GameObject gameObject, Component parent, bool applyParentScale)
    {
        gameObject.SetParentObject(parent.gameObject, applyParentScale);
    }

    public static void SetParentObject(this GameObject gameObject, Component parent)
    {
        gameObject.SetParentObject(parent, false);
    }

    #endregion

    public static void SetScaleIgnoreChildren(this Transform source, Vector3 scale)
    {
        scale.z = 1f;
        Vector2 current = source.localScale;
        float inverseX = current.x / scale.x;
        float inverseY = current.y / scale.y;

        source.localScale = scale;

        foreach (Transform child in source)
        {
            Vector2 childScale = child.localScale;
            childScale.x *= inverseX;
            childScale.y *= inverseY;
            child.localScale = childScale;
        }
    }

    public static IEnumerable<Transform> GetSiblings(this Transform source)
    {
        return source.parent.GetChildren().Except(source);
    }

    public static void SetWorldScale(this GameObject gameObject, Vector3 worldScale)
    {
        Transform parent = gameObject.transform.parent;

        if (parent == null)
        {
            gameObject.transform.localScale = worldScale;
        }
        else
        {
            Vector3 parentScale = parent.lossyScale;
            parentScale.z = 1;
            parent.gameObject.SetWorldScale(parentScale);

            gameObject.transform.parent = null;
            gameObject.transform.localScale = worldScale;
            gameObject.transform.parent = parent;
        }
    }

    public static void SendMessagesToEach(this List<GameObject> gameObjects, string message)
    {
        gameObjects.SendMessagesToEach(message, SendMessageOptions.RequireReceiver);
    }

    public static void SendMessagesToEach(this List<GameObject> gameObjects, string message, SendMessageOptions options)
    {
        gameObjects.ForEach(g => g.SendMessage(message, options));
    }

    public static void SendMessagesToEach<T>(this List<T> gameObjects, string message) where T:Component
    {
        gameObjects.SendMessagesToEach(message, SendMessageOptions.RequireReceiver);
    }

    public static void SendMessagesToEach<T>(this List<T> components, string message, SendMessageOptions options) where T : Component
    {
        components.ForEach(c => c.SendMessage(message));
    }

    public static bool OverlapScreenPoint(this Collider2D collider, Vector2 point)
    {
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(point);
        return collider.OverlapPoint(worldPoint);
    }

    public static Vector2 ClampWithin(this Rect source, Rect bounds)
    {
        Vector2 offset = Vector2.zero;

        float rightOffset = bounds.xMax - source.xMax;
        float leftOffset = bounds.xMin - source.xMin;
        float bottomOffset = bounds.yMax - source.yMax;
        float topOffset = bounds.yMin - source.yMin;

        if (rightOffset < 0) offset.x = rightOffset;
        if (leftOffset > 0) offset.x = leftOffset;
        if (bottomOffset < 0) offset.y = bottomOffset;
        if (topOffset > 0) offset.y = topOffset;

        return offset;
    }

    public static Vector3[] Verticies(this Rect source)
    {
        return new Vector3[]
        {
            new Vector2(source.xMin, source.yMin),
            new Vector2(source.xMax, source.yMin),
            new Vector2(source.xMax, source.yMax),
            new Vector2(source.xMin, source.yMax),
        };
    }

    public static Sprite Clone(this Sprite source)
    {
        Texture2D sourceTexture = source.texture;

        Texture2D newTexture = new Texture2D(sourceTexture.width, sourceTexture.height, sourceTexture.format, false);
        newTexture.SetPixels(sourceTexture.GetPixels());
        
        Sprite newSprite = Sprite.Create(newTexture, source.rect, Vector2.one * .5f);
        return newSprite;
    }

    public static Touch[] TouchesWithin(this Collider2D collider2D)
    {
        return Input.touches.Where(t => collider2D.OverlapScreenPoint(t.position))
                     .ToArray();
    }

    public static bool TouchWithin(this Collider2D collider2D, out Touch touch)
    {
        Touch[] touches = collider2D.TouchesWithin();

        if (touches.Any())
        {
            touch = touches[0];
            return true;
        }
        else
        {
            touch = default(Touch);
            return false;
        }            
    }

    public static Vector2 GetWorldPosition(this Touch touch)
    {
        return Camera.main.ScreenToWorldPoint(touch.position);
    }

    public static Vector2 position2D(this Transform transform)
    {
        return transform.position;
    }

    public static Transform FindGrandchild(this Transform target, string name)
    {
        if (target.name == name) return target;

        foreach (Transform child in target)
        {
            var result = FindGrandchild(child, name);

            if (result != null) return result;
        }   

        return null;
    }

    public static IEnumerable<Transform> GetChildren(this Transform source)
    {
        var enumerator = source.GetEnumerator();
        while (enumerator.MoveNext())
        {
            yield return enumerator.Current as Transform;
        }
    }

    public static IEnumerable<T> FindChildrenComponents<T>(this Transform parent, string name) where T:Component
    {
        foreach (Transform child in parent)
        {
            if (child.name == name)
                foreach (T t in child.GetComponents<T>())
                {
                    yield return t;
                }
        }
    }

    public static T FindChildComponent<T>(this Transform transform, string childName) where T:Component
    {
        return transform.FindChild(childName).GetComponent<T>();
    }

    public static void Rotate2D(this Transform transform, float angles)
    {
        transform.Rotate(0, 0, angles);
    }

    public static float GetRightAngle(this Transform transform)
    {
        Vector2 right = transform.right;
        float degrees = Mathf.Atan2(right.y, right.x) * 180f/ Mathf.PI;
        return (degrees < 0f) ? 360f + degrees : degrees;
    }

    public static void SetRightAngle(this Transform transform, float angles)
    {
        transform.rotation = Quaternion.Euler(0, 0, angles);
    }

    public static void SetUpDirection(this Transform transform, Vector3 direction)
    {
        transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
    }

	public static Vector2 RotateTowards(this Vector2 current, Vector2 direction, float maxDegrees)
	{
        float sign;
        float angle = current.AngleTo(direction, out sign);

        if (angle <= maxDegrees)
        {
            return direction;
        }
        else
        {
            return Quaternion.Euler(0, 0, maxDegrees * sign) * current;
        }
	}

    public static float AngleToSigned(this Vector2 current, Vector2 direction)
    {
        float sign;
        float angle = current.AngleTo(direction, out sign);

        return angle * sign;
    }

    public static float AngleTo(this Vector2 current, Vector2 direction, out float sign)
    {
        sign = (Vector3.Cross(current, direction).z < 0) ? -1 : 1;
        return Vector2.Angle(current, direction);
    }


    public static Vector2 SnapOrthogonally(this Vector2 current, float snapInterval, Vector2 direction)
    {
        bool snapX = direction.x != 0;
        bool snapY = direction.y != 0;
        return current.Snap(snapInterval, snapX, snapY);
    }

    public static Vector2 Snap(this Vector2 current, float snapInterval)
    {
        return current.Snap(snapInterval, true, true);
    }

    public static Vector2 Snap(this Vector2 current, float snapInterval, bool snapX, bool snapY)
    {
        float x = Mathf.Round(current.x / snapInterval) * snapInterval;
        float y = Mathf.Round(current.y / snapInterval) * snapInterval;

        return new Vector2(x, y);
    }
    
    public static Vector2 RoundToCardinal(this Vector2 direction)
    {
        float magX = Mathf.Abs(direction.x);
        float magY = Mathf.Abs(direction.y);
        float signX = Math.Sign(direction.x);
        float signY = Math.Sign(direction.y);

        return (magX > magY) ? Vector2.right * signX : Vector2.up * signY;
    }

    public static Vector2 RoundToDiagonal(this Vector2 direction)
    {
        int x = (direction.x >= 0) ? 1 : -1;
        int y = (direction.y >= 0) ? 1 : -1;
        return new Vector2(x, y);
    }
    
    public static Vector3 RotateTowards2d(this Vector3 current, Vector2 direction, float maxDegrees)
    {
        return ((Vector2) current).RotateTowards(direction, maxDegrees);
    }

    public static void RotateUpTowards2d(this Transform transform, Vector2 direction, float maxDegrees, float satisfactionAngles)
    {
        if (Vector2.Angle(transform.up, direction) > satisfactionAngles)
            RotateUpTowards2d(transform, direction, maxDegrees);
    }

    public static void RotateUpTowards2d(this Transform transform, Vector2 direction, float maxDegrees)
    {
        transform.SetUpDirection(transform.up.RotateTowards2d(direction, maxDegrees));
    }
    
    public static bool Contains(this Bounds outerBounds, Bounds bounds)
    {
        return outerBounds.Contains(bounds.min) && outerBounds.Contains(bounds.max);
    }

    public static void CapSpeed(this Rigidbody2D rigidbody, float cap)
    {
        Vector2 velocity = rigidbody.velocity;
        float magnitude = MathUtilities.CapMagnitude(velocity.magnitude, cap);

        rigidbody.velocity = velocity.normalized * magnitude;
    }

    public static Vector2 RandomInnerPoint(this Bounds bounds)
    {
        Vector2 randExtent = bounds.extents;
        randExtent.x *= UnityEngine.Random.Range(-1f, 1f);
        randExtent.y *= UnityEngine.Random.Range(-1f, 1f);

        return (Vector2) bounds.center + randExtent;
    }

    public static bool ContainsLayer(this LayerMask mask, int layer)
    {
        return ((1 << layer) & mask) != 0;
    }

    public static Vector2 AsVector2(this CardinalDirections soruce)
    {
        switch (soruce)
        {
            case CardinalDirections.Up:
                return Vector2.up;
            case CardinalDirections.Down:
                return -Vector2.up;
            case CardinalDirections.Right:
                return Vector2.right;
            case CardinalDirections.Left:
                return -Vector2.right;
            default:
                throw new NotImplementedException();
        }
    }
	
	#endregion
		
}

[Serializable]
public struct Vector2Int
{
    public int x, y;

    public Vector2Int(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public static implicit operator Vector2(Vector2Int vector2Int)
    {
        return new Vector2(vector2Int.x, vector2Int.y);
    }

    public static implicit operator Vector3(Vector2Int vector2Int)
    {
        return new Vector3(vector2Int.x, vector2Int.y, 0f);
    }

    public static explicit operator Vector2Int(Vector2 vector2)
    {
        return new Vector2Int((int) vector2.x, (int) vector2.y);
    }

    public static Vector2Int operator +(Vector2Int a, Vector2Int b)
    {
        return new Vector2Int(a.x + b.x, a.y + b.y);
    }

    public static Vector2Int operator -(Vector2Int a, Vector2Int b)
    {
        return new Vector2Int(a.x - b.x, a.y - b.y);
    }

    public static Vector2Int operator -(Vector2Int a)
    {
        return new Vector2Int(-a.x, -a.y);
    }

    public static Vector2Int operator *(Vector2Int a, int scale)
    {
        return new Vector2Int(a.x * scale, a.y *scale);
    }

    public static bool operator ==(Vector2Int a, Vector2Int b)
    {
        return (a.x == b.x && a.y == b.y);
    }

    public static bool operator !=(Vector2Int a, Vector2Int b)
    {
        return !(a==b);
    }

    public override bool Equals(object obj)
    {
        if (obj is Vector2Int)
        {
            return this == (Vector2Int) obj;
        }
        else
        {
            return false;
        }
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}

public static class Gizmos2
{
    public static void DrawLabel(Vector2 position, string text, Color color)
    {
        GUIStyle style = new GUIStyle("Label");
        style.normal.textColor = color;
        style.fontSize = 14;
        style.alignment = TextAnchor.LowerRight;
        
        Vector2 size = style.CalcSize(new GUIContent(text));
        size.y = -size.y;
        size.x += 3;
        Vector2 worldSize = Camera.current.ScreenToWorldPoint(GUIUtility.GUIToScreenPoint(size))
                            - Camera.current.ScreenToWorldPoint(GUIUtility.GUIToScreenPoint(Vector2.zero));
        Rect r = new Rect(position.x, position.y, worldSize.x, worldSize.y);

    }
}

public static class GameObject2
{
    public static GameObject Instantiate(GameObject original, Transform parent, Vector3 localPosition, Quaternion localRotation)
    {
        GameObject obj = GameObject.Instantiate(original) as GameObject;
        obj.SetParentObject(parent);
        obj.transform.localPosition = localPosition;
        obj.transform.localRotation = localRotation;
        return obj;
    }

}

public enum CardinalDirections
{
    Up,
    Down,
    Left,
    Right
}