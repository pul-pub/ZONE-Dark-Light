using UnityEngine;

[CreateAssetMenu(menuName = "Data", fileName = "Data")]
public class DataBase : ScriptableObject
{
    public Item[] items = new Item[] {};
    public Gun[] guns = new Gun[] {};

    public Gun GetGun(int _id)
    {
        foreach (Gun _gun in guns)
            if (_gun.Id == _id)
                return _gun;

        return null;
    }
}
