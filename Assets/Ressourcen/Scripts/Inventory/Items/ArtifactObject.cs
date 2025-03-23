using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Null", menuName = "Artifact")]
public class ArtifactObject : ScriptableObject, IItem
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

    [Header("—————---——  Base  ————----——")]
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
    public float Amplitude = 0.1f;
    public float Period = 0.1f;
    [Header("——-——  Characteristics  —————")]
    public float AntiDamage = 0;
    public float AntiRadiation = 0;
    public float AntiBio = 0;
    public float AntiChimical = 0;
    public float AntiPsi = 0;
    public int ChangeMussUp = 0;
    public float ChangeSpeed = 0;
    public float RecoveryEnergy = 0;

    public Dictionary<string, string> DiscriptionItem
    {
        get
        {
            Dictionary<string, string> _new = new Dictionary<string, string>()
            {
                { "Амплитуда", Amplitude.ToString() },
                { "Период", Period.ToString() },
                { "Защита от урона", AntiDamage.ToString() },
                { "Защита от радиации", AntiRadiation.ToString() },
                { "Защита от био-заражения", AntiBio.ToString() },
                { "Защита от хим-заражения", AntiChimical.ToString() },
                { "Защита от пси-урона", AntiPsi.ToString() },
                { "Бафф к переносу тяжестей", ChangeMussUp.ToString() },
                { "Бафф к скорости", ChangeSpeed.ToString() },
                { "Востановление энергии", RecoveryEnergy.ToString() }
            };

            return _new;
        }
    }

    public string Use() => "";
    public int Restor(int _value) => -1;
    public int Reload(int _value) => -1;


    public IItem CloneItem() => Clone();

    public ArtifactObject Clone()
    {
        ArtifactObject _new = new ArtifactObject();

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

        _new.Amplitude = Amplitude;
        _new.Period = Period;

        _new.AntiDamage = AntiDamage;
        _new.AntiRadiation = AntiRadiation;
        _new.AntiBio = AntiBio;
        _new.AntiChimical = AntiChimical;
        _new.AntiPsi = AntiPsi;
        _new.ChangeMussUp = ChangeMussUp;
        _new.ChangeSpeed = ChangeSpeed;
        _new.RecoveryEnergy = RecoveryEnergy;

        return _new;
    }
}
