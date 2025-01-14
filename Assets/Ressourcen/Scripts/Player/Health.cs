using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public event Action<IMetaEnemy> Deid;
    public float baseValueHealth;
    public bool isDamagebly;
    public float health { get; private set; } = 100f;


    private void Awake()
    {
        health = baseValueHealth;
    }

    public void ApplyDamage(float damage, IMetaEnemy _meta = null)
    {
        if (health > 0 && isDamagebly)
            health -= damage;

        if (health <= 0 && Deid != null)
            Deid.Invoke(_meta);
    }
}
