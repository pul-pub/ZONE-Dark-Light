using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Null", menuName = "SHOP")]
public class ShopObject : ScriptableObject
{
    public int Id;
    public string NameNPC;
    [Space]
    public List<ShopItem> Items;
}

[Serializable]

public class ShopItem
{
    public IItem Item;
    public int[] CellId = new int[2];
}
