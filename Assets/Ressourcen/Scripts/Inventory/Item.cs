using UnityEngine;

public enum TypeItem { Weapon, Cartridge, Armor, Medicine, Backpack, NVG, Food, Water, PNV, Detector, Quest };

[CreateAssetMenu(menuName = "Item", fileName = "NULL")]
public class Item : ScriptableObject
{
    public int id;
    public string Name;
    public TypeItem type;
    public int price = 0;
    [Space]
    public int countCell = 1;
    public int maxCount= 32;
    [Space]
    public Sprite img;
}
