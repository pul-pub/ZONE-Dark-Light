using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public event Action<Vector2> OnMove;
    public event Action<bool> OnAttack;

    public void Update()
    {
        
    }
}
