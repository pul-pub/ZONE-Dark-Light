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
    [SerializeField] private DataBase data;
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
    [SerializeField] private Vector3 screenPosClose;
    [SerializeField] private Vector3 screenPosOpen;
    [Header("---------  Value  ----------")]
    [SerializeField] private TextMeshProUGUI textMassMax;
    [SerializeField] private TextMeshProUGUI textHealthAll;
    [SerializeField] private TextMeshProUGUI textEnergyAll;
    [SerializeField] private Image vinHealth;

    private ObjectItem _iitem;
    private MedicObject _medic;
    private OutFitManager _outFitManager;
    private Coroutine _anim;
    private bool _load = true;

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
            guiMedMenu[i].col = health.listBodyParths[i];
            guiMedMenu[i].OnUse += SetHP;
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < health.listBodyParths.Count; i++)
        {
            health.listBodyParths[i].OnTakeDamade -= guiOverlyBody[i].OnChengeHP;
            health.listBodyParths[i].OnTakeDamade -= guiMenuBody[i].OnChengeHP;
            health.listBodyParths[i].OnTakeDamade -= guiMedMenu[i].OnChengeHP;
            guiMedMenu[i].OnUse -= SetHP;
        }
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        if (_load)
        {
            for (int i = 0; i < health.listBodyParths.Count; i++)
            {
                guiOverlyBody[i].OnChengeHP(health.listBodyParths[i].BodyParth, null);
                guiMenuBody[i].OnChengeHP(health.listBodyParths[i].BodyParth, null);
                guiMedMenu[i].OnChengeHP(health.listBodyParths[i].BodyParth, null);
            }
            _load = false;
        }

        textHealthAll.text = health.HealthAll.ToString();
        textEnergyAll.text = Math.Round(energy.Value, 2, MidpointRounding.ToEven).ToString();
        textMassMax.text = "/ " + _outFitManager.MaxMass.ToString();

        vinHealth.color = new Color(0.5f, 0, 0, 1 - health.HealthAll / 440);
    }

    public void Use(ObjectItem _item)
    {
        if (CheckItemType(_item.Item, "MED"))
        {
            _medic = data.GetMedics(_item.Item.Id);
            _iitem = _item;
            SetActivHealthMenu(true);
        }
    }

    public void SetActivHealthMenu(bool _activ)
    {
        if (_anim == null)
            _anim = StartCoroutine(AnimationMove(medicMenu, _activ ? screenPosOpen : screenPosClose, 40, true));
        else
        {
            StopCoroutine(_anim);
            _anim = null;
            _anim = StartCoroutine(AnimationMove(medicMenu, _activ ? screenPosOpen : screenPosClose, 40, true));
        }
    }

    private void SetHP(BodyParthColider _col)
    {
        Debug.Log(_col.gameObject.name);
        if (_medic && _col.ApplyMedic(_medic.RecoveryHP))
        {
            if (_iitem.Count - 1 > 0)
                _iitem.Count--;
            else
            {
                Destroy(_iitem.gameObject);
                _medic = null;
            }  
        }
    }

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
