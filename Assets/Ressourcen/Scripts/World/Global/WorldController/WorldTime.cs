using UnityEngine;
using UnityEngine.Rendering.Universal;

public class WorldTime : MonoBehaviour
{
    
    

    [Header("—————----—  Grafics  ————----——")]
    public bool IsGlobalLight = true;
    [ConditionallyVisible(nameof(IsGlobalLight))]
    [SerializeField] private Transform sky;
    [ConditionallyVisible(nameof(IsGlobalLight))]
    [SerializeField] private GameObject rain;
    [ConditionallyVisible(nameof(IsGlobalLight))]
    [SerializeField] private Light2D light2D;
    [Header("—————----—  Settings  ————----——")]
    [SerializeField] private float SpeedTime = 1.5f;
    [SerializeField] private float minLightValue = 0.1f;
    [SerializeField] private float maxLightValue = 0.95f;
    [SerializeField] private float minSkyY = 20f;
    [SerializeField] private float maxSkyY = -20f;
    [SerializeField] private int chanceSartRain = 5;

    private float timer = 0f;
    private float stupLight = 0f;
    private float stupSky = 0f;

    private void Awake()
    {
        stupLight = (maxLightValue - minLightValue) / 540;
        stupSky = (maxSkyY - minSkyY) / 540;
    }

    private void OnEnable()
    {
        SaveHeandler.SaveSession += Save;
        SaveHeandler.LoadSession += Load;
    }

    private void OnDisable()
    {
        SaveHeandler.SaveSession -= Save;
        SaveHeandler.LoadSession -= Load;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            timer = SpeedTime;
            AddMinute();
            if (IsGlobalLight)
            {
                SetLight();
                SetPosSky();
                UpdateLight();
            }  
        }
    }

    private void AddMinute()
    {
        StaticValue.time[1] += 1;

        if (StaticValue.time[1] >= 60)
        {
            StaticValue.time[1] = 0;
            StaticValue.time[0] += 1;

            int randRain = Random.Range(0, 100);
            rain.SetActive(randRain <= chanceSartRain && IsGlobalLight);
        }

        if (StaticValue.time[0] >= 24)
            StaticValue.time[0] = 0;
    }

    private void SetPosSky()
    {
        if (StaticValue.time[0] >= 15)
            sky.localPosition = new Vector3(0, maxSkyY - (stupSky * (((StaticValue.time[0] - 15) * 60) + StaticValue.time[1])), 0);
        else if (StaticValue.time[0] >= 12)
            sky.localPosition = new Vector3(0, maxSkyY, 0);
        else if (StaticValue.time[0] >= 3)
            sky.localPosition = new Vector3(0, minSkyY + (stupSky * (((StaticValue.time[0] - 3) * 60) + StaticValue.time[1])), 0);
        else
            sky.localPosition = new Vector3(0, minSkyY, 0);
    }

    private void SetLight()
    {
        if (StaticValue.time[0] >= 15)
            light2D.intensity = maxLightValue - (stupLight * (((StaticValue.time[0] - 15) * 60) + StaticValue.time[1]));
        else if (StaticValue.time[0] >= 12)
            light2D.intensity = maxLightValue;
        else if (StaticValue.time[0] >= 3)
            light2D.intensity = minLightValue + (stupLight * (((StaticValue.time[0] - 3) * 60) + StaticValue.time[1]));
        else
            light2D.intensity = minLightValue;
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

    private void Save() => SaveHeandler.SessionNow.time = StaticValue.time;

    private void Load()
    {
        StaticValue.time = SaveHeandler.SessionNow.time;
        timer = SpeedTime;

        if (IsGlobalLight)
        {
            SetPosSky();
            SetLight();
        }
    }
}
