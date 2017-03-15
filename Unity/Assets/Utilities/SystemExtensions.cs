using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class SystemExtensions 
{
    	
	#region Public Methods

    public static T PickRandom<T>(this IEnumerable<T> source)
    {
        int rand = UnityEngine.Random.Range(0, source.Count());
        return source.ElementAt(rand);
    }

    #region MinBy

    public static T MinBy<T>(this IEnumerable<T> source, Func<T, int> selector)
    {
        T min = default(T);
        int minVal = int.MaxValue;

        foreach (T element in source)
        {
            int val = selector(element);

            if (val < minVal)
            {
                min = element;
                minVal = val;
            }
        }

        return min;
    }

    public static T MinBy<T>(this IEnumerable<T> source, Func<T, float> selector)
    {
        T min = default(T);
        float minVal = float.MaxValue;

        foreach (T element in source)
        {
            float val = selector(element);

            if (val < minVal)
            {
                min = element;
                minVal = val;
            }
        }

        return min;
    }

    #endregion 

    #region MaxBy

    public static T MaxBy<T>(this IEnumerable<T> source, Func<T, int> selector)
    {
        T max = default(T);
        int maxVal = int.MinValue;

        foreach (T element in source)
        {
            int val = selector(element);

            if (val > maxVal)
            {
                max = element;
                maxVal = val;
            }
        }

        return max;
    }

    public static T MaxBy<T>(this IEnumerable<T> source, Func<T, float> selector)
    {
        T max = default(T);
        float maxVal = float.MinValue;

        foreach (T element in source)
        {
            float val = selector(element);

            if (val > maxVal)
            {
                max = element;
                maxVal = val;
            }
        }

        return max;
    }

    #endregion

    #region MinsBy
    public static IEnumerable<T> MinsBy<T>(this IEnumerable<T> source, Func<T, int> selector)
    {
        List<T> mins = new List<T>();
        int minVal = int.MaxValue;

        foreach (T element in source)
        {
            int val = selector(element);

            if (val > minVal) continue;

            if (val < minVal)
            {
                mins.Clear();
                minVal = val;
            }
            mins.Add(element);
        }

        return mins;
    }

    public static IEnumerable<T> MinsBy<T>(this IEnumerable<T> source, Func<T, float> selector)
    {
        List<T> mins = new List<T>();
        float minVal = float.MaxValue;

        foreach (T element in source)
        {
            float val = selector(element);

            if (val > minVal) continue;

            if (val < minVal)
            {
                mins.Clear();
                minVal = val;
            }
            mins.Add(element);
        }

        return mins;
    }
    #endregion

    #region MaxsBy
    public static IEnumerable<T> MaxsBy<T>(this IEnumerable<T> source, Func<T, int> selector)
    {
        List<T> maxs = new List<T>();
        int maxVal = int.MinValue;

        foreach (T element in source)
        {
            int val = selector(element);

            if (val < maxVal) continue;

            if (val > maxVal)
            {
                maxs.Clear();
                maxVal = val;
            }
            maxs.Add(element);
        }

        return maxs;
    }

    public static IEnumerable<T> MaxsBy<T>(this IEnumerable<T> source, Func<T, float> selector)
    {
        List<T> maxs = new List<T>();
        float maxVal = float.MinValue;

        foreach (T element in source)
        {
            float val = selector(element);

            if (val < maxVal) continue;

            if (val > maxVal)
            {
                maxs.Clear();
                maxVal = val;
            }
            maxs.Add(element);
        }

        return maxs;
    }
    #endregion

    

    public static T[,] ToArray2D<T>(this IEnumerable<T> source, int width, int height)
    {
        T[,] result = new T[width, height];

        int x = 0;
        int y = 0;
        foreach (T t in source)
        {
            result[x, y] = t;

            x = (x+1) % width;
            if (x == 0) y++;
        }

        return result;
    }

    public static bool AllEqualBy<T, TResult>(this IEnumerable<T> source, Func<T, TResult> keySelector)
    {
        return source.Select(keySelector).Distinct().Count() <= 1;
    }

    public static void Foreach<T>(this IEnumerable<T> source, Action<T> action)
    {
        foreach (T t in source)
        {
            action(t);
        }
    }

    public static void Foreach2D<T>(this T[,] source, Action<int, int> actionXY)
    {
        int width = source.GetLength(0);

        int x = 0;
        int y = 0;

        var enumerator = source.GetEnumerator();
        while (enumerator.MoveNext())
        {
            actionXY(x, y);
            x = (x + 1) % width;
            if (x == 0) y++;
        }
    }

    public static void SetEach<T>(this IList<T> source, Func<T, T> setter)
    {
        for (int i = 0; i < source.Count(); i++)
        {
            source[i] = setter(source[i]);
        }
    }

    public static IEnumerable<T> Except<T>(this IEnumerable<T> source, params T[] exceptions)
    {
        return source.Except(exceptions.AsEnumerable());
    }

    public static bool ContainsFlag<TEnum>(this TEnum mask, TEnum flag) where TEnum : struct, IConvertible
    {
        int maskValue = (int)(object)mask;
        int flagValue = (int)(object)flag;

        return (maskValue & flagValue) == flagValue;
    }

    #endregion

}

public static class Enumerable2
{

    public static IEnumerable<T> RepeatNew<T>(int count) where T:new()
    {
        for (int i = 0; i < count; i++)
        {
            yield return new T();
        }
    }

}