using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public enum TypeLight { Light, PNV }

[CreateAssetMenu(fileName = "Null", menuName = "Inventory/Light")]
public class LightObject : ScriptableObject, IItem
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
    public int Value { get => (int)_change; set => _change = value; }
    public Sprite Img { get => img; set => img = value; }

    [Header("————----——  Base  ————----——")]
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
    [Header("————---——  Grafics  ————---——")]
    public Sprite ImgLight;
    public TypeLight TypeLight;
    [Header("—————---——  Light  ————----——")]
    public VolumeProfile Profile;

    public Dictionary<string, string> DiscriptionItem
    {
        get
        {
            Dictionary<string, string> _new = new Dictionary<string, string>()
            {
                { "Çàðÿä", Value.ToString() },
                { "Òèï", TypeLight.ToString() }
            };

            return _new;
        }
    }

    private float _change = 100;

    public string Use() => "";
    public int Restor(int _value) => -1;
    public int Reload(int _value) => -1;

    public IItem CloneItem() => Clone();

    public LightObject Clone()
    {
        LightObject _new = new LightObject();

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

        _new.ImgLight = ImgLight;
        _new.TypeLight = TypeLight;

        _new.Profile = Profile;

        return _new;
    }
}
