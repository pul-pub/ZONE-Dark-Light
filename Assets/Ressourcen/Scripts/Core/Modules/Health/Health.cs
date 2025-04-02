using System;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public event Action<IMetaEssence> Death;
    public event Action<TypeBodyParth, float> SetDebaff;
    public event Action OnChangeValueHealth;

    [SerializeField] private bool isDamagebly;
    public List<BodyParthColider> listBodyParths = new();

    public float HealthAll 
    { 
        get
        {
            float _all = 0;

            foreach (BodyParthColider _parth in listBodyParths)
                _all += _parth.BodyParth.Hp;

            return _all;
        }
    }

    private void OnEnable()
    {
        foreach (BodyParthColider _parth in listBodyParths)
            _parth.OnTakeDamade += OnTakeDamage;
    }

    private void OnDisable()
    {
        foreach (BodyParthColider _parth in listBodyParths)
            _parth.OnTakeDamade -= OnTakeDamage;
    }

    public void OnTakeDamage(BodyParthMeta _parth, IMetaEssence _meta)
    {
        if ((_parth.TypeBodyParths == TypeBodyParth.Head && _parth.Hp <= 10) ||
            (_parth.TypeBodyParths == TypeBodyParth.Body && _parth.Hp <= 5))
            Death?.Invoke(_meta);
        if (_parth.TypeBodyParths == TypeBodyParth.Leg && _parth.Hp <= 110)
            SetDebaff?.Invoke(TypeBodyParth.Leg, _parth.MaxDebuff * _parth.ProcentDamage);

        OnChangeValueHealth?.Invoke();
    }

    public void Load()
    {
        listBodyParths[0].BodyParth.Hp = SaveHeandler.SessionNow.hpBodyParth["head"];
        listBodyParths[1].BodyParth.Hp = SaveHeandler.SessionNow.hpBodyParth["body"];
        listBodyParths[2].BodyParth.Hp = SaveHeandler.SessionNow.hpBodyParth["armL"];
        listBodyParths[3].BodyParth.Hp = SaveHeandler.SessionNow.hpBodyParth["armR"];
        listBodyParths[4].BodyParth.Hp = SaveHeandler.SessionNow.hpBodyParth["leg"];
    }

    public void Save()
    {
        SaveHeandler.SessionNow.hpBodyParth["head"] = listBodyParths[0].BodyParth.Hp;
        SaveHeandler.SessionNow.hpBodyParth["body"] = listBodyParths[1].BodyParth.Hp;
        SaveHeandler.SessionNow.hpBodyParth["armL"] = listBodyParths[2].BodyParth.Hp;
        SaveHeandler.SessionNow.hpBodyParth["armR"] = listBodyParths[3].BodyParth.Hp;
        SaveHeandler.SessionNow.hpBodyParth["leg"] = listBodyParths[4].BodyParth.Hp;
    }
}
