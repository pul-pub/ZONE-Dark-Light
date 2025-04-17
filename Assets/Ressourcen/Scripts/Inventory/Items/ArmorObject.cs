using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Null", menuName = "Inventory/Armor")]
public class ArmorObject : ScriptableObject, IItem
{
    #region BASE
    public string Id { get => id; set => id = value; }
    public string Name { get => nameItem; set => nameItem = value; }
    public string Discription { get => discription; set => discription = value; }

    public int Price { get => price; set => price = value; }
    public int CountCell { get => countCell; set => countCell = value; }
    public int MaxCount { get => maxCount; set => maxCount = value; }
    public float Weight { get => weight; set => weight = value; }
    public float Condition { get => condition; set => condition = value; }
    public int Value { get; set; }
    public Sprite Img { get => img; set => img = value; }

    [Header("—————----—  Base  ————----——")]
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
    [Header("—————---——  Grafics  ————---——")]
    public Sprite ImgHead;
    public Sprite ImgBody;
    public Sprite ImgHand;
    public Sprite ImgLeg;
    [Space]
    public RuntimeAnimatorController animLeg;
    [Header("——-——  Characteristic  —————")]
    public int MassUp = 10;
    [Space]
    public int AntiBullet = 0;
    public int AntiRadiation = 0;
    public int AntiBio = 0;
    public int AntiChimical = 0;
    public int AntiPsi = 0;

    public Dictionary<string, string> DiscriptionItem
    {
        get
        {
            Dictionary<string, string> _new = new Dictionary<string, string>()
            {
                { "+ к переносимому весу", MassUp.ToString() },
                { "Пулестойкость", AntiBullet.ToString() },
                { "Защита от радиации", AntiRadiation.ToString() },
                { "Защита от био-заражения", AntiBio.ToString() },
                { "Защита от хим-заражения", AntiChimical.ToString() },
                { "Защита от пси-урона", AntiPsi.ToString() },
            };

            return _new;
        }
    }

    public string Use() => "";
    public int Restor(int _value) => -1;
    public int Reload(int _value) => -1;

    public IItem CloneItem() => Clone();
    public ArmorObject Clone()
    {
        ArmorObject _new = new ArmorObject();

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

        _new.ImgHead = ImgHead;
        _new.ImgBody = ImgBody;
        _new.ImgHand = ImgHand;
        _new.ImgLeg = ImgLeg;
        _new.animLeg = animLeg;

        _new.MassUp = MassUp;
        _new.AntiBullet = AntiBullet;
        _new.AntiRadiation = AntiRadiation;
        _new.AntiBio = AntiBio;
        _new.AntiChimical = AntiChimical;
        _new.AntiPsi = AntiPsi;

        return _new;
    }
}
