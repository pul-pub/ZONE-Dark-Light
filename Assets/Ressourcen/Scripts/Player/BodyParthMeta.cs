using System;
using UnityEditor;
using UnityEngine;

public enum TypeBodyParth { Head, Body, Hand, Leg };

[CreateAssetMenu(fileName = "Null", menuName = "BodyParthMeta")]
public class BodyParthMeta : ScriptableObject
{
    public float baseHp = 100;
    public TypeBodyParth TypeBodyParths;
    [Space]
    public float MaxDebuff = 1;

    public float ProcentDamage
    {
        get => (baseHp - Hp) / baseHp;
    }

    [NonSerialized] public float Hp;

    public BodyParthMeta Clone()
    {
        BodyParthMeta _new = new BodyParthMeta();

        _new.Hp = baseHp;
        _new.baseHp = baseHp;
        _new.MaxDebuff = MaxDebuff;

        return _new;
    }
}
