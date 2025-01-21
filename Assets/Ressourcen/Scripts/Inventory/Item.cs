using System;
using UnityEngine;

public enum TypeItem { Weapon, Cartridge, Armor, Ammo, Medicine, Backpack, NVG, Food, Water, PNV, Detector, HeadArmor, Habar, Quest };

[CreateAssetMenu(menuName = "Item", fileName = "NULL")]
public class Item : ScriptableObject
{
    public int id;
    public string Name;
    public string Discription;
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
    [SerializeField] private DetectorObject detector;
    [SerializeField] private ArtifactObject artifact;
    [SerializeField] private MedicObject medic;
    [NonSerialized] public Gun gunObject;
    [NonSerialized] public LightObject lightObject;
    [NonSerialized] public BackpackObject backpackObject;
    [NonSerialized] public ArmorObject armorObject;
    [NonSerialized] public DetectorObject detectorObject;
    [NonSerialized] public ArtifactObject artifactObject;
    [NonSerialized] public MedicObject medicObject;
    [Space]
    public Sprite img;

    public Item Clone()
    {
        gunObject = gun != null ? gun.Clone() : null;
        lightObject = light != null ? light.Clone() : null;
        backpackObject = backpack != null ? backpack.Clone() : null;
        armorObject = armor != null ? armor.Clone() : null;
        detectorObject = detector != null ? detector.Clone() : null;
        artifactObject = artifact != null ? artifact.Clone() : null;
        medicObject = medic != null ? medic.Clone() : null;
        
        Item _new = new Item();

        _new.id = id;
        _new.Name = Name;
        _new.Discription = Discription;
        _new.type = type;
        _new.typeAmmo = typeAmmo;
        _new.price = price;

        _new.countCell = countCell;
        _new.maxCount = maxCount;

        _new.gunObject = gunObject;
        _new.lightObject = lightObject;
        _new.backpackObject = backpackObject;
        _new.armorObject = armorObject;
        _new.detectorObject = detectorObject;
        _new.artifactObject = artifactObject;
        _new.medicObject = medicObject;

        _new.img = img;

        return _new;
    }
}
