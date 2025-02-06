using System;
using UnityEngine;

public class MobileInput : IInput
{
    public event Action<Vector2> OnMove;

    public event Action<bool> OnShoot;
    public event Action OnRload;
    public event Action<int> OnSetNumWeapon;

    public event Action OnLight;
    public event Action<float> OnCastBolt;

    public event Action<Item[]> OnResetOutfit;

    private FixedJoystick _fixedJoystick;

    public MobileInput(FixedJoystick _fixedJoystick)
    {
        this._fixedJoystick = _fixedJoystick;
    }

    public void ReadMovement()
    {
        if (OnMove != null)
        {
            if (_fixedJoystick.Vertical >= 0.7f || _fixedJoystick.Vertical <= -0.5f)
                OnMove.Invoke(new Vector2(_fixedJoystick.Horizontal, _fixedJoystick.Vertical));
            else
                OnMove.Invoke(new Vector2(_fixedJoystick.Horizontal, 0));
        }
    }
    public void ReadButtonShoot(bool _isActiv) => OnShoot?.Invoke(_isActiv);
    public void ReadButtonLight() => OnLight?.Invoke();
    public void ReadNumWeapon(int _num) => OnSetNumWeapon?.Invoke(_num);
    public void ReadOnCastBolt(Vector2 _vec) => OnCastBolt?.Invoke(Math.Abs(_vec.x) + Math.Abs(_vec.y));
    public void ReadButtonReload() => OnRload?.Invoke();
    public void ReadResetOutfit(Item[] _items) => OnResetOutfit?.Invoke(_items);
}
