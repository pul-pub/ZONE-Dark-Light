using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gun", fileName = "Null")]
public class Gun : Weapon, IItem
{
    #region Base IItem
    public string Id { get => id; set => id = value; }
    public string Name { get => nameItem; set => nameItem = value; }
    public string Discription { get => discription; set => discription = value; }

    public int Price { get => price; set => price = value; }
    public int CountCell { get => countCell; set => countCell = value; }
    public int MaxCount { get => maxCount; set => maxCount = value; }
    public float Weight { get => weight; set => weight = value; }
    public float Condition { get => condition; set => condition = value; }
    public int Value { get => _cerentsAmmo; set => _cerentsAmmo = value; }
    public Sprite Img { get => img; set => img = value; }

    [Header("ЧЧЧЧЧ----Ч  Base  ЧЧЧЧ----ЧЧ")]
    [SerializeField] private string id;
    [SerializeField] private string nameItem;
    [SerializeField] private string discription;
    [Space]
    [SerializeField] private int price;
    [SerializeField] private int countCell;
    [SerializeField] private int maxCount;
    [SerializeField] private float weight;
    [SerializeField] private float condition;
    [SerializeField] private int value;
    [SerializeField] private Sprite img;
    #endregion

    [Header("---------  Grafics  ---------")]
    public Sprite ImgBoxGun;
    public Sprite ImgStor;
    public Vector2 PointOutBullet;
    public Vector2 PointFire;
    public Vector2 PointOutGilz;
    public UnityEngine.Object ObjFire;

    [Header("----------  Audio  ----------")]
    public AudioClip SoundShoot;
    public AudioClip SoundNullShoot;

    [Header("-----  Charecteristic  ------")]
    public string TypeAmmo;
    public int MaxAmmo;
    public bool StorAmmoTakes;
    public bool StorGrffics;

    public Dictionary<string, string> DiscriptionItem
    {
        get
        {
            Dictionary<string, string> _new = new Dictionary<string, string>()
            {
                { "”рон", Dm.ToString() },
                { "—корострельность", ((1 - StartTimeBtwShot) * 100).ToString() },
                { "ќбъЄм магазина", MaxAmmo.ToString() },
                { "Ўанс клина", (Math.Round((1 - (Condition * 0.01f)) * 100), 4).ToString() + " %" },
                { "“ип боеприпаса", (TypeAmmo).ToString() }
            };

            return _new;
        }
    }

    private int _cerentsAmmo = 0;

    public string Use()
    {
        if (Value != 0)
        {
            Value -= 1;
            int _rand = UnityEngine.Random.Range(0, 75);

            if (_rand < Condition)
            {
                return "";
            }
            else
                return "LowCondition";
        }
        else
            return "NoAmmo";
    }

    public int Restor(int _value) => 1;

    public int Reload(int _value)
    {
        int reason = MaxAmmo - _cerentsAmmo;
        int returnAmmos = 0;

        if (_value >= reason)
        {
            returnAmmos += reason;
            _cerentsAmmo += reason;
        }
        else
        {
            _cerentsAmmo += _value;
            returnAmmos = _value;
        }

        return returnAmmos;
    }

    public IItem CloneItem() => Clone();

    public Gun Clone()
    {
        Gun _new = new Gun();

        _new.Id = Id;
        _new.Name = Name;
        _new.Discription = Discription;
        
        _new.Price = Price;
        _new.CountCell = CountCell;
        _new.MaxCount = MaxCount;
        _new.Weight = Weight;
        _new.Condition = Condition;
        _new.Value = Value;
        _new.Img = Img;

        _new.Dm = Dm;
        _new.StartTimeBtwShot = StartTimeBtwShot;

        _new.ImgBoxGun = ImgBoxGun;
        _new.ImgStor = ImgStor;
        _new.PointOutBullet = PointOutBullet;
        _new.PointFire = PointFire;
        _new.PointOutGilz = PointOutGilz;
        _new.ObjFire = ObjFire;

        _new.SoundShoot = SoundShoot;
        _new.SoundNullShoot = SoundNullShoot;

        _new.TypeAmmo = TypeAmmo;
        _new.MaxAmmo = MaxAmmo;
        _new.StorAmmoTakes = StorAmmoTakes;
        _new.StorGrffics = StorGrffics;

        return _new;
    }
}
