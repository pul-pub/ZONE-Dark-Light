using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Null", menuName = "Detector")]
public class DetectorObject : ScriptableObject, IItem
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
    public int Value { get => (int)_charge; set => _charge = value; }
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
    [Header("——  Find Characteristics  ——")]
    public float MinAmplitude = 0.1f;
    public float MaxAmplitude = 0.1f;
    public float MinPeriod = 0.1f;
    public float MaxPeriod = 0.1f;
    [Header("—————-——  Zone find  ————--—")]
    public float RadiusCheck;
    public Vector2 DurectionCheck;

    public Dictionary<string, string> DiscriptionItem
    {
        get
        {
            Dictionary<string, string> _new = new Dictionary<string, string>()
            {
                { "Äèàïàçîíà àìïëèòóäû", MinAmplitude + " - " + MaxAmplitude },
                { "Äèàïàçîíà ïåðèîäà", MinPeriod + " - " + MaxPeriod },
                { "Ðàäèóñ ïîêðûòèÿ", RadiusCheck.ToString() }
            };

            return _new;
        }
    }

    private float _charge = 100;

    public string Use() => "";
    public int Restor(int _value) => -1;
    public int Reload(int _value) => -1;

    public IItem CloneItem() => Clone();

    public DetectorObject Clone()
    {
        DetectorObject _new = new DetectorObject();

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

        _new.MinAmplitude = MinAmplitude;
        _new.MaxAmplitude = MaxAmplitude;
        _new.MinPeriod = MinPeriod;
        _new.MaxPeriod = MaxPeriod;

        _new.RadiusCheck = RadiusCheck;
        _new.DurectionCheck = DurectionCheck;

        return _new;
    }
}
