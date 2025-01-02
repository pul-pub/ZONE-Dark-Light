using System;
using UnityEngine;

public enum TypeInteraction { TackeBackpack, Dialog };

public interface IInput
{
    public event Action<Vector2> OnMove;

    public event Action<bool> OnShoot;
    public event Action OnRload;
    public event Action<int> OnSetNumWeapon;

    public event Action OnLight;
    public event Action<float> OnCastBolt;

    public event Action<Item[]> OnResetOutfit;

    public void ReadMovement();
    public void ReadButtonShoot(bool _isActiv);
    public void ReadButtonReload();
    public void ReadButtonLight();
    public void ReadNumWeapon(int _num);
    public void ReadResetOutfit(Item[] _items);
    public void ReadOnCastBolt(Vector2 _vec);
}
