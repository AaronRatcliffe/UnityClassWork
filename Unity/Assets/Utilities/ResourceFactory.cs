using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEngine;
using System.Collections.Generic;


public static class ResourceFactory<TEnum> where TEnum : struct, IConvertible
{

	#region Fields

    private static Dictionary<TEnum, GameObject> _resourceCache;
	
	#endregion
	
	
    #region Constructor

    static ResourceFactory()
    {
        System.Diagnostics.Debug.Assert(typeof(TEnum).IsEnum);
        _resourceCache = new Dictionary<TEnum, GameObject>();
    }

    #endregion


    #region Public Methods

    /// <summary>
    /// Load and instantiate the resource designated by the given value.
    /// The resource should be located at "*Resources/EnumType/EnumValue"
    /// </summary>
    [System.Diagnostics.DebuggerNonUserCodeAttribute()]
    public static GameObject Instantiate(TEnum resource, Vector3 position, Quaternion rotation)
    {
        GameObject resourceObject;

        if (_resourceCache.ContainsKey(resource))
        {
            resourceObject = _resourceCache[resource];
        }
        else
        {
            string location = typeof(TEnum).Name + "/" + resource.ToString();

            resourceObject = Resources.Load<GameObject>(location);
            _resourceCache.Add(resource, resourceObject);
        }
        
        return GameObject.Instantiate(resourceObject, position, rotation) as GameObject;
    }

    /// <summary>
    /// Load and instantiate the resource designated by the given value.
    /// The resource should be located at "*Resources/EnumType/EnumValue"
    /// </summary>
    [System.Diagnostics.DebuggerNonUserCodeAttribute()]
    public static GameObject Instantiate(TEnum resource, Vector2 position)
    {
        return Instantiate(resource, position, Quaternion.identity);
    }

    /// <summary>
    /// Load and instantiate the resource designated by the given value.
    /// The resource should be located at "*Resources/EnumType/EnumValue"
    /// </summary>
    [System.Diagnostics.DebuggerNonUserCodeAttribute()]
    public static GameObject Instantiate(TEnum resource)
    {
        return Instantiate(resource, Vector2.zero, Quaternion.identity);
    }
	
	#endregion
	
}

public static class SingletonResourceFactory<T> where T : ScriptableObject
{
    private static T cache;

    public static T Get()
    {
        if (cache == null)
        {
            string location = "Static/" + typeof(T).Name;
            cache = Resources.Load<T>(location);
        }

        return cache;
    }
}