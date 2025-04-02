using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public event System.Action Shooting;
    public event System.Action<string> StartReloading;

    public IMetaEssence Meta;

    public int NumWeapon { get; private set; } = 0;
    public bool FlagWeapon { get; private set; } = false;
    public bool IsReload { get; private set; } = false;

    [SerializeField] private DataBase data;
    [SerializeField] private Object objWeapon;
    [Header("Animation")]
    [SerializeField] private Animator anim;
    [SerializeField] private SpriteRenderer stor;
    [Header("SHooting")]
    [SerializeField] private Transform pointGunDown;
    [SerializeField] private Transform pointPistolDown;
    [SerializeField] private Transform pointWeaponUp;
    [SerializeField] private Transform pointWeaponReload;
    [Space]
    [SerializeField] private LayerMask knifeLayer;
    [SerializeField] private KnifeObject knife;

    public IPack _pack;
    private bool _isShoot = false;
    private WeaponObject[] _guns = new WeaponObject[2];
    private float _timer = 0;

    private List<ObjectItem> _ammos = new();

    public void Initialization(int _numWeapon, bool _flagWeapon, IPack _pack)
    {
        this.NumWeapon = _numWeapon;
        this.FlagWeapon = _flagWeapon;
        this._pack = _pack;
        
        if (_numWeapon < 2 && _guns[_numWeapon] == null)
            this.FlagWeapon = false;

        UpdateWeapon();
    }

    private void Update()
    {
        if (_timer >= 0)
            _timer -= Time.deltaTime;

        if (_isShoot && NumWeapon <= 1 && !IsReload && _timer <= 0)
            Shooting?.Invoke();
    }

    public Dictionary<string, Sprite> GetImages()
    {
        Dictionary<string, Sprite> _dict = new Dictionary<string, Sprite>()
        {
            { "GUN", _guns[0]?.Item.Img },
            { "PIS", _guns[1]?.Item.Img  },
        };
        
        return _dict;
    }

    private void KnifeAttack()
    {
        anim.SetTrigger("Attack");
        
        RaycastHit2D[] _hit = Physics2D.BoxCastAll(transform.position, knife.DistantAttack, 0f, Vector2.zero, 0f, knifeLayer);
        if (_hit.Length > 0)
        {
            foreach (RaycastHit2D _h in _hit)
            {
                BodyParthColider _parth = _h.collider.gameObject.GetComponent<BodyParthColider>();

                if (_parth && _parth.Layer == UnityEngine.Random.Range(-1, 1))
                {
                    _parth.ApplyDamage(knife.Dm, Meta);
                }
            }
        }

        _timer = knife.StartTimeBtwShot;
    }

    public void StartReload()
    {
        if (FlagWeapon && _guns[NumWeapon] != null && NumWeapon < 2 && _guns[NumWeapon].AbliveReload() > -1 && !IsReload)
        {
            IsReload = true;
            _ammos = _pack.GetItems(_guns[NumWeapon].GetTypeAmmo());
            _guns[NumWeapon].SetPointWeapon(true, pointWeaponReload, false);
            anim.SetTrigger("Reload");
        }
    }

    public int GetAllItems()
    {
        if (FlagWeapon && NumWeapon < 2 && _guns[NumWeapon] != null)
        {
            int ammos = 0;
            foreach (ObjectItem ii in _pack.GetItems(_guns[NumWeapon].GetTypeAmmo()))
                ammos += ii.Count;
            return ammos;
        }
        else
            return -1;
    }

    public void EndReload()
    {
        IsReload = false;
        _pack.SetItems(_ammos, _guns[NumWeapon].Reload(FindCountAmmos(_ammos)));
        UpdateWeapon();
    }

    public int CheckAbliveReload() => FlagWeapon && NumWeapon < 2 && _guns[NumWeapon] ? _guns[NumWeapon].AbliveReload() : -1;
    public int GetAmmosNow() => FlagWeapon && NumWeapon < 2 && _guns[NumWeapon] ? _guns[NumWeapon].GetCurrentAmmos() : -1;

    public void OnShoot(bool _isShoot) 
    {
        if (NumWeapon <= 1)
            this._isShoot = _isShoot;
        else if (_isShoot && _timer <= 0)
            KnifeAttack();
    }
    public void OnCastelBolt(float _force)
    {
        anim.SetTrigger("Bolt");
        IsReload = true;
    }
    public void OnEndCastelBolt() => IsReload = false;

    public void OnUpdateOutFit(Dictionary<string, IItem> _items)
    {
        for (int i = 0; i < 2; i++)
        {
            if (_items.TryGetValue(i == 0 ? "GUN" : "PIS", out IItem _item))
            {
                if (_guns[i] != null)
                {
                    _guns[i].OnSuccessfulShoot -= OnSetStartTime;
                    Shooting -= _guns[i].Shoot;
                    Destroy(_guns[i].gameObject);
                    _guns[i] = null;
                }

                GameObject _gObj = Instantiate(objWeapon, transform) as GameObject;

                WeaponObject _wepObj = _gObj.GetComponent<WeaponObject>();
                Gun _gun = data.GetGun(_item.Id).Clone();
                AmmoObject _ammo = data.GetAmmos(_gun.TypeAmmo).Clone();

                _gun.Value = _item.Value;
                _gun.Condition = _item.Condition;
                
                _wepObj.Initialization(_item, _gun, _ammo, Meta, transform.parent, stor);
                _wepObj.OnSuccessfulShoot += OnSetStartTime;
                Shooting += _wepObj.Shoot;

                _guns[i] = _wepObj;
            }
            else
            {
                if (_guns[i] != null)
                {
                    _guns[i].OnSuccessfulShoot -= OnSetStartTime;
                    Shooting -= _guns[i].Shoot;
                    Destroy(_guns[i].gameObject);
                    _guns[i] = null;
                    FlagWeapon = i != NumWeapon && FlagWeapon;
                }
            }
        }

        UpdateWeapon();
    }

    public void OnSwitchWeapon(int _num)
    {
        if (NumWeapon != _num)
        {
            NumWeapon = _num;
            FlagWeapon = true;
        }
        else if (NumWeapon == _num && FlagWeapon)
            FlagWeapon = false;
        else if (NumWeapon == _num && !FlagWeapon)
            FlagWeapon = true;

        if (_num < 2)
            if (_guns[_num] == null)
                FlagWeapon = false;

        UpdateWeapon();
    }

    private void OnSetStartTime(float _time) => _timer = _time;

    private void UpdateWeapon()
    {
        for (int i = 0; i < 2; i++)
        {
            if (_guns[i])
                _guns[i].SetPointWeapon(
                    i == NumWeapon && FlagWeapon,
                    FlagWeapon && i == NumWeapon ? pointWeaponUp : (i == 0 ? pointGunDown : pointPistolDown),
                    i == 0 ? (NumWeapon == 0 ? !FlagWeapon : true) : false);
        }

        anim.SetBool("IsUp", FlagWeapon);
        anim.SetInteger("Type", NumWeapon);
    }

    private int FindCountAmmos(List<ObjectItem> _list)
    {
        int count = 0;

        foreach (ObjectItem item in _list)
            count += item.Count;

        return count;
    }

    public void Save()
    {
        SaveHeandler.SessionNow.numGun = NumWeapon;
        SaveHeandler.SessionNow.falgGun = FlagWeapon;
    }
    public void Load()
    {
        NumWeapon = SaveHeandler.SessionNow.numGun;
        FlagWeapon = SaveHeandler.SessionNow.falgGun;

        UpdateWeapon();
    }
}
