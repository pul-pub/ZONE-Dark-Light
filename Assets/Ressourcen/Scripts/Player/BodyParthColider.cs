using System;
using System.Collections.Generic;
using UnityEngine;

public class BodyParthColider : MonoBehaviour
{
    public event Action<BodyParthMeta, IMetaEssence> OnTakeDamade;

    [NonSerialized] public BodyParthMeta BodyParth;
    public int Layer = 0;

    [SerializeField] private BodyParthMeta parth;

    public void Awake()
    {
        BodyParth = parth.Clone();
    }

    public void ApplyDamage(float damage, IMetaEssence _meta = null)
    {
        if (_meta == null)
            StaticValue.countDm += damage;

        if (BodyParth.Hp > 0)
            BodyParth.Hp -= damage;

        OnTakeDamade?.Invoke(BodyParth, _meta);
    }

    public void ApplyMedic(float _medic)
    {
        if (BodyParth.Hp + _medic <= BodyParth.baseHp)
            BodyParth.Hp += _medic;
        else
            BodyParth.Hp = BodyParth.baseHp;
    }
}
    