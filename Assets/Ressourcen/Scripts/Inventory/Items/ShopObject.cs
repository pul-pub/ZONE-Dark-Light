using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Null", menuName = "Inventory/SHOP")]
public class ShopObject : ScriptableObject
{
    public DataBase data;
    public int Id;
    public List<ShopItem> Stor;

    public List<ObjectItem> GetListItems()
    {
        List<ObjectItem> _list = new List<ObjectItem>();

        foreach (ShopItem item in Stor)
        {
            ObjectItem _new = new ObjectItem();

            _new.Item = data.GetItem(item.ID).CloneItem();
            _new.Item.Value = item.Value;
            _new.Item.Condition = item.Value;
            _new.Count = 1;
            _new.CellsId = item.CellId;

            _list.Add(_new);
        }
        
        return _list;
    }
}

[Serializable]

public class ShopItem
{
    public string ID;
    public int Value;
    public float Cond;
    public int[] CellId = new int[2];
}
