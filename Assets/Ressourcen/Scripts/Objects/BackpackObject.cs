using UnityEngine;

[CreateAssetMenu(fileName = "Null", menuName = "Backpack")]
public class BackpackObject : ScriptableObject
{
    public int Id;
    public int condition = 100;

    public Sprite img;
    public int massUp;

    public BackpackObject Clone()
    {
        BackpackObject _new = new BackpackObject();

        _new.Id = Id;
        _new.condition = condition;

        _new.img = img;
        _new.massUp = massUp;

        return _new;
    }
}
