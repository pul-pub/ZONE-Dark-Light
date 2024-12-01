using UnityEngine;
using UnityEngine.Rendering.Universal;

public class WorldTime : MonoBehaviour
{
    public float speedTime = 1.5f;
    public bool IsSetGragics = true;

    [SerializeField] private Transform sky;
    [SerializeField] private GameObject rain;
    [SerializeField] private Light2D light2D;

    private float timer;

    void Awake()
    {
        timer = speedTime;

        if (IsSetGragics)
        {
            SetPosSky();
            SetLight();
        }
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            timer = speedTime;
            AddMinute();
            if (IsSetGragics)
                SetLight();
            SetPosSky();
            UpdateLight();
        }
    }

    private void AddMinute()
    {
        StaticValue.time[1] += 1;

        if (StaticValue.time[1] >= 60)
        {
            StaticValue.time[1] = 0;
            StaticValue.time[0] += 1;

            rain.SetActive(false);

            int randRain = Random.Range(0, 10);
            if (randRain >= 8 && IsSetGragics)
            {
                rain.SetActive(true);
            }
        }

        if (StaticValue.time[0] >= 24)
        {
            StaticValue.time[0] = 0;
        }
    }

    private void SetPosSky()
    {
        if (((StaticValue.time[0] * 60) + StaticValue.time[1]) % 24 == 0)
        {
            if (StaticValue.time[0] >= 3 && StaticValue.time[0] < 12)
            {
                sky.localPosition -= new Vector3(0, 1.3f, 0);
            }
            else if (StaticValue.time[0] >= 12 && StaticValue.time[0] < 15)
            {
                sky.localPosition = new Vector3(0, 0.5f, 0);
            }
            else if (StaticValue.time[0] >= 15 && StaticValue.time[0] < 24)
            {
                sky.localPosition += new Vector3(0, 1.3f, 0);
            }
            else
            {
                sky.localPosition = new Vector3(0, 29, 0);
            }
        }
    }

    private void SetLight()
    {
        if (StaticValue.time[0] >= 3 && StaticValue.time[0] < 12)
        {
            light2D.intensity += 0.0013f;
        }
        else if (StaticValue.time[0] >= 12 && StaticValue.time[0] < 15)
        {
            light2D.intensity = 0.902f;
        }
        else if (StaticValue.time[0] >= 15 && StaticValue.time[0] < 24)
        {
            light2D.intensity -= 0.0013f;
        }
        else
        {
            light2D.intensity = 0.20f;
        }
    }

    private void UpdateLight()
    {
        if (StaticValue.time[0] >= 3 && StaticValue.time[0] < 12)
        {
            StaticValue.lightLevel += 0.0013f;
        }
        else if (StaticValue.time[0] >= 12 && StaticValue.time[0] < 15)
        {
            StaticValue.lightLevel = 1.002f;
        }
        else if (StaticValue.time[0] >= 15 && StaticValue.time[0] < 24)
        {
            StaticValue.lightLevel -= 0.0013f;
        }
        else
        {
            StaticValue.lightLevel = 0.30f;
        }
    }
}
