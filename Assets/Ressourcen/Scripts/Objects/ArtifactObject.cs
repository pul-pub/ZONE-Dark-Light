using UnityEngine;

[CreateAssetMenu(fileName = "Null", menuName = "Artifact")]
public class ArtifactObject : ScriptableObject
{
    public int Id;
    public int Condition = 100;
    [Header("Find parameters")]
    public float Amplitude = 0.1f;
    public float Period = 0.1f;
    [Header("Anomaly Paremeters")]
    public int Chardge = 100;
    [Space]
    public float AntiDamage = 0;
    public float AntiRadiation = 0;
    public float AntiBio = 0;
    public float AntiChimical = 0;
    public float AntiPsi = 0;
    public int ChangeMussUp = 0;
    public float ChangeSpeed = 0;
    public float RecoveryHealth = 0;
    public float RecoveryEnergy = 0;

    public ArtifactObject Clone()
    {
        ArtifactObject _new = new ArtifactObject();

        _new.Id = Id;
        _new.Condition = Condition;

        _new.Amplitude = Amplitude;
        _new.Period = Period;

        _new.Chardge = Chardge;
        _new.AntiDamage = AntiDamage;
        _new.AntiRadiation = AntiRadiation;
        _new.AntiBio = AntiBio;
        _new.AntiChimical = AntiChimical;
        _new.AntiPsi = AntiPsi;
        _new.ChangeMussUp = ChangeMussUp;
        _new.ChangeSpeed = ChangeSpeed;
        _new.RecoveryHealth = RecoveryHealth;
        _new.RecoveryEnergy = RecoveryEnergy;

        return _new;
    }
}
