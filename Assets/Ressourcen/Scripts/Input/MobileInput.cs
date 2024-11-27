using System;
using UnityEngine;

public class MobileInput : IInput
{
    public event Action<Vector2> OnMove;

    public event Action<bool> OnShoot;
    public event Action OnRload;
    public event Action<int> OnSetNumWeapon;

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

    public void ReadButton(string _type)
    {
        if (OnRload != null && _type == "reload")
            OnRload.Invoke();
    }

    public void ReadButtonShoot(bool _isActiv)
    {
        if (OnShoot != null)
            OnShoot.Invoke(_isActiv);
    }

    public void ReadNumWeapon(int _num)
    {
        if (OnSetNumWeapon != null)
            OnSetNumWeapon.Invoke(_num);
    }

    public void ReadResetOutfit(Item[] _items)
    {
        if (OnResetOutfit != null)
            OnResetOutfit.Invoke(_items);
    }
}
