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
    public event Action OnPressMultiButton;

    public event Action<NPCBackpack> OnInteractionPack;

    public event Action<Item[]> OnResetOutfit;

    public void ReadMovement();
    public void ReadButton(string _type);
    public void ReadButtonShoot(bool _isActiv);
    public void ReadButtonReload();
    public void ReadButtonLight();
    public void ReadNumWeapon(int _num);
    public void ReadPressMultiButton();
    public void ReadStartInteraction(TypeInteraction _type, NPCBackpack _npc);
    public void ReadResetOutfit(Item[] _items);
}
