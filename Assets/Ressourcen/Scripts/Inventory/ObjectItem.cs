using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ObjectItem : EventTrigger
{
    public Item item { get; private set; }
    public int count = 0;
    public int[] cellsId = new int[2];
    
    private Vector2 objectStartPos;
    private bool isDragging = false;

    private Inventory _inventory;
    private Transform[] _baseParent;
    private Transform[] _dragParent;
    private RectTransform _rt;
    private TextMeshProUGUI _tmp;

    private GameObject _to;
    private int[] _currentCells = new int[2];

    private int countClick = 0;

    public void Initialization(Item _item, int[] _cells, Vector3 _pos, Inventory _inv, int _count, Transform[] _base, Transform[] _drag)
    {
        _rt = GetComponent<RectTransform>();
        _tmp = GetComponentInChildren<TextMeshProUGUI>();

        _baseParent = _base;
        _dragParent = _drag;   

        count = _count;
        item = _item;
        cellsId = _cells;
        _inventory = _inv;
        
        transform.parent = _baseParent[cellsId[0] < 500 ? 0 : 1];

        if (item.countCell != 1)
        {
            transform.position = _pos + new Vector3(62, 0);
            _rt.sizeDelta = new Vector2(245, _rt.sizeDelta.y);
        }
        else
            transform.position = _pos;

        UpdateValue();
    }

    public void UpdateValue()
    {
        GetComponentsInChildren<Image>()[1].sprite = item.img;
        if (count > 1)
            _tmp.text = count.ToString();
        else
            _tmp.text = "";
    }

    public override void OnBeginDrag(PointerEventData data)
    {
        isDragging = true;
        objectStartPos = transform.position;
        transform.parent = _dragParent[cellsId[0] < 500 ? 0 : 1];
    }

    public override void OnDrag(PointerEventData data) => transform.position = new Vector3(data.position.x, data.position.y, 5);

    public override void OnEndDrag(PointerEventData data)
    {
        countClick = 0;

        if (_to == null)
        {
            RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, new Vector2(2, 2), 0f, Vector2.zero, 0f);

            if (hits.Length > 0)
            {
                foreach (RaycastHit2D _hit in hits)
                {
                    if (_hit.collider.gameObject.CompareTag("Item"))
                    {
                        _currentCells = cellsId;
                        
                        if (cellsId.Length == 1)
                            cellsId[0] = -1;
                        else
                        {
                            cellsId[0] = -1;
                            cellsId[1] = -1;
                        }

                        _to = _hit.collider.gameObject;
                        
                        OnDrop(int.Parse(_to.name));
                    }
                }
            }
            else
                transform.position = objectStartPos;
        }

        transform.parent = _baseParent[cellsId[0] < 500 ? 0 : 1];
    }

    private void OnDrop(int _cell)
    {
        if (_cell < 49 || _cell > 100)
        {
            if (_cell < 49)
            {
                if (item.countCell == 1)
                {
                    ObjectItem _ii = _inventory.CheckCell(_cell);

                    if (_ii == null)
                    {
                        objectStartPos = _to.transform.position;

                        _currentCells[0] = int.Parse(_to.name);
                        UpdateValue();
                    }
                    else if (_ii.item.id == item.id)
                    {
                        if (_inventory.CalculatedCountItems(_ii, this) > 0)
                        {
                            _ii.UpdateValue();
                        }
                        else
                        {
                            if (item.id > 100)
                                _inventory.ChengeOutfit();

                            _ii.UpdateValue();
                            Destroy(gameObject);
                        }
                    }
                }
                else if (item.countCell == 2 && (_cell + 1) % 7 != 0)
                {
                    ObjectItem _ii_1 = _inventory.CheckCell(_cell);
                    ObjectItem _ii_2 = _inventory.CheckCell(_cell + 1);

                    if (_ii_1 == null && _ii_2 == null)
                    {
                        objectStartPos = _to.transform.position + new Vector3(75, 0);

                        _currentCells[0] = int.Parse(_to.name);
                        _currentCells[1] = int.Parse(_to.name) + 1;
                    }
                    else if (_ii_1 == _ii_2 && _ii_1.item.id == item.id && item.type != TypeItem.Weapon)
                    {
                        if (_inventory.CalculatedCountItems(_ii_1, this) > 0)
                        {
                            _ii_1.UpdateValue();
                        }
                        else
                        {
                            if (item.id > 100)
                                _inventory.ChengeOutfit();

                            _ii_1.UpdateValue();
                            Destroy(gameObject);
                        }
                    }
                }

                if (_currentCells[0] > 100)
                    _inventory.ChengeOutfit();
            }
            else if (_cell > 100 && _cell < 120)
            {
                ObjectItem _ii_1 = _inventory.CheckCell(_cell);

                if (_cell == 101 && item.type == TypeItem.Weapon && item.countCell == 2)
                {
                    ObjectItem _ii_2 = _inventory.CheckCell(_cell + 1);

                    if (_ii_1 != null && _ii_2 != null)
                    {
                        _ii_1.cellsId[0] = _currentCells[0];
                        _ii_1.cellsId[1] = _currentCells[1];

                        _ii_1.transform.position = objectStartPos;
                    }

                    objectStartPos = _to.transform.position + new Vector3(85, 0);
                    _currentCells[0] = int.Parse(_to.name);
                    _currentCells[1] = int.Parse(_to.name) + 1;
                    _inventory.ChengeOutfit();
                }
                else if (_cell == 103 && item.type == TypeItem.Weapon && item.countCell == 1)
                {
                    if (_ii_1 != null)
                    {
                        _ii_1.cellsId[0] = _currentCells[0];

                        _ii_1.transform.position = objectStartPos;
                    }

                    objectStartPos = _to.transform.position;
                    _currentCells[0] = int.Parse(_to.name);
                    _inventory.ChengeOutfit();
                }
                else if ((_cell == 104 && item.type == TypeItem.Armor) ||
                         (_cell == 105 && item.type == TypeItem.Backpack) ||
                         (_cell == 106 && item.type == TypeItem.PNV) ||
                         (_cell == 107 && item.type == TypeItem.HeadArmor) )
                {
                    if (_ii_1 != null)
                    {
                        _ii_1.cellsId[0] = _currentCells[0];

                        _ii_1.transform.position = objectStartPos;
                    }

                    objectStartPos = _to.transform.position;
                    _currentCells[0] = int.Parse(_to.name);
                    _inventory.ChengeOutfit();
                }
            }
        }

        if (item.id >= 80 && item.id <= 90)
            _inventory.ChengeNPCBackpack();
        if (item.id > 100)
            _inventory.ChengeOutfit();

        transform.position = objectStartPos;
        cellsId = _currentCells;
        isDragging = false;
        _to = null;
    }

    public override void OnPointerClick(PointerEventData data)
    {
        if (countClick >= 1)
        {
            _inventory.OpenDiscription(this);
            countClick = 0;
        }
        else
            countClick++;
    }
}
