using System.Collections.Generic;
using UnityEngine;

public enum TypeInventory { INVENTORY, STOR };
public enum TypeReturn { TRUE, FALSE, DELETE };

public class Inventory : MonoBehaviour
{
    public event System.Action OnChangeOutfit;

    [SerializeField] private DataBase inventoryData;
    [SerializeField] private int coutCells = 49;
    [Header("Objects Item")]
    [SerializeField] private Object itemBG;
    [SerializeField] private Object objectItem;
    [Header("Parebts Item")]
    [SerializeField] private Transform parentBG;
    [SerializeField] private Transform parentObjectItem;

    public List<ObjectItem> _items = new List<ObjectItem>();
    public List<ObjectCell> _cellObjs = new List<ObjectCell>();

    public void CreateList()
    {
        for (int i = 0; i < 20; i++)
        {
            AddItem(inventoryData.items[Random.Range(0, inventoryData.items.Length)], Random.Range(1, 10));
        }

        ChengeOutfit();
    }

    public int CalculatedCountItems(ObjectItem _currentItem, ObjectItem _targetItem)
    {
        if (_currentItem.count + _targetItem.count <= _currentItem.item.maxCount)
        {
            _currentItem.count += _targetItem.count;

            return 0;
        }
        else
        {
            _currentItem.count = _currentItem.item.maxCount;

            return _currentItem.count + _targetItem.count - _currentItem.item.maxCount;
        }
    }

    private bool AddItem(Item _item, int _count)
    {
        if (_item.countCell == 1)
        {
            GameObject _gObj = Instantiate(objectItem, parentObjectItem) as GameObject;

            int[] _cells = new int[1] { FindNullCell() };

            _gObj.GetComponent<ObjectItem>().Initialization(_item, _cells, FindCellPosition(_cells[0]), this, _count);
            
            _items.Add(_gObj.GetComponent<ObjectItem>());
        }
        else
        {
            GameObject _gObj = Instantiate(objectItem, parentObjectItem) as GameObject;

            int _res = FindNullCell(true);
            int[] _cells = new int[2] { _res, _res + 1 };

            _gObj.GetComponent<ObjectItem>().Initialization(_item, _cells, FindCellPosition(_cells[0]), this, 1);

            _items.Add(_gObj.GetComponent<ObjectItem>());
        }
        return true;
    }

    private int FindNullCell(bool _duble = false)
    {
        int _cell = 0;

        if (_items.Count == 0)
            return _cell;
        
        if (_duble)
        {
            while (_cell < coutCells - 1)
            {
                if ((_cell + 1) % 7 != 0)
                {
                    bool _is = false;

                    foreach (ObjectItem _item in _items)
                    {
                        for (int i = 0; i < _item.cellsId.Length; i++)
                            if (_item.cellsId[i] == _cell)
                                _is = true;
                    }

                    if (!_is)
                        return _cell;
                }
                _cell += 1;
            }
        }
        else
        {
            while (_cell < coutCells)
            {
                bool _is = false;

                foreach (ObjectItem _item in _items)
                {
                    for (int i = 0; i < _item.cellsId.Length; i++)
                        if (_item.cellsId[i] == _cell)
                            _is = true;
                }

                if (!_is)
                    return _cell;

                _cell += 1;
            }
        }
        return -1;
    }

    public Item FindItemCell(int _cell)
    {
        foreach (ObjectItem _item in _items)
            if (_item.cellsId[0] == _cell)
                return _item.item;

        return null;
    }

    private Vector3 FindCellPosition(int _cell)
    {
        foreach (ObjectCell _cellObj in _cellObjs)
            if (_cellObj.cellID == _cell)
                return _cellObj.gameObject.transform.position;

        return Vector3.zero;
    }

    public ObjectItem CheckCell(int _cellID)
    {
        foreach (ObjectItem _itemObj in _items)
        {
            if (_itemObj.cellsId.Length == 1)
            {
                if (_itemObj.cellsId[0] == _cellID)
                {
                    Debug.Log(_itemObj.cellsId[0]);
                    return _itemObj;
                }
                    
            }
            else
            {
                if (_itemObj.cellsId[0] == _cellID || _itemObj.cellsId[1] == _cellID)
                    return _itemObj;
            }
        }

        return null;
    }

    public void ChengeOutfit()
    {
        if (OnChangeOutfit != null)
            OnChangeOutfit.Invoke();
    }
}
