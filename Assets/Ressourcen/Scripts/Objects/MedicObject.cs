using UnityEngine;

[CreateAssetMenu(fileName = "Null", menuName = "Medic")]
public class MedicObject : ScriptableObject
{
    public int Id;
    public int Condition = 100;
    [Header("Characterisctic")]
    public float RecoveryHP = 1f;
    public float StoppingBleeding = 1f;

    public MedicObject Clone()
    {
        MedicObject _new = new MedicObject();

        _new.Id = Id;
        _new.Condition = Condition;

        _new.RecoveryHP = RecoveryHP;
        _new.StoppingBleeding = StoppingBleeding;

        return _new;
    }
}
