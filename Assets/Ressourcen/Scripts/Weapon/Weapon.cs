using UnityEngine;

public class Weapon : ScriptableObject
{
    [Header("Base")]
    public string Name;
    public int Id;

    [Header("Fight Specifications")]
    public float dm;
    public float startTimeBtwShot;
}
