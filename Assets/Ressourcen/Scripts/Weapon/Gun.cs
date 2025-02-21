using System.Runtime.CompilerServices;
using UnityEngine;

public enum TypeAmmo { NULL, x5x45, x7x62, x5x56, x9x19, Shotgun_12_20, Shotgun_12_10, Shotgun_12_1 };

[CreateAssetMenu(menuName = "Gun", fileName = "Null")]
public class Gun : Weapon
{
    [Header("Grafics")]
    public Sprite imgBoxGun;
    public bool isStorUp = true;
    public Sprite imgStor;
    public Vector2 pointFire;
    [Header("Audio")]
    public AudioClip soundShoot;
    public AudioClip soundSpusk;
    [Header("Ammo")]
    public TypeAmmo typeAmmo;
    public int ammo;
    public float force;
    public float verticalRecoil;
    public int scatterHorizontal;

    public int currentAmmos { set; get; } = 0;

    public bool Shoot(Object _bullet, Transform _parent, Transform _pointStart, int _flipX, IMetaEssence _meta = null)
    {
        if (typeAmmo == TypeAmmo.Shotgun_12_20 ||
            typeAmmo == TypeAmmo.Shotgun_12_10)
        {
            for (int i = 0; i < (typeAmmo == TypeAmmo.Shotgun_12_20 ? 20 : 10); i++)
            {
                GameObject _gObj = Object.Instantiate(_bullet, _pointStart.position, _pointStart.rotation, _parent) as GameObject;
                _gObj.transform.eulerAngles = new Vector3(0, 0, _flipX > 0 ? 0 : 180);

                Bullet _bulletScript = _gObj.GetComponent<Bullet>();

                _bulletScript.dm = dm / (typeAmmo == TypeAmmo.Shotgun_12_20 ? 20 : 10);
                _bulletScript.Layer = 0;
                _bulletScript.force = force;
                _bulletScript.meta = _meta;

                _gObj.transform.eulerAngles = new Vector3(0, 0, _gObj.transform.eulerAngles.z + Random.Range(-8f, 8f));
            }
        }
        else
        {
            GameObject _gObj = Object.Instantiate(_bullet, _pointStart.position, _pointStart.rotation, _parent) as GameObject;
            _gObj.transform.eulerAngles = new Vector3(0, 0, _flipX > 0 ? 0 : 180);

            Bullet _bulletScript = _gObj.GetComponent<Bullet>();

            _bulletScript.dm = dm;
            _bulletScript.Layer = Random.Range(-1, 1);
            _bulletScript.force = force;
            _bulletScript.meta = _meta;

            _gObj.transform.eulerAngles = new Vector3(0, 0, _gObj.transform.eulerAngles.z + Random.Range(-1f, 1f));
        }

        currentAmmos -= 1;

        return true;
    }

    public int Reload(int _ammos)
    {
        int reason = ammo - currentAmmos;
        int returnAmmos = 0;

        if (_ammos >= reason)
        {
            returnAmmos += reason;
            currentAmmos += reason;
        }
        else
        {
            currentAmmos += _ammos;
            returnAmmos = _ammos;
        }

        return returnAmmos;
    }

    public Gun Clone()
    {
        Gun _new = new Gun();

        _new.Name = Name;
        _new.Id = Id;
        _new.condition = condition;
        _new.dm = dm;
        _new.startTimeBtwShot = startTimeBtwShot;

        _new.imgBoxGun = imgBoxGun;
        _new.isStorUp = isStorUp;
        _new.imgStor = imgStor;
        _new.pointFire = pointFire;

        _new.soundShoot = soundShoot;
        _new.soundSpusk = soundSpusk;

        _new.typeAmmo = typeAmmo;
        _new.ammo = ammo;
        _new.force = force;
        _new.verticalRecoil = verticalRecoil;
        _new.currentAmmos = currentAmmos;
        _new.scatterHorizontal = scatterHorizontal;

        return _new;
    }
}
