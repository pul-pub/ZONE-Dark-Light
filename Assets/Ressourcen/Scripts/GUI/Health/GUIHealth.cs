using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUIHealth : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private Energy energy;
    [SerializeField] private GUIInventory inventory;
    [Header("---------  Overly  ----------")]
    [SerializeField] private List<GUIBodyParth> guiOverlyBody;
    [SerializeField] private GUIButton overlyButton;
    [Header("----------  Menu  -----------")]
    [SerializeField] private List<GUIBodyParth> guiMenuBody;
    [SerializeField] private GUIButton menuButton;
    [Header("---------  Medic  ----------")]
    [SerializeField] private List<GUIBodyParth> guiMedMenu;
    [SerializeField] private List<GUIButton> guiMedButtons;
    [SerializeField] private RectTransform medicMenu;
    [Header("---------  Value  ----------")]
    [SerializeField] private TextMeshProUGUI textMassMax;
    [SerializeField] private TextMeshProUGUI textHealthAll;
    [SerializeField] private TextMeshProUGUI textEnergyAll;
    [SerializeField] private Image vinHealth;

    private ObjectItem _medic;
    private OutFitManager _outFitManager;

    private void Awake()
    {
        _outFitManager = health.gameObject.GetComponent<OutFitManager>();
    }

    private void OnEnable()
    {
        for (int i = 0; i < health.listBodyParths.Count; i++)
        {
            health.listBodyParths[i].OnTakeDamade += guiOverlyBody[i].OnChengeHP;
            health.listBodyParths[i].OnTakeDamade += guiMenuBody[i].OnChengeHP;
            health.listBodyParths[i].OnTakeDamade += guiMedMenu[i].OnChengeHP;
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < health.listBodyParths.Count; i++)
        {
            health.listBodyParths[i].OnTakeDamade -= guiOverlyBody[i].OnChengeHP;
            health.listBodyParths[i].OnTakeDamade -= guiMenuBody[i].OnChengeHP;
            health.listBodyParths[i].OnTakeDamade -= guiMedMenu[i].OnChengeHP;
        }
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        textHealthAll.text = health.HealthAll.ToString();
        textEnergyAll.text = Math.Round(energy.Value, 2, MidpointRounding.ToEven).ToString();
        textMassMax.text = "/ " + _outFitManager.MaxMass.ToString();

        vinHealth.color = new Color(0.5f, 0, 0, 1 - health.HealthAll / 440);
    }

    public void Use(ObjectItem _item)
    {
        if (CheckItemType(_item.Item, "MED"))
            _medic = _item;
    }

    private void OpenHealthMenu()
    {

    }

    private bool CheckItemType(IItem _ii, string _targetType)
    {
        char[] _listID = _ii.Id.ToCharArray();

        string _typeItem = _listID[0].ToString() + _listID[1].ToString() + _listID[2].ToString();

        return _typeItem == _targetType;
    }

    private IEnumerator AnimationMove(RectTransform _transform, Vector3 _target, float _speed)
    {
        while (!IsTargetValue(_transform.localPosition, _target))
        {
            _transform.localPosition = Vector3.MoveTowards(_transform.localPosition, _target, _speed * Time.deltaTime * 100);
            yield return new WaitForEndOfFrame();
        }
    }

    private bool IsTargetValue(Vector3 current, Vector3 target, float range = 0.1f)
    {
        Vector3 _v = current - target;
        return (Math.Abs(_v.x) <= range && Math.Abs(_v.y) <= range && Math.Abs(_v.z) <= range);
    }
}
