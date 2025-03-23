using System;
using UnityEngine;

public class WeaponObject : MonoBehaviour
{
    public event Action<float> OnSuccessfulShoot;
    public event Action<string> OnUnsuccessfulShoot;

    public IItem Item { get; private set; }
    public bool IsUp { get; private set; }

    [SerializeField] private Transform pointOutBullet;
    [SerializeField] private Transform pointOutGilz;
    [Space]
    [SerializeField] private Transform userTransf;
    [Space]
    [SerializeField] private SpriteRenderer spRenderGun;
    [SerializeField] private SpriteRenderer spRenderStor;

    private Gun Weapon;
    private AmmoObject Ammos;
    private IMetaEssence Meta;

    private Transform _targetPoint;
    private SpriteRenderer _storHand;


    public void Initialization(IItem _item, Gun _wp, AmmoObject _am, IMetaEssence _user, Transform _userTrans, SpriteRenderer _stor)
    {
        Item = _item;
        Weapon = _wp;
        Ammos = _am;
        Meta = _user;
        _storHand = _stor;
        
        pointOutBullet.localPosition = _wp.PointOutBullet;
        pointOutGilz.localPosition = _wp.PointOutGilz;

        spRenderGun.sprite = _wp.ImgBoxGun;
        if (_wp.StorGrffics)
            spRenderStor.sprite = _wp.ImgStor;
        else
            spRenderStor.gameObject.SetActive(false);

        userTransf = _userTrans;
    }


    private void Update()
    {
        if (_targetPoint != null) 
        {
            transform.position = _targetPoint.position;
            transform.rotation = _targetPoint.rotation;
        }
    }

    public int AbliveReload() => Item.Value < Weapon.MaxAmmo ? Item.Value : -1;
    public int GetCurrentAmmos() => Item.Value;
    public string GetTypeAmmo() => Weapon.TypeAmmo;

    public void Shoot()
    {
        if (IsUp)
        {
            string _ret;

            if ((_ret = Item.Use()) == "")
            {
                GameObject _gObj;
                for (int i = 0; i <= Ammos.CountIn; i++)
                {
                    _gObj = Instantiate(Ammos.ObjBullet, pointOutBullet.position, pointOutBullet.rotation) as GameObject;
                    _gObj.transform.rotation = Quaternion.Euler(0, 0,
                        (userTransf.localScale.x > 0 ? _gObj.transform.eulerAngles.z : _gObj.transform.eulerAngles.z + 180) + 
                        (Ammos.CountIn == 1 ? UnityEngine.Random.Range(-1, 1) : UnityEngine.Random.Range(-5f, 5f)));
                    Bullet _bulletScript = _gObj.GetComponent<Bullet>();

                    _bulletScript.dm = Weapon.Dm / Ammos.CountIn;
                    _bulletScript.Layer = UnityEngine.Random.Range(-1, 1);
                    _bulletScript.force = Ammos.ForceBullet;
                    _bulletScript.meta = Meta;
                }

                _gObj = Instantiate(Ammos.ObjGilz, pointOutGilz.position, pointOutGilz.rotation) as GameObject;
                _gObj.GetComponent<Rigidbody2D>().AddForce(
                    (userTransf.localScale.x > 0 ? -_gObj.transform.right : _gObj.transform.right) * 5, ForceMode2D.Impulse);

                _gObj = Instantiate(Weapon.ObjFire, pointOutBullet.position, pointOutBullet.rotation) as GameObject;
                _gObj.transform.eulerAngles = new Vector3(0, 0, userTransf.localScale.x > 0 ? 0 : 180);

                OnSuccessfulShoot?.Invoke(Weapon.StartTimeBtwShot);
            }
            else
                OnUnsuccessfulShoot?.Invoke(_ret);
        }
    }

    public int Reload(int _value) => Item.Reload(_value);

    public void SetPointWeapon(bool _is, Transform _point, bool _flipY = false)
    {
        IsUp = _is;
        if (IsUp && Weapon.StorAmmoTakes)
            _storHand.sprite = Weapon.ImgStor;
        _targetPoint = _point;
        transform.localScale = new Vector3(transform.localScale.x,
                                           _flipY ? (transform.localScale.y > 0 ? -transform.localScale.y : transform.localScale.y) : 
                                                    (transform.localScale.y < 0 ? -transform.localScale.y : transform.localScale.y),
                                           transform.localScale.z);
    }
}
