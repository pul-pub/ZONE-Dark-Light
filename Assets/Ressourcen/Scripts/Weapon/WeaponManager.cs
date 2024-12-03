using TMPro;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public Inventory inv;

    [SerializeField] private DataBase data;
    [Header("Weapon Grafics")]
    [SerializeField] private SpriteRenderer[] spRenderHead;
    [SerializeField] private SpriteRenderer[] spRenderBack;
    [Header("Animation")]
    [SerializeField] private Animator anim;
    [Header("SHooting")]
    [SerializeField] private Transform pointBullet;
    [SerializeField] private Transform pointGilz;
    [SerializeField] private Object objBullet;
    [SerializeField] private Object objGilz;
    [SerializeField] private Object objFire;
    [SerializeField] private Transform parent;

    private float _timer = 0;

    public bool IsShoot { private set; get; } = false;
    public Gun[] _guns { private set; get; } = new Gun[2];

    public int _numWeapon { private set; get; } = 0;
    public bool _flagWeapon { private set; get; } = false;
    public bool _isReload { private set; get; } = false;

    private void Update()
    {
        if (_timer >= 0)
            _timer -= Time.deltaTime;

        if (IsShoot && _numWeapon <= 1)
            Shoot();
    }

    private void Shoot()
    {
        if (_timer <= 0)
        {
            if (_numWeapon <= 1)
            {
                if (_flagWeapon && _guns[_numWeapon] != null && _guns[_numWeapon].currentAmmos >= 1 && !_isReload)
                {
                    if (_guns[_numWeapon].Shoot(objBullet, parent, pointBullet, (int)transform.parent.localScale.x))
                    {
                        GameObject _gObj = Instantiate(objGilz, pointGilz.position, pointGilz.rotation, parent) as GameObject;
                        Instantiate(objFire, pointBullet.position, pointBullet.rotation, parent);

                        _gObj.GetComponent<Rigidbody2D>().AddForce(-_gObj.transform.right * 5, ForceMode2D.Impulse);
                    }

                    _timer = _guns[_numWeapon].startTimeBtwShot;
                }
            }
            else
            {
                anim.SetTrigger("Attack");
            }
        }
    }

    public void StartReload()
    {
        if (_flagWeapon && _guns[_numWeapon] != null && _guns[_numWeapon].currentAmmos < _guns[_numWeapon].ammo && !_isReload)
        {
            _isReload = true;
            anim.SetTrigger("Reload");
        }
    }

    public void EndReload()
    {
        _isReload = false;
        inv.SetCountAmmo(_guns[_numWeapon].Reload(inv.GetCountAmmos(_guns[_numWeapon])));
    }

    public void SetNumberWeapon(int _num)
    {
        if (_numWeapon != _num)
        {
            _numWeapon = _num;
            _flagWeapon = true;
        }
        else if (_numWeapon == _num && _flagWeapon)
            _flagWeapon = false;
        else if (_numWeapon == _num && !_flagWeapon)
            _flagWeapon = true;

        if (_num < 2)
            if (_guns[_num] == null)
                _flagWeapon = false;
        
        UpdateWeapon();
    }

    public void SetIsShoot(bool _isShoot) 
    {
        if (_numWeapon <= 1)
            IsShoot = _isShoot;
        else if (_isShoot)
            Shoot();
    }

    public void SetGunList(Item[] _items)
    {
        for (int i = 0; i < 2; i++)
        {
            if (_items[i] != null)
                _guns[i] = _items[i].gunObject;
            else
                _guns[i] = null;
        }

        SetNumberWeapon(_numWeapon);
        UpdateWeapon();
    }

    private void UpdateWeapon()
    {
        if (_numWeapon <= 1)
        {
            for (int i = 0; i < 2; i++)
            {
                if (_guns[i] != null)
                {
                    if (_flagWeapon)
                    {
                        for (int j = 0; j < 2; j++)
                            spRenderHead[j].gameObject.SetActive(true);

                        if (!_guns[_numWeapon].isStorUp)
                            spRenderHead[1].gameObject.SetActive(false);

                        spRenderHead[0].sprite = _guns[_numWeapon].imgBoxGun;
                        spRenderHead[1].sprite = _guns[_numWeapon].imgStor;
                        spRenderHead[2].sprite = _guns[_numWeapon].imgStor;
                    }
                    else
                    {
                        for (int j = 0; j < 2; j++)
                            spRenderHead[j].gameObject.SetActive(false);
                    }

                    if (_guns[0] != null && _numWeapon != 0)
                    {
                        for (int j = 0; j < 2; j++)
                            spRenderBack[j].gameObject.SetActive(true);

                        spRenderBack[0].sprite = _guns[0].imgBoxGun;
                        spRenderBack[1].sprite = _guns[0].imgStor;
                    }
                    else if (_guns[0] != null && _numWeapon == 0 && !_flagWeapon)
                    {
                        for (int j = 0; j < 2; j++)
                            spRenderBack[j].gameObject.SetActive(true);

                        spRenderBack[0].sprite = _guns[0].imgBoxGun;
                        spRenderBack[1].sprite = _guns[0].imgStor;
                    }
                    else
                    {
                        for (int j = 0; j < 2; j++)
                            spRenderBack[j].gameObject.SetActive(false);
                    }
                }
            }
        }
        else
        {
            for (int j = 0; j < 2; j++)
                spRenderHead[j].gameObject.SetActive(false);
        }

        anim.SetBool("IsUp", _flagWeapon);
        anim.SetInteger("Type", _numWeapon);
    }
}
