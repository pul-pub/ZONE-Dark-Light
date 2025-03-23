using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUIHandlerInput : MonoBehaviour
{
    public event Action<Vector2> Move;
    public event Action<float> CastBolt;

    public event Action<bool> Shoot;
    public event Action Reload;
    public event Action<int> SwitchWeapon;
    public event Action SwitchLight;

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
    [SerializeField] private FixedJoystick _joystickBolt;
    [Header("—————----—  Buttons  ————----——")]
    [SerializeField] private GUIButton buttonShoot;
    [SerializeField] private GUIButton buttonReload;
    [SerializeField] private GUIButton buttonSwitchLight;
    [SerializeField] private GUIButton[] buttonsNumWeapon;

    private RectTransform _buttonShootTr;
    private RectTransform _buttonReloadTr;
    private RectTransform _buttonSwitchLightTr;
    private Coroutine[] _animation = new Coroutine[3];

    public void Initialization(TypeGroup _group)
    {
        _buttonShootTr = buttonShoot.gameObject.GetComponent<RectTransform>();
        _buttonReloadTr = buttonReload.gameObject.GetComponent<RectTransform>();
        _buttonSwitchLightTr = buttonSwitchLight.gameObject.GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        _joystickMove.Pressing += OnMoveing;
        _joystickBolt.EndPress += OnCastBolt;

        buttonShoot.Clicking += OnShoot;
        buttonReload.Click += OnReload;
        buttonSwitchLight.Click += OnSwitchLight;

        buttonsNumWeapon[0].Click += OnSwitchWeapon;
        buttonsNumWeapon[1].Click += OnSwitchWeapon;
        buttonsNumWeapon[2].Click += OnSwitchWeapon;
    }

    private void OnDisable()
    {
        _joystickMove.Pressing -= OnMoveing;
        _joystickMove.EndPress -= OnCastBolt;

        buttonShoot.Clicking -= OnShoot;
        buttonReload.Click -= OnReload;
        buttonSwitchLight.Click -= OnSwitchLight;

        buttonsNumWeapon[0].Click -= OnSwitchWeapon;
        buttonsNumWeapon[1].Click -= OnSwitchWeapon;
        buttonsNumWeapon[2].Click -= OnSwitchWeapon;
    }

    private void Update()
    {
        if ((weapon.FlagWeapon && (weapon.CheckAbliveReload() >= 1 || weapon.CheckAbliveReload() == -1)) != (_buttonShootTr.gameObject.transform.localPosition == butShootT))
            _animation[0] = StartCoroutine(AnimationMove(_buttonShootTr,
                (weapon.FlagWeapon && (weapon.CheckAbliveReload() >= 1 || weapon.CheckAbliveReload() == -1)) ? butShootT : butShootF, 20));

        if ((weapon.FlagWeapon && weapon.CheckAbliveReload() >= 0) != (_buttonReloadTr.gameObject.transform.localPosition == butReloadT))
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

    public void OnUpdateOutFit(Dictionary<string, IItem> _items)
    {
        buttonsNumWeapon[0].gameObject.SetActive(_items.TryGetValue("GUN", out IItem _item));
        buttonsNumWeapon[0].GetComponent<Image>().sprite = _item?.Img;
        buttonsNumWeapon[1].gameObject.SetActive(_items.TryGetValue("PIS", out _item));
        buttonsNumWeapon[1].GetComponent<Image>().sprite = _item?.Img;
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
