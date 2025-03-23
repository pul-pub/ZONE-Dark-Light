using MessagePack;
using MessagePack.Unity;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[MessagePackObject]
public class Character
{
    public event Action<Dictionary<string, bool>> OnResetSwitchObject;

    #region PLAYER
    [Key(0)]
    public PlayerPos pos;
    [Key(1)]
    public int idScene = 1;
    #endregion

    #region INVENTORY
    [Key(2)]
    public int money = 0;
    [Key(3)]
    public List<SavesItem> items = new List<SavesItem>();
    #endregion

    #region WEAPON_ON_PLAYER
    [Key(4)]
    public int numGun = 0;
    [Key(5)]
    public bool falgGun = false;
    #endregion

    #region QUESTS
    [Key(6)]
    public int idActivQuest = -1;
    [Key(7)]
    public List<int> idQuests = new List<int>();
    [Key(8)]
    public List<int> idEndingQuests = new List<int>();
    #endregion

    #region WORLD
    [Key(10)]
    public Dictionary<string, bool> switcherObject;
    [Key(11)]
    public int[] time = new int[2] { 0, 0 };
    [Key(12)]
    public bool isRain = false;
    #endregion

    #region CHARECTER
    [Key(13)]
    public float health = 100;
    [Key(14)]
    public string name = "--NONE--";
    [Key(15)]
    public int idFace = 0;
    [Key(16)]
    public Dictionary<string, int> characteristics = new Dictionary<string, int>();
    [Key(17)]
    public Dictionary<string, float> hpBodyParth = new Dictionary<string, float>();
    [Key(19)]
    public bool onLight = false;
    #endregion

    [Key(18)]
    public string endTimeSession;

    public Character Clone()
    {
        Character _new = new Character();

        _new.pos = new(pos.flipX, pos.x, pos.y);
        _new.idScene = idScene;

        _new.money = money;
        _new.items = new();
        foreach (SavesItem si in items)
            _new.items.Add(si.Clone());

        _new.numGun = numGun;
        _new.falgGun = falgGun;

        _new.idActivQuest = idActivQuest;
        _new.idQuests = idQuests;
        _new.idEndingQuests = idEndingQuests;

        _new.switcherObject = new();
        foreach (string k in switcherObject.Keys)
            _new.switcherObject.Add(k, switcherObject[k]);
        _new.hpBodyParth = new();
        _new.time = time;
        _new.isRain = isRain;

        _new.health = health;
        _new.name = name;
        _new.idFace = idFace;
        _new.characteristics = characteristics;
        foreach (string k in hpBodyParth.Keys)
            _new.hpBodyParth.Add(k, hpBodyParth[k]);

        _new.endTimeSession = endTimeSession;

        return _new;
    }

    public bool GetSwitchObject(string _name)
    {
        foreach (string k in StaticValue.baseSwitcherObject.Keys)
            if (k == _name)
                switcherObject.TryAdd(k, StaticValue.baseSwitcherObject[k]);

        return switcherObject[_name];
    }

    public void SetSwitchObject(string _name, bool _value)
    {
        foreach (string k in switcherObject.Keys)
        {
            if (k == _name)
            {
                switcherObject[k] = _value;
                OnResetSwitchObject?.Invoke(switcherObject);
                return;
            }
        }
        
        foreach (string k in StaticValue.baseSwitcherObject.Keys)
        {
            if (k == _name)
            {
                switcherObject.Add(k, StaticValue.baseSwitcherObject[k]);
                switcherObject[k] = _value;
                OnResetSwitchObject?.Invoke(switcherObject);
                return;
            }
        }
    }
}

[MessagePackObject]
public class SavesItem
{
    [Key(0)]
    public string idItem;
    [Key(1)]
    public int count = 0;
    [Key(2)]
    public float condition;
    [Key(3)]
    public int value;
    [Key(4)]
    public int[] cellsId = new int[2];

    public SavesItem Clone()
    {
        SavesItem _new = new();

        _new.idItem = idItem;
        _new.count = count;
        _new.condition = condition;
        _new.value = value;
        _new.cellsId = cellsId;

        return _new;
    }
} 

[MessagePackObject]
public class PlayerPos
{
    [Key(0)]
    public int flipX;
    [Key(1)]
    public float x;
    [Key(2)]
    public float y;

    public PlayerPos(int flipX, float x, float y)
    {
        this.flipX = flipX;
        this.x = x;
        this.y = y;
    }
}
