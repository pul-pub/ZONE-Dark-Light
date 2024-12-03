using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public event Action Deid;

    public float health { get; private set; } = 100f;

    public void ApplyDamage(float damage)
    {
        if (health > 0)
            health -= damage;

        if (health <= 0 && Deid != null)
            Deid.Invoke();
    }
}
