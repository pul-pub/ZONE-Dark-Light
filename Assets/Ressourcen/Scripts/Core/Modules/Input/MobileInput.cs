using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MobileInput : MonoBehaviour, IInput
{
    public event Action<Vector2> Move;
    public event Action<Vector2> Viwe;
    public event Action<float> CastBolt;

    public event Action<bool> Shoot;
    public event Action Reload;
    public event Action<int> SwitchWeapon;
    public event Action SwitchLight;

    public event Action<string> PressInteractionButton;

    [SerializeField] private Image[] imgButtons;
    [Header("---------  Modules  ---------")]
    [SerializeField] private WeaponManager weapon;
    [SerializeField] private Vector3 butShootT;
    [SerializeField] private Vector3 butShootF;
    [SerializeField] private Vector3 butReloadT;
    [SerializeField] private Vector3 butReloadF;
    [Space]
    [SerializeField] private LightManager lightM;
    [SerializeField] private Vector3 butLightT;
    [SerializeField] private Vector3 butLightF;
    [Header("—————---—  Joysticks  ————---——")]
    [SerializeField] private FixedJoystick _joystickMove;
    [SerializeField] private FixedJoystick _joystickVive;
    [SerializeField] private FixedJoystick _joystickBolt;
    [Header("—————----—  Buttons  ————----——")]
    [SerializeField] private GUIButton buttonShoot;
    [SerializeField] private GUIButton buttonReload;
    [SerializeField] private GUIButton buttonSwitchLight;
    [SerializeField] private GUIButton[] buttonsNumWeapon;
    [Header("-----  Interaction Button  -----")]
    [SerializeField] private GUIButton buttonInt;
    [SerializeField] private TextMeshProUGUI buttonIntText;
    [Space]
    [SerializeField] private Vector3 butIntT;
    [SerializeField] private Vector3 butIntF;

    private RectTransform _buttonIntTr;
    private RectTransform _buttonShootTr;
    private RectTransform _buttonReloadTr;
    private RectTransform _buttonSwitchLightTr;
    private Coroutine[] _animation = new Coroutine[4];

    public void Initialization(TypeGroup _group)
    {
        _buttonIntTr = buttonInt.gameObject.GetComponent<RectTransform>();
        _buttonShootTr = buttonShoot.gameObject.GetComponent<RectTransform>();
        _buttonReloadTr = buttonReload.gameObject.GetComponent<RectTransform>();
        _buttonSwitchLightTr = buttonSwitchLight.gameObject.GetComponent<RectTransform>();

        foreach (Image img in imgButtons)
            img.color = new Color(img.color.r, img.color.g, img.color.b, SaveHeandler.Settings.alfaUi);
    }

    private void OnEnable()
    {
        _joystickMove.Pressing += OnMoveing;
        _joystickVive.Pressing += OnViwe;
        _joystickBolt.EndPress += OnCastBolt;

        buttonShoot.Clicking += OnShoot;
        buttonReload.Click += OnReload;
        buttonSwitchLight.Click += OnSwitchLight;

        buttonsNumWeapon[0].Click += OnSwitchWeapon;
        buttonsNumWeapon[1].Click += OnSwitchWeapon;
        buttonsNumWeapon[2].Click += OnSwitchWeapon;

        buttonInt.Click += OnPressInteractionButton;
    }

    private void OnDisable()
    {
        _joystickMove.Pressing -= OnMoveing;
        _joystickVive.Pressing -= OnViwe;
        _joystickMove.EndPress -= OnCastBolt;

        buttonShoot.Clicking -= OnShoot;
        buttonReload.Click -= OnReload;
        buttonSwitchLight.Click -= OnSwitchLight;

        buttonsNumWeapon[0].Click -= OnSwitchWeapon;
        buttonsNumWeapon[1].Click -= OnSwitchWeapon;
        buttonsNumWeapon[2].Click -= OnSwitchWeapon;

        buttonInt.Click -= OnPressInteractionButton;
    }

    private void Update()
    {
        if ((weapon.FlagWeapon && (weapon.CheckAbliveReload() >= 1 || weapon.CheckAbliveReload() == -1)) != (_buttonShootTr.gameObject.transform.localPosition == butShootT))
            _animation[0] = StartCoroutine(AnimationMove(_buttonShootTr, 
                (weapon.FlagWeapon && (weapon.CheckAbliveReload() >= 1 || weapon.CheckAbliveReload() == -1)) ? butShootT : butShootF, 20));
        
        if ((weapon.FlagWeapon &&  weapon.CheckAbliveReload() >= 0) != (_buttonReloadTr.gameObject.transform.localPosition == butReloadT))
            _animation[1] = StartCoroutine(AnimationMove(_buttonReloadTr, 
                (weapon.FlagWeapon && weapon.CheckAbliveReload() >= 0) ? butReloadT : butReloadF, 20));

        if (lightM.HaveLight != (buttonSwitchLight.gameObject.transform.localPosition == butLightT))
            _animation[2] = StartCoroutine(AnimationMove(_buttonSwitchLightTr, lightM.HaveLight ? butLightT : butLightF, 20));
    }

    private void OnMoveing(Vector2 _vec) => Move?.Invoke(_vec);
    private void OnCastBolt(Vector2 _force) => CastBolt?.Invoke(Math.Abs(_force.x) + Math.Abs(_force.y));
    private void OnShoot(bool _shooting) => Shoot?.Invoke(_shooting);
    private void OnReload(string _num) => Reload?.Invoke();
    private void OnSwitchLight(string _num) => SwitchLight?.Invoke();
    private void OnSwitchWeapon(string _num) => SwitchWeapon?.Invoke(int.Parse(_num));
    private void OnPressInteractionButton(string _setting) => PressInteractionButton?.Invoke(_setting);
    private void OnViwe(Vector2 _setting) => Viwe?.Invoke(_setting);

    public void OnUpdateOutFit(Dictionary<string, IItem> _items)
    {
        buttonsNumWeapon[0].gameObject.SetActive(_items.TryGetValue("GUN", out IItem _item));
        buttonsNumWeapon[0].GetComponent<Image>().sprite = _item?.Img;
        buttonsNumWeapon[1].gameObject.SetActive(_items.TryGetValue("PIS", out _item));
        buttonsNumWeapon[1].GetComponent<Image>().sprite = _item?.Img;
    }

    public void OnShowInteractionBut(string _type)
    {
        if (_type != "")
        {
            _animation[3] = StartCoroutine(AnimationMove(_buttonIntTr, butIntT, 30, true));
            buttonInt.toEndAction = _type;

            switch (_type)
            {
                case "ETR":
                    buttonIntText.text = "Ïåðåéòè";
                    break;
                case "PAK":
                    buttonIntText.text = "Îáûñêàòü";
                    break;
                case "DLG":
                    buttonIntText.text = "Ãîâîðèòü";
                    break;
            }
        }
        else
        {
            _animation[3] = StartCoroutine(AnimationMove(_buttonIntTr, butIntF, 20, true));
            buttonInt.toEndAction = "";
            buttonIntText.text = "";
        } 
    }

    private IEnumerator AnimationMove(RectTransform _transform, Vector3 _target, float _speed, bool _animScale = false)
    {
        while (!IsTargetValue(_transform.localPosition, _target))
        {
            _transform.localPosition = Vector3.MoveTowards(_transform.localPosition, _target, _speed * Time.deltaTime * 100);

            if (_animScale)
                _transform.localScale = new Vector3(
                    -((butIntF.y - (_transform.localPosition.y - butIntT.y)) / -butIntF.y),
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
