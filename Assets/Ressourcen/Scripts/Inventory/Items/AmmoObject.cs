using UnityEngine;

[CreateAssetMenu(fileName = "Null", menuName = "Inventory/Ammo")]
public class AmmoObject : ScriptableObject, IItem
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

    [Header("-----  Charecteristic  ------")]
    public float ForceBullet;
    public int CountIn;

    [Header("----------  Shoot  ----------")]
    public Object ObjBullet;
    public Object ObjGilz;

    public string Use() => "";
    public int Restor(int _value) => -1;
    public int Reload(int _value) => -1;


    public IItem CloneItem() => Clone();
    public AmmoObject Clone()
    {
        AmmoObject _new = new AmmoObject();

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

        _new.ForceBullet = ForceBullet;
        _new.CountIn = CountIn;

        _new.ObjBullet = ObjBullet;
        _new.ObjGilz = ObjGilz;

        return _new;
    }
}
