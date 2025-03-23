using UnityEngine;

public abstract class Weapon : ScriptableObject
{
    [Header("Fight Specifications")]
    public float Dm;
    public float StartTimeBtwShot;
}
