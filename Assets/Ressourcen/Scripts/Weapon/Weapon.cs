using UnityEngine;

public abstract class Weapon : ScriptableObject
{
    [Header("Base")]
    public string Name;
    public int Id;
    public int condition = 100;

    [Header("Fight Specifications")]
    public float dm;
    public float startTimeBtwShot;
}
