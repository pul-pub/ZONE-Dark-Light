using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Null", menuName = "NPC Backpack")]
public class NPCBackpack : ScriptableObject
{
    public event Action OnNullBackpack;

    public int Id;
    public int condition = 100;
    [Header("Random")]
    [SerializeField] private bool randomPack = false;
    [SerializeField] private List<Item> itemsRandom = new List<Item>(10);
    [SerializeField] private List<int> countMin = new List<int>(10);
    [SerializeField] private List<int> countMax = new List<int>(10);
    [SerializeField] private List<int> chanceRandom = new List<int>(10);
    [Header("Static")]
    [SerializeField] private List<Item> itemsStatic = new List<Item>(10);
    [SerializeField] private List<int> countStatic = new List<int>(10);

    [NonSerialized] public List<Item> items = new List<Item>(10);
    [NonSerialized] public List<int> count = new List<int>(10);

    public void NullCountPack()
    {
        if (OnNullBackpack != null)
            OnNullBackpack.Invoke();
    }

    public NPCBackpack Clone()
    {
        if (randomPack)
        {
            for (int i = 0; i < itemsRandom.Count; i++)
            {
                if (UnityEngine.Random.Range(0, 400) >= chanceRandom[i])
                {
                    items.Add(itemsRandom[i]);
                    count.Add(UnityEngine.Random.Range(countMin[i], countMax[i]));
                }
            }
        }
        else
        {
            items = itemsStatic;
            count = countStatic;
        }

        NPCBackpack _new = new NPCBackpack();

        _new.Id = Id;
        _new.condition = condition;

        _new.items = new List<Item>(items.Count);
        foreach (Item _item in items)
            _new.items.Add(_item);
        _new.count = new List<int>(count.Count);
        foreach (int _i in count)
            _new.count.Add(_i);

        return _new;
    }
}
