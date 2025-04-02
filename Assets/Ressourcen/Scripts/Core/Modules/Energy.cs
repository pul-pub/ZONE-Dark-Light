using UnityEngine;

public class Energy : MonoBehaviour
{
    public float Value { get; private set; } = 100f;

    private float _timer = 1f;

    private void Update()
    {
        if (_timer >= 0)
            _timer -= Time.deltaTime;
    }

    public void SetDownEnergy(float _energy)
    {
        if (_timer <= 0)
        {
            if (Value - _energy > 0)
                Value -= _energy;
            else
                Value = 0;

            _timer = 0.1f;
        } 
    }

    public void SetUpEnergy(float _energy)
    {
        if (Value + _energy < 100)
            Value += _energy;
        else
            Value = 100;
    }

    public void Save()
    {
        SaveHeandler.SessionNow.energy = Value;
    }

    public void Load()
    {
        Value = SaveHeandler.SessionNow.energy;
    }
}
