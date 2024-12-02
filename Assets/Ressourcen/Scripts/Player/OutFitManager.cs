using UnityEngine;

public class OutFitManager : MonoBehaviour
{
    public int MaxMass { get; private set; } = 20;

    private BackpackObject backpack;

    public void OnResetOutfit(Item[] _items)
    {
        if (_items[3] != null)
        {
            backpack = _items[3].backpackObject;
            MaxMass = 20 + backpack.massUp;
        }
        else
        {
            backpack = null;
            MaxMass = 20;
        }
    }
}
