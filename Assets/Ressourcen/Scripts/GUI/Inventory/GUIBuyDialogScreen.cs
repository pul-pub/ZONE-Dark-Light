using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUIBuyDialogScreen : MonoBehaviour
{
    public event Action<ObjectItem, int[]> AddItem;

    [SerializeField] private GameObject screen;
    [Space]
    [SerializeField] private RectTransform buyDialog;
    [SerializeField] private Vector3 screenPosClose;
    [SerializeField] private Vector3 screenPosOpen;
    [Space]
    [SerializeField] private Slider buySlider;
    [SerializeField] private TextMeshProUGUI countItem;
    [SerializeField] private TextMeshProUGUI moneyBuy;
    [SerializeField] private TextMeshProUGUI nameItem;

    private ObjectItem _itemCurrent;
    private int[] _targetCell;
    private Coroutine _anim;

    private void Awake()
    {
        buySlider.onValueChanged.AddListener(OnSetCount);
    }

    public void SetDialogScreen(ObjectItem _item, int[] _target)
    {
        _itemCurrent = _item;
        _targetCell = _target;

        screen.SetActive(true);
        if (_anim == null)
            _anim = StartCoroutine(AnimationMove(buyDialog, screenPosOpen, 40, true));
        else
        {
            StopCoroutine(_anim);
            _anim = null;
            StartCoroutine(AnimationMove(buyDialog, screenPosOpen, 40, true));
        }

        nameItem.text = _item.Item.Name;
        buySlider.maxValue = _item.Item.MaxCount;
        buySlider.value = 1;
        countItem.text = "x1";
        moneyBuy.text = _itemCurrent.Item.Price.ToString() + " руб";
    }

    public void OnSetCount(float value)
    {
        countItem.text = "x" + ((int)value).ToString();
        moneyBuy.text = ((int)value * _itemCurrent.Item.Price).ToString() + " руб";
    }

    public void OnApply()
    {
        _itemCurrent.Count = (int)buySlider.value;
        AddItem?.Invoke(_itemCurrent, _targetCell);
        OnReject();
    }

    public void OnReject()
    {
        screen.SetActive(false);
        if (_anim == null)
            _anim = StartCoroutine(AnimationMove(buyDialog, screenPosClose, 40, true));
        else
        {
            StopCoroutine(_anim);
            _anim = null;
            StartCoroutine(AnimationMove(buyDialog, screenPosClose, 40, true));
        }
    }

    private IEnumerator AnimationMove(RectTransform _transform, Vector3 _target, float _speed, bool _animScale = false)
    {
        while (!IsTargetValue(_transform.localPosition, _target))
        {
            _transform.localPosition = Vector3.MoveTowards(_transform.localPosition, _target, _speed * Time.deltaTime * 100);

            if (_animScale)
                _transform.localScale = new Vector3(
                    -((screenPosClose.y - (_transform.localPosition.y - screenPosOpen.y)) / -screenPosClose.y),
                    _transform.localScale.y, _transform.localScale.z);

            yield return new WaitForEndOfFrame();
        }
    }

    private bool IsTargetValue(Vector3 current, Vector3 target, float range = 0.1f)
    {
        Vector3 _v = current - target;
        return (Mathf.Abs(_v.x) <= range && Mathf.Abs(_v.y) <= range && Mathf.Abs(_v.z) <= range);
    }
}
