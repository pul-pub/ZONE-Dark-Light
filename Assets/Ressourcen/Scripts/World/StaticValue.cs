using System.Collections.Generic;
using UnityEngine;

public static class StaticValue
{
    public static string SessionToken;

    public static int[] time = new int[2] { 6, 0 };
    public static float lightLevel = 0.69f;

    public static Dictionary<string, bool> baseSwitcherObject = new();

    static StaticValue()
    {
        baseSwitcherObject.Add("Karatel", true); //первый чистонебовац на болтотах
        baseSwitcherObject.Add("StartCall", true); // Начальный разговор по рации
        baseSwitcherObject.Add("ProvodnikCall-Cerkov", true); // Говорит проводник после перехода на Церковь
        baseSwitcherObject.Add("InScene", false); //анимация ухода Крота и Сварога
        baseSwitcherObject.Add("SvarogS", true); //первый разговор со Сварогом
        baseSwitcherObject.Add("MninBoss-1", true); //мини босс в церкви
        baseSwitcherObject.Add("MninBoss-1-End", true); //коней иследований
    }
}
