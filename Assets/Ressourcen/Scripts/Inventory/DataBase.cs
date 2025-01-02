using UnityEngine;

[CreateAssetMenu(menuName = "Data", fileName = "Data")]
public class DataBase : ScriptableObject
{
    public Item[] items = new Item[] {};
    public Gun[] guns = new Gun[] {};
    public LightObject[] lights = new LightObject[] {};

    public Sprite[] faces = new Sprite[] { };
    public Quest[] quests = new Quest[] { };

    public Quest GetQuest(int _id)
    {
        foreach (Quest _q in quests)
            if (_q.Id == _id)
                return _q;

        return null;
    }

    public Item GetItem(int _id)
    {
        foreach (Item _item in items)
            if (_item.id == _id)
                return _item;

        return null;
    }

    public Gun GetGun(int _id)
    {
        foreach (Gun _gun in guns)
            if (_gun.Id == _id)
                return _gun;

        return null;
    }

    public LightObject GetLight(int _id)
    {
        foreach (LightObject _light in lights)
            if (_light.Id == _id)
                return _light;

        return null;
    }
}
