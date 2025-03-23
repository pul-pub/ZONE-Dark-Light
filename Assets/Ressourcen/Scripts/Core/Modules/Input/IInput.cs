using System;
using UnityEngine;

public interface IInput
{
    public event Action<Vector2> Move;
    public event Action<Vector2> Viwe;

    public event Action<bool> Shoot;
    public event Action Reload;
    public event Action<int> SwitchWeapon;

    public event Action SwitchLight;
    public event Action<float> CastBolt;

    public void Initialization(TypeGroup _group);
}
