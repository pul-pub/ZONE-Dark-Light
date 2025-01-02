using System.Collections.Generic;
using UnityEngine;

public static class StaticValue
{
    public static string SessionToken;

    public static int[] time = new int[2] { 6, 0 };
    public static float lightLevel = 0.69f;

    public static Dictionary<string, bool> baseSwitcherObject = new();
}
