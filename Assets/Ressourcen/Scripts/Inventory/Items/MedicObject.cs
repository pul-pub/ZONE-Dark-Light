using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Null", menuName = "Inventory/Medic")]
public class MedicObject : ScriptableObject, IItem
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
    [Header("ЧЧЧЧ----ЧЧ  Base  ЧЧЧЧ----ЧЧ")]
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

    [Header("----------  Characterisctic  ---------")]
    public float RecoveryHP = 1f;
    public float StoppingBleeding = 1f;
    public Dictionary<string, string> DiscriptionItem
    {
        get
        {
            Dictionary<string, string> _new = new Dictionary<string, string>()
            {
                { "¬осстановление здоровь€", RecoveryHP.ToString() },
                { "ќстанавливающие действие", StoppingBleeding.ToString() }
            };

            return _new;
        }
    }

    public string Use() => "";
    public int Restor(int _value) => -1;
    public int Reload(int _value) => -1;

    public IItem CloneItem() => Clone();

    public MedicObject Clone()
    {
        MedicObject _new = new MedicObject();

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

        _new.RecoveryHP = RecoveryHP;
        _new.StoppingBleeding = StoppingBleeding;

        return _new;
    }
}
