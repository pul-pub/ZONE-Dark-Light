using UnityEngine;
using UnityEngine.Rendering;

public enum TypeLight { Light, PNV }

[CreateAssetMenu(fileName = "Null", menuName = "LightObject")]
public class LightObject : ScriptableObject
{
    public int Id;
    public int condition = 100;

    public Sprite img;
    public int change = 100;
    public TypeLight typeLight;

    public VolumeProfile profile;

    public LightObject Clone()
    {
        LightObject _new = new LightObject();

        _new.Id = Id;
        _new.condition = condition;

        _new.img = img;
        _new.change = change;
        _new.typeLight = typeLight;

        _new.profile = profile;

        return _new;
    }
}
