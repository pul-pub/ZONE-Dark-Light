using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class ObjectItem : EventTrigger
{
    public event Action<ObjectItem, int> OnEndDragItem;

    public Item item { get; private set; }
    public int count = 0;
    public int[] cellsId = new int[2];
    
    private Vector2 objectStartPos;
    private bool isDragging = false;

    private Inventory _inventory;
    private RectTransform _rt;
    private TextMeshProUGUI _tmp;

    private GameObject _to;
    private int[] _currentCells = new int[2];

    public void Initialization(Item _item, int[] _cells, Vector3 _pos, Inventory _inv, int _count)
    {
        _rt = GetComponent<RectTransform>();
        _tmp = GetComponentInChildren<TextMeshProUGUI>();

        count = _count;
        item = _item;
        cellsId = _cells;
        _inventory = _inv;

        if (item.countCell != 1)
        {
            transform.position = _pos + new Vector3(62, 0);
            _rt.sizeDelta = new Vector2(245, _rt.sizeDelta.y);
        }
        else
            transform.position = _pos;

        UpdateValue();
    }

    public override void OnBeginDrag(PointerEventData data)
    {
        isDragging = true;
        objectStartPos = transform.position;
    }

    public override void OnDrag(PointerEventData data) => transform.position = new Vector3(data.position.x, data.position.y, 5);

    public override void OnEndDrag(PointerEventData data)
    {
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
                    else if (_ii.item == item)
                    {
                        if (_inventory.CalculatedCountItems(_ii, this) > 0)
                        {
                            _ii.UpdateValue();
                        }
                        else
                        {
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
                    else if (_ii_1 == _ii_2 && _ii_1.item == item)
                    {
                        if (_inventory.CalculatedCountItems(_ii_1, this) > 0)
                        {
                            _ii_1.UpdateValue();
                        }
                        else
                        {
                            _ii_1.UpdateValue();
                            Destroy(gameObject);
                        }
                    }
                }
            }
            else if (_cell > 100 && _cell < 120)
            {

            }
        }

        transform.position = objectStartPos;
        cellsId = _currentCells;
        isDragging = false;
        _to = null;
    }

    public void UpdateValue()
    {
        GetComponentsInChildren<Image>()[1].sprite = item.img;
        if (count > 1)
            _tmp.text = count.ToString();
        else
            _tmp.text = "";
    }
}
