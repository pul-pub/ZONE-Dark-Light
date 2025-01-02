using MessagePack;
using System.Collections.Generic;
using UnityEngine;

[MessagePackObject]
public class Character
{
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
    #endregion

    [Key(17)]
    public string endTimeSession;

    public Character Clone()
    {
        Character _new = new Character();

        _new.pos = pos;
        _new.idScene = idScene;

        _new.money = money;
        _new.items = items;

        _new.numGun = numGun;
        _new.falgGun = falgGun;

        _new.idActivQuest = idActivQuest;
        _new.idQuests = idQuests;
        _new .idEndingQuests = idEndingQuests;

        _new.switcherObject = switcherObject;
        _new.time = time;
        _new.isRain = isRain;

        _new.health = health;
        _new.name = name;
        _new.idFace = idFace;
        _new.characteristics = characteristics;

        _new.endTimeSession = endTimeSession;

        return _new;
    }
}

[MessagePackObject]
public class SavesItem
{
    [Key(0)]
    public int idItem;
    [Key(1)]
    public int count = 0;
    [Key(2)]
    public Dictionary<string, int> conditionItem = new Dictionary<string, int>(5);
    [Key(3)]
    public Dictionary<string, int> customPropertyItem = new Dictionary<string, int>(5);
    [Key(4)]
    public int[] cellsId = new int[2];
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
