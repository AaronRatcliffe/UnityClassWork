using System;
using UnityEngine;

public static class MathUtilities 
{

    public static int TrueMod(int num, int mod)
    {
        return ((num % mod) + mod) % mod;
    }

    public static float TrueMod(float num, float mod)
    {
        return ((num % mod) + mod) % mod;
    }

    public static float CapMagnitude(float value, float maxMagnitude)
    {
        return (Mathf.Sign(value) * Mathf.Min(Mathf.Abs(value), maxMagnitude));            
    }

}
