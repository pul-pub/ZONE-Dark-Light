using System;
using System.Collections.Generic;

public interface IPack
{
    event Action<Dictionary<string, IItem>> ChangeOutfit;
    event Action OnUpdateInventory;

    List<ObjectItem> DeathPack { get; set; }
    float WeightInventory { get; }

    void Initialization();
    List<ObjectItem> GetItems(string _id);
    void SetItems(List<ObjectItem> _items, int _count);
    void CreateDeathPack(IMetaEssence _meta);
}
