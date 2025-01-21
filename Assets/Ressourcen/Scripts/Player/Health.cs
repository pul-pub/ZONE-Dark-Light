using System;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public event Action<IMetaEnemy> Deid;
    public event Action<TypeBodyParth, float> SetDebaff;
    public event Action OnChangeValueHealth;

    public float baseValueHealth;
    
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

    [SerializeField] private bool isDamagebly;
    public List<BodyParthColider> listBodyParths;

    private void Awake()
    {
        
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

    public void OnTakeDamage(BodyParthMeta _parth, IMetaEnemy _meta)
    {
        if ((_parth.TypeBodyParths == TypeBodyParth.Head && _parth.Hp <= 5) ||
            (_parth.TypeBodyParths == TypeBodyParth.Body && _parth.Hp <= 5))
            Deid?.Invoke(_meta);
        if (_parth.TypeBodyParths == TypeBodyParth.Leg && _parth.Hp <= 110)
            SetDebaff?.Invoke(TypeBodyParth.Leg, _parth.MaxDebuff * _parth.ProcentDamage);

        OnChangeValueHealth?.Invoke();
    }
}
