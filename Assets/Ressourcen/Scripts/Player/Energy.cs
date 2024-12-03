using UnityEngine;

public class Energy : MonoBehaviour
{
    public float energy { get; private set; } = 100f;

    private float _timer = 1f;

    private void Update()
    {
        if (_timer >= 0)
            _timer -= Time.deltaTime;
    }

    public void SetDownEnergy(float _energy)
    {
        if (_timer <= 0 && energy > 0)
            energy -= _energy;
    }

    public void SetUpEnergy()
    {
        if (_timer <= 0)
        {
            if (energy < 100)
                energy += 0.5f;
            _timer = 0.1f;
        }
    }
}
