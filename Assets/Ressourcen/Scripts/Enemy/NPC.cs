using System;
using UnityEngine;

public class NPC : NpcAI 
{
    [Header("NPC")]
    [NonSerialized] public NPCBackpack backpack;
    [SerializeField] private NPCBackpack backpackObject;

    [SerializeField] private Movement movement;
    [SerializeField] private Health health;
    [SerializeField] private Item[] items;
    [SerializeField] private WeaponManager weapon;
    [SerializeField] private OutFitManager outfit;
    [SerializeField] private int numberWeapon = 2;
    [SerializeField] private bool flagWeapon = false;
    [SerializeField] private Vector3 dideAngel;

    private void Awake()
    {
        if (numberWeapon != 2)
            weapon.SetGunList(new Item[2] { items[0] ? items[0].Clone() : null, items[1] ? items[1].Clone() : null });

        if (weapon._flagWeapon != flagWeapon)
            weapon.SetNumberWeapon(numberWeapon);

        for (int i = 0; i < 2; i++)
        {
            if (weapon._guns[0] != null)
            {
                weapon._guns[0].startTimeBtwShot += 0.7f;
                weapon._guns[0].Reload(100);
            }
        }

        if (outfit)
            outfit.OnResetOutfit(new Item[4] { null, null, items[2] ? items[2].Clone() : null, null });
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
