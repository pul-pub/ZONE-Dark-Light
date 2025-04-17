using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ObjectItem : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler
{
    public event Action<ObjectItem> OpenDescription;
    public event Action<ObjectItem, int[]> OpenStoreDialogScreen;
    public event Action OnUpdateAll;
    public event Action OnUpdateOutFit;
    public event Action<Transform> OnStartDraging;
    public event Action<Transform, int> OnEndDraging;
    public event Action<ObjectItem, ObjectCell> OnCheckCell;

    public IItem Item { get; set; }
    public int Count = 0;
    public int[] CellsId = new int[2];

    public string SpeshialType = "";
    
    private Vector2 _startPos;
    private bool _isDragging = false;

    private Transform _parents;
    private TextMeshProUGUI _textCount;

    private ObjectCell _targetCell;
    private int[] _currentCells = new int[2];

    private int _countClick = 0;
    private float _timer = 0;

    private void OnDestroy() => OnUpdateAll.Invoke();

    private void Update()
    {
        if (_timer >= 0)
            _timer -= Time.deltaTime;
    }

    public void Initialization(IItem _item, int[] _cells, Vector3 _pos, int _count)
    {
        RectTransform _rt = GetComponent<RectTransform>();
        _textCount = GetComponentInChildren<TextMeshProUGUI>();

        Count = _count;
        Item = _item;
        CellsId = _cells;
        
        if (Item.CountCell > 1)
        {
            transform.position = _pos + new Vector3(75, 0);
            _rt.sizeDelta = new Vector2(245, _rt.sizeDelta.y);
        }
        else
            transform.position = _pos;
        
        UpdateValue();
        _startPos = transform.position;
        OnEndDrag(null);
    }

    public void UpdateValue()
    {
        GetComponentsInChildren<Image>()[1].sprite = Item.Img;
        _textCount.text = Count > 1 ? Count.ToString() : "";
    }

    public void OnBeginDrag(PointerEventData data)
    {
        _isDragging = true;
        _startPos = transform.position;
        OnStartDraging.Invoke(transform);
    }

    public void OnDrag(PointerEventData data) => transform.position = new Vector3(data.position.x, data.position.y, 5);

    public void OnEndDrag(PointerEventData data)
    {
        _countClick = 0;

        if (_targetCell == null)
        {
            RaycastHit2D _hit = Physics2D.BoxCast(transform.position, new Vector2(4, 4), 0f, Vector2.zero, 0f);

            if (_hit)
            {
                if (_targetCell = _hit.collider.gameObject.GetComponent<ObjectCell>())
                {
                    if (CellsId.Length == 1)
                    {
                        _currentCells = new int[1] { CellsId[0] };
                        CellsId = new int[1] { -1 };
                    }
                    else
                    {
                        _currentCells = new int[2] { CellsId[0], CellsId[1] };
                        CellsId = new int[2] { -1, -1 };
                    }
                    OnCheckCell.Invoke(this, _targetCell);
                }
            }
            else
                transform.position = _startPos;
        }
    }

    public void OnPointerClick(PointerEventData data)
    {
        if (_countClick < 1 && _timer <= 0)
        {
            _countClick++;
            _timer = 0.5f;
        }
        else if (_countClick == 1 && _timer > 0)
        {
            OpenDescription.Invoke(this);
            _countClick = 0;
        }
        else
            _countClick = 0;
    }

    public void OnDrop(ObjectItem _ii, ObjectItem _main)
    {
        if (_main == this)
        {
            if ((_targetCell.targetItemType == "" || CheckItemType(Item, _targetCell.targetItemType)) &&
                (_targetCell.targetSize != -1 ? _targetCell.targetSize == Item.CountCell : true) &&
                (Item.CountCell > 1 ? (_targetCell.cellID + 1) % 7 != 0 : true))
            {
                if (_ii == null)
                {
                    CellsId = Item.CountCell > 1 ? new int[2] { _targetCell.cellID, _targetCell.cellID + 1 } : new int[1] { _targetCell.cellID };
                    transform.position = Item.CountCell > 1 ? _targetCell.transform.position + new Vector3(75, 0) : _targetCell.transform.position;

                    OnUpdateAll.Invoke();
                    if (SpeshialType == "")
                        OnEndDraging.Invoke(transform, _targetCell.cellID);
                    else if (SpeshialType == "STR")
                    {
                        transform.position = _startPos;
                        CellsId = _currentCells;
                        ObjectItem item = new ObjectItem();
                        item.Item = Item.CloneItem();
                        OpenStoreDialogScreen.Invoke(item, Item.CountCell > 1 ? 
                            new int[2] { _targetCell.cellID, _targetCell.cellID + 1 } : new int[1] { _targetCell.cellID });
                        OnEndDraging.Invoke(transform, _currentCells[0]);
                    }
                }
                else if (_ii.Item.Id == Item.Id)
                {
                    int reason;
                    if ((reason = AdditionItems(_ii, this)) == 0)
                        Destroy(gameObject);
                    else
                        Count = reason;

                    CellsId = Item.CountCell > 1 ? new int[2] { _currentCells[0], _currentCells[1] } : new int[1] { _currentCells[0] };
                    transform.position = _startPos;

                    OnUpdateAll.Invoke();
                    if (SpeshialType == "")
                        OnEndDraging.Invoke(transform, _targetCell.cellID);
                    else if (SpeshialType == "STR")
                    {
                        transform.position = _startPos;
                        CellsId = _currentCells;
                        ObjectItem item = new ObjectItem();
                        item.Item = Item.CloneItem();
                        OpenStoreDialogScreen.Invoke(item, Item.CountCell > 1 ? new int[2] { _currentCells[0], _currentCells[1] } : new int[1] { _currentCells[0] });
                        OnEndDraging.Invoke(transform, _currentCells[0]);
                    }
                }
                else
                {
                    if (_ii.Item.CountCell == Item.CountCell && (_currentCells[0] > 100 && _currentCells[0] < 200))
                    {
                        CellsId = Item.CountCell > 1 ? new int[2] { _ii.CellsId[0], _ii.CellsId[1] } : new int[1] { _ii.CellsId[0] };
                        _ii.CellsId = _ii.Item.CountCell > 1 ? new int[2] { _currentCells[0], _currentCells[1] } : new int[1] { _currentCells[0] };
                        transform.position = _ii.transform.position;
                        _ii.transform.position = _startPos;

                        OnEndDraging.Invoke(transform, CellsId[0]);
                        OnEndDraging.Invoke(_ii.transform, _ii.CellsId[0]);
                    }
                    else
                    {
                        transform.position = _startPos;
                        CellsId = _currentCells;
                        OnEndDraging.Invoke(transform, _currentCells[0]);
                    }
                } 
            }
            else if (_targetCell.targetItemType == "NPC" && CheckItemType(Item, "HBR"))
            {
                transform.position = _startPos;
                CellsId = _currentCells;
                ObjectItem item = this.MemberwiseClone() as ObjectItem;
                item.Item = Item.CloneItem();
                item.Item.MaxCount = Count;
                OpenStoreDialogScreen.Invoke(item, Item.CountCell > 1 ?
                    new int[2] { _targetCell.cellID, _targetCell.cellID + 1 } : new int[1] { _targetCell.cellID });
                OnEndDraging.Invoke(transform, _currentCells[0]);
            }
            else
            {
                transform.position = _startPos;
                CellsId = _currentCells;
                OnEndDraging.Invoke(transform, _currentCells[0]);
            }

            if ((_currentCells[0] >= 100 && _currentCells[0] <= 200) ||
                (CellsId[0] >= 100 && CellsId[0] <= 200))
                OnUpdateOutFit.Invoke();

            _isDragging = false;
            _targetCell = null;
        }
    }

    private bool CheckItemType(IItem _ii, string _targetType)
    {
        char[] _listID = _ii.Id.ToCharArray();

        string _typeItem = _listID[0].ToString() + _listID[1].ToString() + _listID[2].ToString();
        
        return _typeItem == _targetType;
    }

    private int AdditionItems(ObjectItem _currentItem, ObjectItem _targetItem)
    {
        if (_currentItem.Count + _targetItem.Count <= _currentItem.Item.MaxCount)
        {
            _currentItem.Count += _targetItem.Count;

            return 0;
        }
        else
        {
            int count = _currentItem.Count;
            _currentItem.Count = _currentItem.Item.MaxCount;

            return count + _targetItem.Count - _currentItem.Item.MaxCount;
        }
    }
}
