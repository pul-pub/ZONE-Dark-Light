using UnityEngine;

[CreateAssetMenu(menuName = "Data", fileName = "Data")]
public class DataBase : ScriptableObject
{
    public Item[] items = new Item[] {};
    public Gun[] guns = new Gun[] {};
    public LightObject[] lights = new LightObject[] {};

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
