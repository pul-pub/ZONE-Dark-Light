using System;
using UnityEngine;

public class DesctopInput : IInput
{
    public event Action<Vector2> OnMove;

    public event Action<bool> OnShoot;
    public event Action OnRload;
    public event Action<int> OnSetNumWeapon;

    public event Action OnLight;
    public event Action<float> OnCastBolt;

    public event Action<Item[]> OnResetOutfit;

    private FixedJoystick _fixedJoystick;


    public void ReadMovement()
    {
        if (OnMove != null)
        {
            OnMove.Invoke(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
        }
    }
    public void ReadButtonShoot(bool _isActiv) => OnShoot?.Invoke(_isActiv);
    public void ReadButtonLight() => OnLight?.Invoke();
    public void ReadNumWeapon(int _num) => OnSetNumWeapon?.Invoke(_num);
    public void ReadOnCastBolt(Vector2 _vec) => OnCastBolt?.Invoke(Math.Abs(_vec.x) + Math.Abs(_vec.y));
    public void ReadButtonReload() => OnRload?.Invoke();
    public void ReadResetOutfit(Item[] _items)
    {
        if (OnResetOutfit != null)
            OnResetOutfit.Invoke(_items);
    }
}
