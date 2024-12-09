using UnityEngine;

[CreateAssetMenu(fileName = "Null", menuName = "ArmorObject")]
public class ArmorObject : ScriptableObject
{
    public int Id;
    public int Condition = 100;

    public Sprite ImgHead;
    public Sprite ImgBody;
    public Sprite ImgHand;
    public Sprite ImgLeg;

    public int massUp = 10;

    public int AntiBullet = 0;
    public int AntiRadiation = 0;
    public int AntiBio = 0;
    public int AntiChimical = 0;
    public int AntiPsi = 0;

    public ArmorObject Clone()
    {
        ArmorObject _new = new ArmorObject();

        _new.Id = Id;
        _new.Condition = Condition;

        _new.ImgHead = ImgHead;
        _new.ImgBody = ImgBody;
        _new.ImgHand = ImgHand;

        _new.massUp = massUp;

        _new.AntiBullet = AntiBullet;
        _new.AntiRadiation = AntiRadiation;
        _new.AntiBio = AntiBio;
        _new.AntiChimical = AntiChimical;
        _new.AntiPsi = AntiPsi;

        return _new;
    }
}
