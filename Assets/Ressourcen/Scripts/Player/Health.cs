using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Health : MonoBehaviour
{
    public event Action<IMetaEssence> Deid;
    public event Action<TypeBodyParth, float> SetDebaff;
    public event Action OnChangeValueHealth;
    public event Action OnEndInitilization;

    [SerializeField] private bool isDamagebly;
    public List<BodyParthColider> listBodyParths;

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


    private void OnEnable()
    {
        foreach (BodyParthColider _parth in listBodyParths)
            _parth.OnTakeDamade += OnTakeDamage;

        if (gameObject.GetComponent<Player>())
            SaveHeandler.OnSaveSession += Save;
    }

    private void OnDisable()
    {
        foreach (BodyParthColider _parth in listBodyParths)
            _parth.OnTakeDamade -= OnTakeDamage;

        if (gameObject.GetComponent<Player>())
            SaveHeandler.OnSaveSession -= Save;
    }

    private void Start()
    {
        if (gameObject.GetComponent<Player>())
        {
            listBodyParths[0].BodyParth.Hp = SaveHeandler.SessionSave.hpBodyParth["head"];
            listBodyParths[1].BodyParth.Hp = SaveHeandler.SessionSave.hpBodyParth["body"];
            listBodyParths[2].BodyParth.Hp = SaveHeandler.SessionSave.hpBodyParth["armL"];
            listBodyParths[3].BodyParth.Hp = SaveHeandler.SessionSave.hpBodyParth["armR"];
            listBodyParths[4].BodyParth.Hp = SaveHeandler.SessionSave.hpBodyParth["leg"];

            OnEndInitilization?.Invoke();
        }
    }


    public void OnTakeDamage(BodyParthMeta _parth, IMetaEssence _meta)
    {
        if ((_parth.TypeBodyParths == TypeBodyParth.Head && _parth.Hp <= 5) ||
            (_parth.TypeBodyParths == TypeBodyParth.Body && _parth.Hp <= 5))
            Deid?.Invoke(_meta);
        if (_parth.TypeBodyParths == TypeBodyParth.Leg && _parth.Hp <= 110)
            SetDebaff?.Invoke(TypeBodyParth.Leg, _parth.MaxDebuff * _parth.ProcentDamage);

        OnChangeValueHealth?.Invoke();
    }

    private void Save()
    {
        SaveHeandler.SessionSave.hpBodyParth["head"] = listBodyParths[0].BodyParth.Hp;
        SaveHeandler.SessionSave.hpBodyParth["body"] = listBodyParths[1].BodyParth.Hp;
        SaveHeandler.SessionSave.hpBodyParth["armL"] = listBodyParths[2].BodyParth.Hp;
        SaveHeandler.SessionSave.hpBodyParth["armR"] = listBodyParths[3].BodyParth.Hp;
        SaveHeandler.SessionSave.hpBodyParth["leg"] = listBodyParths[4].BodyParth.Hp;
    }
}
