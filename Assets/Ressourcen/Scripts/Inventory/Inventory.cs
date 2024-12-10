using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public event System.Action OnChangeOutfit;

    [SerializeField] private DataBase inventoryData;
    [SerializeField] private int coutCells = 49;
    [Header("Objects Item")]
    [SerializeField] private Object objectItem;
    [Header("Parebts Item")]
    [SerializeField] private Transform[] parentObjectItem;
    [SerializeField] private Transform[] parentObjectItem_DRAG;

    public List<ObjectItem> _items = new List<ObjectItem>();
    public List<ObjectItem> _itemsNPC = new List<ObjectItem>();
    private NPC _npcPack;
    public List<ObjectCell> _cellObjs = new List<ObjectCell>();

    private List<ObjectItem> _ammo = new List<ObjectItem>();
    public float allMass = 0;

    public void CreateList()
    {
        for (int i = 0; i < 20; i++)
        {
            AddItem(inventoryData.items[Random.Range(0, inventoryData.items.Length)], Random.Range(1, 128), _items);
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

    private bool AddItem(Item _item, int _count, List<ObjectItem> _list, bool _npc = false)
    {
        if (_item.countCell == 1)
        {
            GameObject _gObj = Instantiate(objectItem, parentObjectItem[_npc ? 1 : 0]) as GameObject;

            int[] _cells = new int[1] { FindNullCell(false, _npc) };

            _gObj.GetComponent<ObjectItem>().Initialization(
                _item.Clone(),
                _cells, FindCellPosition(_cells[0]),
                this,
                _count < _item.maxCount ? _count : _item.maxCount,
                parentObjectItem,
                parentObjectItem_DRAG);

            _list.Add(_gObj.GetComponent<ObjectItem>());
        }
        else
        {
            GameObject _gObj = Instantiate(objectItem, parentObjectItem[_npc ? 1 : 0]) as GameObject;

            int _res = FindNullCell(true, _npc);
            int[] _cells = new int[2] { _res, _res + 1 };

            _gObj.GetComponent<ObjectItem>().Initialization(
                _item.Clone(),
                _cells, FindCellPosition(_cells[0]),
                this,
                _count < _item.maxCount ? _count : _item.maxCount,
                parentObjectItem,
                parentObjectItem_DRAG);

            _list.Add(_gObj.GetComponent<ObjectItem>());
        }

        CalculatedMass();

        return true;
    }

    private int FindNullCell(bool _duble = false, bool _npcBackpack = false)
    {
        int _cell = (_npcBackpack ? 500 : 0);

        if (_items.Count == 0)
            return _cell;
        
        if (_duble)
        {
            while (_cell < (_npcBackpack ? 500 + coutCells : coutCells) - 1)
            {
                if ((_cell + 1) % 7 != 0)
                {
                    bool _is = false;

                    foreach (ObjectItem _item in (_npcBackpack ? _itemsNPC : _items))
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
            while (_cell < (_npcBackpack ? 500 + coutCells : coutCells))
            {
                bool _is = false;

                foreach (ObjectItem _item in (_npcBackpack ? _itemsNPC : _items))
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

    public int GetCountAmmos(Gun _gun)
    {
        _ammo.Clear();
        int ammos = 0;

        foreach (ObjectItem item in _items)
        {
            if (item.item.typeAmmo == _gun.typeAmmo && item.item.type == TypeItem.Ammo)
            {
                ammos += item.count;

                _ammo.Add(item);
            }
        }

        return ammos;
    }

    public void SetCountAmmo(int _ammoForReload)
    {
        int _ammos = _ammoForReload;
        for (int i = 0; i < _ammo.Count; i++)
        {
            if (_ammo[i] != null)
            {
                if (_ammo[i].count == _ammos)
                {
                    _items.Remove(_ammo[i]);
                    Destroy(_ammo[i].gameObject);
                    break;
                }
                else if (_ammo[i].count < _ammos)
                {
                    _ammos -= _ammo[i].count;
                    _items.Remove(_ammo[i]);
                    Destroy(_ammo[i].gameObject);
                }
                else if (_ammo[i].count > _ammos)
                {
                    _ammo[i].count = _ammo[i].count - _ammos;
                    _ammo[i].UpdateValue();
                    break;
                }
            }
        }

        CalculatedMass();
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

    public void SetActivOutfit(bool _activ)
    {
        for (int i = 0; i < _items.Count; i++)
        {
            if (_items[i].cellsId[0] >= 100)
                _items[i].gameObject.SetActive(_activ);
        }
    }

    public void OnBackpackNPC(NPC _npc)
    {
        for (int i = 0; i < _itemsNPC.Count; i++)
            Destroy(_itemsNPC[i].gameObject);

        _itemsNPC.Clear();
        _npcPack = _npc;

        for (int i = 0; i < _npc.backpack.items.Count; i++)
        {
            if (_npc.backpack.items[i] != null)
                AddItem(_npc.backpack.items[i], _npc.backpack.count[i], _itemsNPC, true);
        }
    }
    public void GiveAllItem()
    {
        for (int i = 0; i < _npcPack.backpack.items.Count; i++)
        {
            if (_npcPack.backpack.items[i] != null)
                AddItem(_npcPack.backpack.items[i], _npcPack.backpack.count[i], _items);
        }

        for (int i = 0; i < _itemsNPC.Count; i++)
            Destroy(_itemsNPC[i].gameObject);

        _itemsNPC.Clear();
        _npcPack.backpack.NullCountPack();
    }

    public void ChengeOutfit()
    {
        if (OnChangeOutfit != null)
            OnChangeOutfit.Invoke();
    }

    public void ChengeNPCBackpack()
    {
        for (int i = 0; i < _itemsNPC.Count; i++)
        {
            if (_itemsNPC[i] != null)
            {
                if (_itemsNPC[i].cellsId[0] < 500)
                {
                    _items.Add(_itemsNPC[i]);
                    foreach (Item _ii in _npcPack.backpack.items)
                    {
                        if (_ii.id == _itemsNPC[i].item.id)
                        {
                            _npcPack.backpack.items.Remove(_ii);
                            break;
                        }
                    }    
                    _itemsNPC.Remove(_itemsNPC[i]);

                    if (_npcPack.backpack.items.Count <= 0)
                    {
                        _npcPack.backpack.NullCountPack();
                        break;
                    }
                }
            }
        }
    }

    private void CalculatedMass()
    {
        allMass = 0;

        foreach (ObjectItem _itemObj in _items)
            if (_itemObj != null)
                allMass += _itemObj.item.weight * _itemObj.count;
    }
}
