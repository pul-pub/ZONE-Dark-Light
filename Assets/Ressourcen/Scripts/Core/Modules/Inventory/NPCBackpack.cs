using System;
using System.Collections.Generic;
using UnityEngine;

public class NPCBackpack : MonoBehaviour, IPack
{
    public event System.Action<Dictionary<string, IItem>> ChangeOutfit;
    public event System.Action OnUpdateInventory;

    public List<ObjectItem> DeathPack { get; set; } = new();
    public float WeightInventory
    {
        get
        {
            float weight = 0;

            foreach (ObjectItem i in _main)
                if (i)
                    weight += i.Item.Weight * i.Count;

            return weight;
        }
    }

    [SerializeField] private DataBase data;
    [Header("---------  Random  ----------")]
    [SerializeField] private List<RandomItem> itemsRandom = new List<RandomItem>();
    [Header("---------  Static  ----------")]
    [SerializeField] private List<StaticItem> itemsStatic = new List<StaticItem>();

    private List<ObjectItem> _main = new();

    public void Initialization()
    {
        Dictionary<string, IItem> _outFit = new();
        foreach (StaticItem ii in itemsStatic)
        {
            IItem _new = data.GetItem(ii.id).CloneItem();
            ObjectItem _obj = new();

            _new.Value = ii.value;
            _new.Condition = ii.cond;
            _obj.Count = ii.count;
            _obj.Item = _new;

            _main.Add(_obj);

            if (CheckItemID(ii.id, "GUN"))
                _outFit.Add(_new.CountCell == 1 ? "PIS" : "GUN", _new);
            else if (CheckItemID(ii.id, "ARM"))
                _outFit.Add("ARM", _new);
            else if (CheckItemID(ii.id, "PAK"))
                _outFit.Add("PAK", _new);
        }

        if (_outFit.Count > 0)
            ChangeOutfit?.Invoke(_outFit);

        OnUpdateInventory?.Invoke();
    }

    public void CreateDeathPack(IMetaEssence _meta)
    {
        foreach (RandomItem randomItem in itemsRandom)
        {
            int chance = UnityEngine.Random.Range(0 , 100);

            if (chance < randomItem.chance)
            {
                IItem _new = data.GetItem(randomItem.id).CloneItem();
                ObjectItem _obj = new();

                _new.Value = randomItem.value;
                _new.Condition = UnityEngine.Random.Range(randomItem.condMin, randomItem.condMax);
                _obj.Count = UnityEngine.Random.Range(randomItem.countMin, randomItem.countMax);
                _obj.Item = _new;

                DeathPack.Add(_obj);
            }
        }
    }

    public List<ObjectItem> GetItems(string _id)
    {
        List<ObjectItem> _items = new();

        foreach (ObjectItem item in _main)
            if (item.Item.Id == _id)
                _items.Add(item);

        return _items;
    }

    public void SetItems(List<ObjectItem> _items, int _count)
    {
        for (int i = 0; i < _items.Count; i++)
        {
            if (_items[i] != null)
            {
                if (_items[i].Count >= _count)
                {
                    SubtractionItems(_items[i], _count);
                    break;
                }
                else
                {
                    _count -= _items[i].Count;
                    SubtractionItems(_items[i], _count);
                }
            }
        }
    }

    private void SubtractionItems(ObjectItem _currentItem, int _subtractionCount)
    {
        if (_currentItem.Count - _subtractionCount > 0)
            _currentItem.Count -= _subtractionCount;
        else
        {
            _currentItem.Count = 0;
            _main.Remove(_currentItem);
            return;
        }
    }

    private bool CheckItemID(string _id, string _targetType)
    {
        char[] _listID = _id.ToCharArray();
        string _typeItem = _listID[0].ToString() + _listID[1].ToString() + _listID[2].ToString();
        return _typeItem == _targetType;
    }
}

[Serializable]

public class RandomItem
{
    public string id;
    public int value;

    public float chance;
    public int countMax;
    public int countMin;
    public int condMax;
    public int condMin;
}

[Serializable]

public class StaticItem
{
    public string id;
    public int value;

    public int count;
    public float cond;
}
