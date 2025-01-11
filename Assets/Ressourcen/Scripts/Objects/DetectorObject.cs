using UnityEngine;

[CreateAssetMenu(fileName = "Null", menuName = "Detector")]
public class DetectorObject : ScriptableObject
{
    public int Id;
    public int Condition = 100;
    [Header("Charecteristic")]
    public int Chardge = 100;
    [Space]
    public float MinAmplitude = 0.1f;
    public float MaxAmplitude = 0.1f;
    public float MinPeriod = 0.1f;
    public float MaxPeriod = 0.1f;
    [Space]
    public float RadiusCheck;
    public Vector2 DurectionCheck;

    public DetectorObject Clone()
    {
        DetectorObject _new = new DetectorObject();

        _new.Id = Id;
        _new.Condition = Condition;

        _new.Chardge = Chardge;
        _new.MinAmplitude = MinAmplitude;
        _new.MaxAmplitude = MaxAmplitude;
        _new.MinPeriod = MinPeriod;
        _new.MaxPeriod = MaxPeriod;

        _new.RadiusCheck = RadiusCheck;
        _new.DurectionCheck = DurectionCheck;

        return _new;
    }
}
