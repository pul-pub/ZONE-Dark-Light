using UnityEngine;

public class NPC : NpcAI 
{
    [SerializeField] private Movement movement;
    [SerializeField] private Health health;
    [SerializeField] private WeaponManager weapon;
    [SerializeField] private int numberWeapon = 2;
    [SerializeField] private bool flagWeapon = false;

    private void Awake()
    {
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
        Destroy(gameObject);
    }

    private void Shoot() => weapon.Shoot();
}
