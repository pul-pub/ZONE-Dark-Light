using System;
using UnityEngine;

public class BodyParthColider : MonoBehaviour
{
    public event Action<BodyParthMeta, IMetaEssence> OnTakeDamade;

    public BodyParthMeta BodyParth { get; private set; }
    public int Layer = 0;

    [SerializeField] private BodyParthMeta parth;

    public void Awake()
    {
        BodyParth = parth.Clone();
    }

    public void Start()
    {
        OnTakeDamade?.Invoke(BodyParth, null);
    }

    public void ApplyDamage(float damage, IMetaEssence _meta = null)
    {
        if (_meta == null)
            StaticValue.countDm += damage;

        if (BodyParth.Hp > 0)
            BodyParth.Hp -= damage;

        if (BodyParth.Hp < 0)
            BodyParth.Hp = 0;

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
    