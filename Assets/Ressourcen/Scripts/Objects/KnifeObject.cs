using UnityEngine;

[CreateAssetMenu(fileName = "Null", menuName = "KnifeObject")]
public class KnifeObject : Weapon
{
    [Header("Distant")]
    public Vector2 distantAttack;

    public KnifeObject Clone()
    {
        KnifeObject _new = new KnifeObject();

        _new.Id = Id;
        _new.Name = Name;
        _new.condition = condition;

        _new.startTimeBtwShot = startTimeBtwShot;
        _new.dm = dm;

        _new.distantAttack = distantAttack;

        return _new;
    }
}
