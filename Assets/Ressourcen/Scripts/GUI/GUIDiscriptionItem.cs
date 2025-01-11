using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUIDiscriptionItem : MonoBehaviour
{
    public event Action<ObjectItem> OnUse;

    [SerializeField] private GameObject screen;
    [Space]
    [SerializeField] private Image imgItem;
    [SerializeField] private GameObject butUse;
    [SerializeField] private TextMeshProUGUI textCount;
    [SerializeField] private TextMeshProUGUI textCondition;
    [Space]
    [SerializeField] private TextMeshProUGUI textName;
    [SerializeField] private TextMeshProUGUI textBaseCharecteristic;
    [Space]
    [SerializeField] private UnityEngine.Object objCharecterisctic;
    [SerializeField] private UnityEngine.Object objDiscription;
    [SerializeField] private RectTransform parent;

    private ObjectItem _curentItem;

    public void SetDiscription(ObjectItem _item)
    {
        screen.SetActive(true);

        Transform[] _list = parent.gameObject.GetComponentsInChildren<Transform>();

        for (int i = 0; i < _list.Length; i++)
            if (!_list[i].CompareTag("MainCamera"))
                Destroy(_list[i].gameObject);

        if (_item.item.countCell == 1)
            imgItem.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 300);
        else
            imgItem.GetComponent<RectTransform>().sizeDelta = new Vector2(600, 300);
        imgItem.sprite = _item.item.img;
        textCount.text = "x" + _item.count;
        if (_item.item.armorObject)
            textCondition.text = _item.item.armorObject.Condition + "%";
        else if (_item.item.gunObject)
            textCondition.text = _item.item.gunObject.condition + "%";
        else
            textCondition.text = "100%";

        _curentItem = _item;

        if (_item.item.detectorObject)
            butUse.SetActive(true);
        else
            butUse.SetActive(false);

        textName.text = _item.item.Name;
        textBaseCharecteristic.text = _item.item.type.ToString() + "\n" + "Сталкер" + "\n" + _item.item.weight;

        if (_item.item.gunObject)
        {
            for (int i = 0; i < 5; i++)
            {
                GameObject _gObj = Instantiate(objCharecterisctic, parent) as GameObject;

                switch (i)
                {
                    case 0:
                        _gObj.GetComponentsInChildren<TextMeshProUGUI>()[0].text = "Урон";
                        _gObj.GetComponentsInChildren<TextMeshProUGUI>()[1].text = _item.item.gunObject.dm.ToString();
                        break;

                    case 1:
                        _gObj.GetComponentsInChildren<TextMeshProUGUI>()[0].text = "Скорострельность";
                        _gObj.GetComponentsInChildren<TextMeshProUGUI>()[1].text = ((1 - _item.item.gunObject.startTimeBtwShot) * 100).ToString();
                        break;

                    case 2:
                        _gObj.GetComponentsInChildren<TextMeshProUGUI>()[0].text = "Магазина";
                        _gObj.GetComponentsInChildren<TextMeshProUGUI>()[1].text = _item.item.gunObject.currentAmmos + "/" + _item.item.gunObject.ammo;
                        break;

                    case 3:
                        _gObj.GetComponentsInChildren<TextMeshProUGUI>()[0].text = "Шанс клина";
                        _gObj.GetComponentsInChildren<TextMeshProUGUI>()[1].text = ((1 - (_item.item.gunObject.condition * 0.01f)) * 100) + "%";
                        break;

                    case 4:
                        _gObj.GetComponentsInChildren<TextMeshProUGUI>()[0].text = "Тип боеприпаса";
                        _gObj.GetComponentsInChildren<TextMeshProUGUI>()[1].text = _item.item.gunObject.typeAmmo.ToString();
                        break;

                }

                parent.sizeDelta = new Vector2(0, 400 + 100 * i);
                _gObj.GetComponent<RectTransform>().localPosition -= new Vector3(0, 100 * i, 0);
            }
        }
        else if (_item.item.armorObject)
        {
            for (int i = 0; i < 5; i++)
            {
                GameObject _gObj = Instantiate(objCharecterisctic, parent) as GameObject;

                switch (i)
                {
                    case 0:
                        _gObj.GetComponentsInChildren<TextMeshProUGUI>()[0].text = "Пулестойкость";
                        _gObj.GetComponentsInChildren<TextMeshProUGUI>()[1].text = _item.item.armorObject.AntiBullet.ToString();
                        break;

                    case 1:
                        _gObj.GetComponentsInChildren<TextMeshProUGUI>()[0].text = "Защика от радиации";
                        _gObj.GetComponentsInChildren<TextMeshProUGUI>()[1].text = _item.item.armorObject.AntiRadiation.ToString();
                        break;

                    case 2:
                        _gObj.GetComponentsInChildren<TextMeshProUGUI>()[0].text = "Химичиская защита";
                        _gObj.GetComponentsInChildren<TextMeshProUGUI>()[1].text = _item.item.armorObject.AntiChimical.ToString();
                        break;

                    case 3:
                        _gObj.GetComponentsInChildren<TextMeshProUGUI>()[0].text = "Биологическая защита";
                        _gObj.GetComponentsInChildren<TextMeshProUGUI>()[1].text = _item.item.armorObject.AntiBio.ToString();
                        break;

                    case 4:
                        _gObj.GetComponentsInChildren<TextMeshProUGUI>()[0].text = "Переносимый вес";
                        _gObj.GetComponentsInChildren<TextMeshProUGUI>()[1].text = _item.item.armorObject.massUp + "КГ";
                        break;

                }

                parent.sizeDelta = new Vector2(0, 400 + 100 * i);
                _gObj.GetComponent<RectTransform>().localPosition -= new Vector3(0, 100 * i, 0);
            }
        }
        else if (_item.item.lightObject)
        {
            GameObject _gObj = Instantiate(objCharecterisctic, parent) as GameObject;

            _gObj.GetComponentsInChildren<TextMeshProUGUI>()[0].text = "Заряд";
            _gObj.GetComponentsInChildren<TextMeshProUGUI>()[1].text = _item.item.lightObject.change + "%";

            parent.sizeDelta = new Vector2(0, 400 + 100);
        }

        GameObject _ggObj = Instantiate(objDiscription, parent) as GameObject;
        _ggObj.GetComponentsInChildren<TextMeshProUGUI>()[0].text = _item.item.Discription;
        parent.sizeDelta += new Vector2(0, 250);
        _ggObj.GetComponent<RectTransform>().localPosition -= new Vector3(0, parent.sizeDelta.y - 125, 0);
    }

    public void Use() => OnUse?.Invoke(_curentItem);
}
