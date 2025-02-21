using System;
using System.Collections.Generic;
using UnityEngine;

public enum TypeGroup { Millitary, Scientist, Stalker, ClearSky, Bandut, Dolg, Svoboda, Monolith, Mutant };

public class NPC : NpcAI, IMetaEssence
{
    [Header("NPC")]
    [SerializeField] string NameNPC;
    [SerializeField] private TypeGroup TypeBaseG;
    [SerializeField] SpriteRenderer Face;
    [Space]
    [NonSerialized] public NPCBackpack backpack;
    [SerializeField] private NPCBackpack backpackObject;
    [Space]
    [SerializeField] private Movement movement;
    [SerializeField] private Health health;
    [SerializeField] private Item[] items;
    [SerializeField] private WeaponManager weapon;
    [SerializeField] private OutFitManager outfit;
    [SerializeField] private int numberWeapon = 2;
    [SerializeField] private bool flagWeapon = false;
    [SerializeField] private Vector3 dideAngel;

    public Dictionary<string, Sprite> visualEnemy { get; set; } = new Dictionary<string, Sprite>();
    public string Name { get; set; }
    public TypeGroup TypeG { get; set; }

    private void Awake()
    {
        Name = NameNPC;
        weapon.Meta = this;
        typeGroup = TypeBaseG;
        TypeG = TypeBaseG;

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
        {
            visualEnemy.Add("Face", Face ? Face.sprite : null);
            visualEnemy.Add("Body", items[2] ? items[2].Clone().armorObject.ImgBody : null);
            visualEnemy.Add("Hand", items[2] ? items[2].Clone().armorObject.ImgHand : null);
            visualEnemy.Add("Leg", items[2] ? items[2].Clone().armorObject.ImgLeg : null);

            outfit.OnResetOutfit(new Item[4] { null, null, items[2] ? items[2].Clone() : null, null });
        }
        else
        {
            visualEnemy.Add("Body", Face.sprite);
        }
    }

    private void OnEnable()
    {
        OnMove += movement.Move;
        OnAttack += Shoot;
        health.Deid += OnDide;
        health.SetDebaff += OnDebuff;
    }

    private void OnDisable()
    {
        OnMove -= movement.Move;
        OnAttack -= Shoot;
        health.Deid -= OnDide;
        health.SetDebaff -= OnDebuff;
    }

    private void OnDide(IMetaEssence _meta)
    {
        if (_meta == null)
            StaticValue.countKills++;

        if (gameObject.name == "MninBoss-1")
            SaveHeandler.SessionSave.SetSwitchObject("MninBoss-1", false);

        movement.Dide(dideAngel);
        backpack = backpackObject.Clone();
        backpack.OnNullBackpack += delegate { Destroy(gameObject); };

        OnMove -= movement.Move;
        OnAttack -= Shoot;
    }

    private void Shoot() => weapon.Shoot();

    private void OnDebuff(TypeBodyParth _type, float _value)
    {
        if (_type == TypeBodyParth.Leg)
            movement.debufSpeed += _value;
    }
}
