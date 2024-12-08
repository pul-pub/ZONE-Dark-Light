using System;
using UnityEngine;

public class NPC : NpcAI 
{
    [Header("NPC")]
    [NonSerialized] public NPCBackpack backpack;
    [SerializeField] private NPCBackpack backpackObject;

    [SerializeField] private Movement movement;
    [SerializeField] private Health health;
    [SerializeField] private Item[] weapons;
    [SerializeField] private WeaponManager weapon;
    [SerializeField] private int numberWeapon = 2;
    [SerializeField] private bool flagWeapon = false;
    [SerializeField] private Vector3 dideAngel;

    private void Awake()
    {
        if (numberWeapon != 2)
            weapon.SetGunList(new Item[] { weapons[0].Clone(), null });

        if (weapon._flagWeapon != flagWeapon)
            weapon.SetNumberWeapon(numberWeapon);
    }

    private void OnEnable()
    {
        OnMove += movement.Move;
        OnAttack += Shoot;
        health.Deid += OnDide;
    }

    private void OnDisable()
    {
        OnMove -= movement.Move;
        OnAttack -= Shoot;
        health.Deid -= OnDide;
    }

    private void OnDide()
    {
        movement.Dide(dideAngel);
        backpack = backpackObject.Clone();
        backpack.OnNullBackpack += delegate { Destroy(gameObject); };

        OnMove -= movement.Move;
        OnAttack -= Shoot;
    }

    private void Shoot() => weapon.Shoot();
}
