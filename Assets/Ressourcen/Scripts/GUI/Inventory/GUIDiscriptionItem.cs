using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUIDiscriptionItem : MonoBehaviour
{
    public event Action<ObjectItem> OnUse;

    [SerializeField] private DataBase data;
    [Header("-----------  GUI  -----------")]
    [SerializeField] private RectTransform screen;
    [SerializeField] private Vector3 screenPosOpen;
    [SerializeField] private Vector3 screenPosClose;
    [Space]
    [SerializeField] private Image imgItem;
    [SerializeField] private GameObject buttonUse;
    [SerializeField] private TextMeshProUGUI textCount;
    [SerializeField] private TextMeshProUGUI textCondition;
    [Space]
    [SerializeField] private TextMeshProUGUI textName;
    [SerializeField] private TextMeshProUGUI textBaseCharecteristic;
    [Space]
    [SerializeField] private RectTransform parent;
    [SerializeField] private UnityEngine.Object objCharecterisctic;
    [SerializeField] private UnityEngine.Object objDiscription;
    

    private ObjectItem _curentItem;

    public void OnOpenDiscription(ObjectItem _item)
    {
        StartCoroutine(AnimationMove(screen, screenPosOpen, 70, true));

        _curentItem = _item;

        imgItem.GetComponent<RectTransform>().sizeDelta = _item.Item.CountCell == 1 ? new Vector2(300, 300) : new Vector2(600, 300);
        imgItem.sprite = _item.Item.Img;
        textCount.text = "x" + _item.Count;
        textCondition.text = _item.Item.Condition + "%";
        buttonUse.SetActive(CheckItemType(_item.Item, "DTR") || CheckItemType(_item.Item, "MED"));

        Transform[] _list = screen.gameObject.GetComponentsInChildren<Transform>();
        for (int i = 0; i < _list.Length; i++)
            if (!_list[i].CompareTag("MainCamera"))
                Destroy(_list[i].gameObject);
        parent.sizeDelta = new Vector2(0, 400);

        char[] _listID = _item.Item.Id.ToCharArray();
        string _typeItem = _listID[0].ToString() + _listID[1].ToString() + _listID[2].ToString();
        textName.text = _item.Item.Name;
        textBaseCharecteristic.text = _typeItem + "\n" + "Сталкер" + "\n" + _item.Item.Weight;

        Dictionary<string, string> dict = null;

        if (CheckItemType(_item.Item, "GUN"))
            dict = data.GetGun(_item.Item.Id).DiscriptionItem;
        else if (CheckItemType(_item.Item, "ARM"))
            dict = data.GetArmor(_item.Item.Id).DiscriptionItem;
        else if (CheckItemType(_item.Item, "LIT"))
            dict = data.GetLight(_item.Item.Id).DiscriptionItem;
        else if (CheckItemType(_item.Item, "MED"))
            dict = data.GetMedics(_item.Item.Id).DiscriptionItem;
        else if (CheckItemType(_item.Item, "DET"))
            dict = data.GetDetector(_item.Item.Id).DiscriptionItem;
        else if (CheckItemType(_item.Item, "PCK"))
            dict = data.GetBackpack(_item.Item.Id).DiscriptionItem;
        else if (CheckItemType(_item.Item, "ART"))
            dict = data.GetArtifact(_item.Item.Id).DiscriptionItem;

        if (dict != null)
        {
            int i = 0;
            foreach (string key in dict.Keys)
            {
                GameObject _gObj = Instantiate(objCharecterisctic, parent) as GameObject;

                _gObj.GetComponentsInChildren<TextMeshProUGUI>()[0].text = key;
                _gObj.GetComponentsInChildren<TextMeshProUGUI>()[1].text = dict[key];

                parent.sizeDelta = new Vector2(0, 400 + 100 * i);
                _gObj.GetComponent<RectTransform>().localPosition -= new Vector3(0, 100 * i, 0);

                i++;
            }
        }

        GameObject _ggObj = Instantiate(objDiscription, parent) as GameObject;
        _ggObj.GetComponentsInChildren<TextMeshProUGUI>()[0].text = _item.Item.Discription;
        parent.sizeDelta += new Vector2(0, 250);
        _ggObj.GetComponent<RectTransform>().localPosition -= new Vector3(0, parent.sizeDelta.y - 125, 0);
    }

    public void CloaseDiscription() => StartCoroutine(AnimationMove(screen, screenPosClose, 70, true));

    public void Use() => OnUse?.Invoke(_curentItem);

    private bool CheckItemType(IItem _ii, string _targetType)
    {
        char[] _listID = _ii.Id.ToCharArray();

        string _typeItem = _listID[0].ToString() + _listID[1].ToString() + _listID[2].ToString();

        return _typeItem == _targetType;
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
        return (Math.Abs(_v.x) <= range && Math.Abs(_v.y) <= range && Math.Abs(_v.z) <= range);
    }
}
