using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private DataBase data;
    [Header("Weapon Grafics")]
    [SerializeField] private SpriteRenderer[] spRenderHead;
    [SerializeField] private SpriteRenderer[] spRenderBack;

    private float _timer = 0;

    public bool IsShoot { private set; get; } = false;
    private Gun[] _guns = new Gun[2];

    private int _numWeapon = 0;
    private bool _flagWeapon = false;

    private AnimatorObject _anim = new AnimatorObject();

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
            if (_numWeapon < 3)
            {
                _timer = 0.1f;
            }
            else
            {
                _timer = 0.1f;
            }
        }
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
        Debug.Log(_flagWeapon + " >>> " + _numWeapon);
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
        if (_guns[_numWeapon] != null)
        {
            if (_numWeapon == 0)
            {
                for (int j = 0; j < 2; j++)
                {
                    spRenderHead[j].gameObject.SetActive(!_flagWeapon);
                    spRenderBack[j].gameObject.SetActive(_flagWeapon);
                }
            }
            
            if (_flagWeapon)
            {
                spRenderHead[0].sprite = _guns[_numWeapon].imgBoxGun;
                spRenderHead[1].sprite = _guns[_numWeapon].imgStor;
            }
            else
            {
                spRenderBack[0].sprite = _guns[_numWeapon].imgBoxGun;
                spRenderBack[1].sprite = _guns[_numWeapon].imgStor;
            }
        }
    }
}
