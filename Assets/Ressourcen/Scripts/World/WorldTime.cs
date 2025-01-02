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

    private void OnEnable()
    {
        SaveHeandler.OnSaveSession += SaveSessino;
    }

    private void OnDisable()
    {
        SaveHeandler.OnSaveSession -= SaveSessino;
    }

    void Awake()
    {
        StaticValue.time = SaveHeandler.SessionSave.time;
        timer = speedTime;

        if (StaticValue.time[0] >= 3 && StaticValue.time[0] < 12)
        {
            sky.localPosition = new Vector3(0, 20 - ((((StaticValue.time[0] - 3) * 60) + StaticValue.time[1]) / 3 * 0.2f), 0);
            light2D.intensity = 0.1f + ((((StaticValue.time[0] - 3) * 60) + StaticValue.time[1]) / 2 * 0.0016f);
        }
        else if (StaticValue.time[0] >= 12 && StaticValue.time[0] < 15)
        {
            sky.localPosition = new Vector3(0, -16f, 0);
            light2D.intensity = 0.964f;
        }
        else if (StaticValue.time[0] >= 15 && StaticValue.time[0] < 24)
        {
            sky.localPosition = new Vector3(0, -16 + ((((StaticValue.time[0] - 15) * 60) + StaticValue.time[1]) / 3 * 0.2f), 0);
            light2D.intensity = 0.964f - ((((StaticValue.time[0] - 15) * 60) + StaticValue.time[1]) / 2 * 0.0016f);
        }
        else
        {
            sky.localPosition = new Vector3(0, 20, 0);
            light2D.intensity = 0.1f;
        }

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
        if (((StaticValue.time[0] * 60) + StaticValue.time[1]) % 3 == 0)
        {
            if (StaticValue.time[0] >= 3 && StaticValue.time[0] < 12)
            {
                sky.localPosition -= new Vector3(0, 0.2f, 0);
            }
            else if (StaticValue.time[0] >= 12 && StaticValue.time[0] < 15)
            {
                sky.localPosition = new Vector3(0, -16f, 0);
            }
            else if (StaticValue.time[0] >= 15 && StaticValue.time[0] < 24)
            {
                sky.localPosition += new Vector3(0, 0.2f, 0);
            }
            else
            {
                sky.localPosition = new Vector3(0, 20, 0);
            }
        }
    }

    private void SetLight()
    {
        if (StaticValue.time[0] >= 3 && StaticValue.time[0] < 12)
        {
            light2D.intensity += 0.0016f;
        }
        else if (StaticValue.time[0] >= 12 && StaticValue.time[0] < 15)
        {
            light2D.intensity = 0.964f;
        }
        else if (StaticValue.time[0] >= 15 && StaticValue.time[0] < 24)
        {
            light2D.intensity -= 0.0016f;
        }
        else
        {
            light2D.intensity = 0.1f;
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

    private void SaveSessino()
    {
        SaveHeandler.SessionSave.time = StaticValue.time;
    }
}
