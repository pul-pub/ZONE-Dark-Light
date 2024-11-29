using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private DataBase data;
    [Header("Weapon Grafics")]
    [SerializeField] private SpriteRenderer[] spRenderHead;
    [SerializeField] private SpriteRenderer[] spRenderBack;
    [Header("Animation")]
    [SerializeField] private Animator anim;
    [Header("SHooting")]
    [SerializeField] private Transform pointBullet;
    [SerializeField] private Object objBullet;
    [SerializeField] private Transform parent;

    private float _timer = 0;

    public bool IsShoot { private set; get; } = false;
    private Gun[] _guns = new Gun[2];

    private int _numWeapon = 0;
    private bool _flagWeapon = false;
    private bool _isReload = false;

    private void Update()
    {
        if (_timer >= 0)
            _timer -= Time.deltaTime;

        if (IsShoot)
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
                    if (_guns[_numWeapon].Shoot(objBullet, parent, pointBullet))
                    {
                        Debug.Log("F"); 
                    }

                    _timer = _guns[_numWeapon].startTimeBtwShot;
                }
            }
            else
            {
                _timer = 0.1f;
            }
        }
    }

    public void StartReload()
    {

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

    public void SetIsShoot(bool _isShoot) => IsShoot = _isShoot;

    public void SetGunList(Item[] _items)
    {
        for (int i = 0; i < 2; i++)
        {
            if (_items[i] != null)
                _guns[i] = data.GetGun(_items[i].id).Clone();
            else
                _guns[i] = null;
        }

        UpdateWeapon();
    }

    private void UpdateWeapon()
    {
        for (int i = 0; i < 2; i++)
        {
            if (_guns[i] != null)
            {
                if (_flagWeapon)
                {
                    _guns[_numWeapon].Reload(100);

                    for (int j = 0; j < 2; j++)
                        spRenderHead[j].gameObject.SetActive(true);

                    if (!_guns[_numWeapon].isStorUp)
                        spRenderHead[1].gameObject.SetActive(false);

                    spRenderHead[0].sprite = _guns[_numWeapon].imgBoxGun;
                    spRenderHead[1].sprite = _guns[_numWeapon].imgStor;
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

        anim.SetBool("IsUp", _flagWeapon);
        anim.SetInteger("Type", _numWeapon);
    }
}
