using System.Runtime.CompilerServices;
using UnityEngine;

public enum TypeAmmo { NULL, Shotgun_12, s5x45 };

[CreateAssetMenu(menuName = "Gun", fileName = "Null")]
public class Gun : Weapon
{
    [Header("Grafics")]
    public Sprite imgBoxGun;
    public Sprite imgStor;
    [Header("Audio")]
    public AudioClip soundShoot;
    public AudioClip soundSpusk;
    [Header("Ammo")]
    public TypeAmmo typeAmmo;
    public int ammo;
    public float force;
    public float verticalRecoil;

    public int currentAmmos { private set; get; } = 0;

    public bool Shoot(Object bullet, Transform parent, GameObject _pointStart)
    {
        if (typeAmmo == TypeAmmo.Shotgun_12)
        {
            for (int i = 0; i < 5; i++)
            {
                GameObject obj = Object.Instantiate(bullet, _pointStart.transform.position, _pointStart.transform.rotation, parent) as GameObject;

                Bullet _bullet = obj.GetComponent<Bullet>();

                _bullet.dm = dm;
                _bullet.force = force;

                obj.transform.eulerAngles = new Vector3(0, 0, obj.transform.eulerAngles.z + Random.Range(-8f, 8f));
            }
        }
        else
        {
            GameObject obj = Object.Instantiate(bullet, _pointStart.transform.position, _pointStart.transform.rotation, parent) as GameObject;

            Bullet _bullet = obj.GetComponent<Bullet>();

            _bullet.dm = dm;
            _bullet.force = force;
        }

        currentAmmos -= 1;

        return false;
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
        _new.dm = dm;
        _new.startTimeBtwShot = startTimeBtwShot;

        _new.imgBoxGun = imgBoxGun;
        _new.imgStor = imgStor;

        _new.soundShoot = soundShoot;
        _new.soundSpusk = soundSpusk;

        _new.typeAmmo = typeAmmo;
        _new.ammo = ammo;
        _new.force = force;
        _new.verticalRecoil = verticalRecoil;
        _new.currentAmmos = currentAmmos;

        return _new;
    }
}
