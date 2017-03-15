using System;
using UnityEngine;

public class MultiEnumFlagsAttribute : PropertyAttribute
{
    public MultiEnumFlagsAttribute() { }
}

public class SingleEnumFlagAttribute : PropertyAttribute
{
    public SingleEnumFlagAttribute() { }
}

public class LongStringAttribute : PropertyAttribute
{
    public LongStringAttribute() { }
}

public class ValidRegexAttribute : PropertyAttribute
{
    public readonly string Regex;

    public ValidRegexAttribute(string regex)
    {
        Regex = regex;
    }
}