using System.Collections.Generic;
using UnityEngine;

public abstract class Inventory : MonoBehaviour, IPack
{
    public event System.Action<Dictionary<string, IItem>> ChangeOutfit;
    public event System.Action OnUpdateInventory;
    public event System.Action<ObjectItem, ObjectItem> OnEndCheckCell;
    public event System.Action<ObjectItem> OpenDiscription;

    public int Money { get; private set; }
    public float WeightInventory
    {
        get
        {
            float weight = 0;

            foreach (ObjectItem i in _items)
                if (i)
                    weight += i.Item.Weight * i.Count;

            return weight;
        }
    }

    public List<ObjectItem> DeathPack { get; set; }

    protected DataBase _data;
    protected Object _objectItem;
    protected Transform[] _parentBase = new Transform[3];
    protected Transform _parentDrag;

    private List<ObjectItem> _items = new List<ObjectItem>();
    protected List<ObjectCell> _cellObjs = new List<ObjectCell>();

    private int _coutCellsPlayer 
    {
        get
        {
            int _count = 0;
            foreach (ObjectCell cell in _cellObjs)
                _count++;

            return _count;
        }
    }
    private int _coutCellsNPC
    {
        get
        {
            int _count = 0;
            if (_cellObjs.Find(c => c.cellID > 500))
                _count++;

            return _count;
        }
    }

    public void Initialization()
    {

    }

    public void CreateDeathPack(IMetaEssence _meta)
    {

    }


    public void OnGiveMoney(int _money) => Money += _money;
    public void AddItems(List<IItem> _item, int[] _count)
    {
        for (int i = 0; i < _item.Count; i++)
            AddItem(_item[i].CloneItem(), _count[i], _items);
    }

    public int CalculatedMass()
    {
        int _allMass = 0;

        foreach (ObjectItem _itemObj in _items)
            if (_itemObj != null && _itemObj.CellsId[0] < 500)
                _allMass += (int)(_itemObj.Item.Weight * _itemObj.Count);

        return _allMass;
    }

    public List<ObjectItem> GetItems(string _id)
    {
        List<ObjectItem> _items = new();

        foreach (ObjectItem item in this._items)
            if (item.Item.Id == _id && item.CellsId[0] < 500)
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

        CalculatedMass();
    }

    public void AdditionItems(ObjectItem _currentItem, ObjectItem _targetItem)
    {
        if (_currentItem.Count + _targetItem.Count <= _currentItem.Item.MaxCount)
            _currentItem.Count += _targetItem.Count;
        else
            _currentItem.Count = _currentItem.Item.MaxCount;
    }

    public void UpdateAllItems()
    {
        for (int i = 0; i < _items.Count; i++)
        {
            if (_items[i])
                _items[i].UpdateValue();
            else
                _items.Remove(_items[i]);
        }

        OnUpdateInventory?.Invoke();
    }


    protected bool AppItem(IItem _item, int _count) => AddItem(_item, _count, _items);
    protected bool AddNPCItem(IItem _item, int _count) => AddItem(_item, _count, _items, 500);
    protected void UpdateStatusItems(string _screen)
    {
        foreach (ObjectItem oi in _items)
        {
            Debug.Log(oi.CellsId[0]);
            if (oi.CellsId[0] >= 100)
                oi.gameObject.SetActive(_screen == "OTF");

            if (oi.CellsId[0] >= 500)
            {
                _items.Remove(oi);
                Destroy(oi.gameObject);
            }
        }
    }

    private bool AddItem(IItem _item, int _count, List<ObjectItem> _list, int _startCell = 0, int[] _addCell = null)
    {
        GameObject _gObj = Instantiate(_objectItem, _parentBase[0]) as GameObject;

        int[] _cells = _addCell == null ? FindNullCell(_item.CountCell, _startCell) : _addCell;

        ObjectItem ii = _gObj.GetComponent<ObjectItem>();

        ii.OnStartDraging += DragingStart;
        ii.OnEndDraging += DragingEnd;
        ii.OnCheckCell += CheckCell;
        ii.OnUpdateAll += UpdateAllItems;
        ii.OpenDescription += OnOpenDiscription;
        ii.OnUpdateOutFit += OnUpdateOutFit;
        OnEndCheckCell += ii.OnDrop;
        
        ii.Initialization(_item.CloneItem(), _cells, FindCellPosition(_cells[0]), _count < _item.MaxCount ? _count : _item.MaxCount);
        _list.Add(_gObj.GetComponent<ObjectItem>());

        OnUpdateInventory?.Invoke();

        return true;
    }

    private int[] FindNullCell(int _countCells = 1, int _startCell = 0)
    {
        for (int i = _startCell; i < _startCell + _coutCellsPlayer; i++)
            if (_countCells > 1 ? (i + 1) % 7 != 0 : true && GetCheckedCell(i) == null)
                return _countCells > 1 ? new int[2] { i, i + 1 } : new int[1] { i };
        
        return new int[1] { -1 };
    }

    private Vector3 FindCellPosition(int _cell)
    {
        foreach (ObjectCell _cellObj in _cellObjs)
            if (_cellObj.cellID == _cell)
                return _cellObj.gameObject.transform.position;

        return Vector3.zero;
    }

    private ObjectItem GetCheckedCell(int _id) => _items.Find(i => i.Item.CountCell > 1 ? (i.CellsId[0] == _id || i.CellsId[1] == _id) : (i.CellsId[0] == _id));

    private void CheckCell(ObjectItem _obj, ObjectCell _cell)
    {
        if (_obj.Item.CountCell == 1)
            OnEndCheckCell?.Invoke(_items.Find(i => i.Item.CountCell > 1 ? 
            (i.CellsId[0] == _cell.cellID || i.CellsId[1] == _cell.cellID) :
            (i.CellsId[0] == _cell.cellID)), _obj);
        else
        {
            ObjectItem i_1 = _items.Find(i => i.Item.CountCell > 1 ? 
                (i.CellsId[0] == _cell.cellID || i.CellsId[1] == _cell.cellID) :
                (i.CellsId[0] == _cell.cellID));
            ObjectItem i_2 = _items.Find(i => i.Item.CountCell > 1 ? 
                (i.CellsId[0] == _cell.cellID + 1 || i.CellsId[1] == _cell.cellID + 1) :
                (i.CellsId[0] == _cell.cellID + 1));

            OnEndCheckCell?.Invoke(i_1 == null && i_2 == null ? null : (i_1 == null ? i_2 : null), _obj);
        }
    }

    private bool CheckItemID(IItem _ii, string _targetType, string _targetID = "")
    {
        char[] _listID = _ii.Id.ToCharArray();

        string _typeItem = (_listID[0] + _listID[1] + _listID[2]).ToString();
        string _numItem = "";

        for (int i = 3; i < _listID.Length; i++)
            _numItem += _listID[i];

        if (_typeItem == _targetType && _targetID != "" ? _targetID == _ii.Id : true)
            return true;

        return false;
    }

    private void SubtractionItems(ObjectItem _currentItem, int _subtractionCount)
    {
        if (_currentItem.Count - _subtractionCount > 0)
            _currentItem.Count -= _subtractionCount;
        else
        {
            _currentItem.Count = 0;
            _items.Remove(_currentItem);
            Destroy(_currentItem.gameObject);
            return;
        }

        _currentItem.UpdateValue();
    }


    protected void OnUpdateOutFit()
    {
        Dictionary<string, IItem> outfit = new Dictionary<string, IItem>();

        if (_items.Find(i => i.CellsId[0] == 101))
            outfit.Add("GUN", _items.Find(i => i.CellsId[0] == 101).Item);
        if (_items.Find(i => i.CellsId[0] == 103))
            outfit.Add("PIS", _items.Find(i => i.CellsId[0] == 103).Item);
        if (_items.Find(i => i.CellsId[0] == 104))
            outfit.Add("ARM", _items.Find(i => i.CellsId[0] == 104).Item);
        if (_items.Find(i => i.CellsId[0] == 105))
            outfit.Add("PAK", _items.Find(i => i.CellsId[0] == 105).Item);
        if (_items.Find(i => i.CellsId[0] == 106))
            outfit.Add("LIT", _items.Find(i => i.CellsId[0] == 106).Item);

        ChangeOutfit?.Invoke(outfit);
    }

    private void DragingStart(Transform _tr) => _tr.parent = _parentDrag;

    private void DragingEnd(Transform _tr, int _id) => _tr.parent = (_id >= 100 ? (_id >= 500 ? _parentBase[2] : _parentBase[1]) : _parentBase[0]);

    private void OnOpenDiscription(ObjectItem _item) => OpenDiscription?.Invoke(_item);

    protected void Save()
    {
        SaveHeandler.SessionNow.money = Money;
        SaveHeandler.SessionNow.items.Clear();
        for (int i = 0; i < _items.Count; i++)
        {
            SavesItem _si = new SavesItem();

            _si.idItem = _items[i].Item.Id;
            _si.count = _items[i].Count;
            _si.cellsId = _items[i].CellsId;
            _si.condition = _items[i].Item.Condition;
            _si.value = _items[i].Item.Value;

            SaveHeandler.SessionNow.items.Add(_si);
        }
    }

    protected void Load()
    {
        Money = SaveHeandler.SessionNow.money;
        foreach (SavesItem s in SaveHeandler.SessionNow.items)
        {
            IItem _i = _data.GetItem(s.idItem).CloneItem();

            _i.Condition = s.condition;
            _i.Value = s.value;

            AddItem(_i, s.count, _items, 0, s.cellsId);
        }
        OnUpdateOutFit();
    }
}
