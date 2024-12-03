using System;
using UnityEngine;

public enum TypeItem { Weapon, Cartridge, Armor, Ammo, Medicine, Backpack, NVG, Food, Water, PNV, Detector, HeadArmor, Quest };

[CreateAssetMenu(menuName = "Item", fileName = "NULL")]
public class Item : ScriptableObject
{
    public int id;
    public string Name;
    public TypeItem type;
    public TypeAmmo typeAmmo;
    public int price = 0;
    [Space]
    public int countCell = 1;
    public int maxCount= 32;
    public float weight = 0.1f;
    [Space]
    [SerializeField] private Gun gun;
    [SerializeField] private LightObject light;
    [SerializeField] private BackpackObject backpack;
    [SerializeField] private ArmorObject armor;
    [NonSerialized] public Gun gunObject;
    [NonSerialized] public LightObject lightObject;
    [NonSerialized] public BackpackObject backpackObject;
    [NonSerialized] public ArmorObject armorObject;
    [Space]
    public Sprite img;

    public Item Clone()
    {
        gunObject = gun != null ? gun.Clone() : null;
        lightObject = light != null ? light.Clone() : null;
        backpackObject = backpack != null ? backpack.Clone() : null;
        armorObject = armor != null ? armor.Clone() : null;
        
        Item _new = new Item();

        _new.id = id;
        _new.Name = Name;
        _new.type = type;
        _new.typeAmmo = typeAmmo;
        _new.price = price;

        _new.countCell = countCell;
        _new.maxCount = maxCount;

        _new.gunObject = gunObject;
        _new.lightObject = lightObject;
        _new.backpackObject = backpackObject;
        _new.armorObject = armorObject;

        _new.img = img;

        return _new;
    }
}
