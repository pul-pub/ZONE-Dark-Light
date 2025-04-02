using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI valueAlfaGUI;
    [SerializeField] private Slider sliderAlfaGUI;
    [SerializeField] private Image[] imagesGUI;
    [Space]
    [SerializeField] private TextMeshProUGUI valueMusic;
    [SerializeField] private Slider sliderMusic;
    [Space]
    [SerializeField] private TextMeshProUGUI valueSound;
    [SerializeField] private Slider sliderSound;
    [Space]
    [SerializeField] private TextMeshProUGUI valueTargetFPS;
    [SerializeField] private Slider sliderTargetFPS;
    [Space]
    [SerializeField] private TextMeshProUGUI valueVSync;
    [SerializeField] private Slider sliderVSync;
    [Space]
    [SerializeField] private Toggle valueAnimTrava;
    [Space]
    [SerializeField] private GUIButton exit;
    [SerializeField] private GameObject settingsScreen;

    private void Awake()
    {
        Load();
        ApplySettings();
    }

    private void OnEnable()
    {
        exit.Click += delegate { SaveHeandler.ExportSettings(); };
        exit.Click += delegate { settingsScreen.gameObject.SetActive(false); };
    }

    private void OnDisable()
    {
        exit.Click -= delegate { SaveHeandler.ExportSettings(); };
        exit.Click -= delegate { settingsScreen.gameObject.SetActive(false); };
    }

    public void Exit() => Application.Quit();
    public void OpenURL(string _url) => Application.OpenURL(_url);

    private void OnSetValueAlfaGUI(float _value)
    {
        SaveHeandler.Settings.alfaUi = _value;
        valueAlfaGUI.text = (System.Math.Round(_value, 2, System.MidpointRounding.ToEven) * 100).ToString() + "%";
        foreach (Image img in imagesGUI)
            img.color = new Color(img.color.r, img.color.g, img.color.b, _value);
        ApplySettings();
    }
    private void OnSetValueMusic(float _value)
    {
        SaveHeandler.Settings.volMusic = _value;
        valueMusic.text = _value.ToString() + "%";
        ApplySettings();
    }
    private void OnSetValueSound(float _value)
    {
        SaveHeandler.Settings.volSound = _value;
        valueSound.text = _value.ToString() + "%";
        ApplySettings();
    }
    private void OnSetValueFrameRate(float _value)
    {
        SaveHeandler.Settings.FPSMode = (int)_value == sliderTargetFPS.maxValue ? -1 : (int)_value;
        valueTargetFPS.text = (int)_value == sliderTargetFPS.maxValue ? "MAX" : ((int)_value).ToString();
        ApplySettings();
    }
    private void OnSetVSyncMode(float _value)
    {
        SaveHeandler.Settings.vSync = (int)_value;
        valueVSync.text = ((int)_value).ToString();
        ApplySettings();
    } 
    private void OnSetAnimTravMode(bool _value) => SaveHeandler.Settings.travsAnim = _value;

    private void Load()
    {
        sliderTargetFPS.maxValue = sliderTargetFPS.maxValue + 1;

        sliderAlfaGUI.onValueChanged.AddListener(OnSetValueAlfaGUI);
        sliderMusic.onValueChanged.AddListener(OnSetValueMusic);
        sliderSound.onValueChanged.AddListener(OnSetValueSound);
        sliderTargetFPS.onValueChanged.AddListener(OnSetValueFrameRate);
        sliderVSync.onValueChanged.AddListener(OnSetVSyncMode);
        valueAnimTrava.onValueChanged.AddListener(OnSetAnimTravMode);

        sliderAlfaGUI.value = SaveHeandler.Settings.alfaUi;
        valueAlfaGUI.text = (System.Math.Round(SaveHeandler.Settings.alfaUi, 2, System.MidpointRounding.ToEven) * 100).ToString() + "%";
        foreach (Image img in imagesGUI)
            img.color = new Color(img.color.r, img.color.g, img.color.b, SaveHeandler.Settings.alfaUi);
        sliderMusic.value = SaveHeandler.Settings.volMusic;
        valueMusic.text = SaveHeandler.Settings.volMusic.ToString() + "%";
        sliderSound.value = SaveHeandler.Settings.volSound;
        valueSound.text = SaveHeandler.Settings.volSound.ToString() + "%";
        sliderTargetFPS.value = SaveHeandler.Settings.FPSMode;
        valueTargetFPS.text = ((int)SaveHeandler.Settings.FPSMode).ToString();
        sliderVSync.value = SaveHeandler.Settings.vSync;
        valueVSync.text = SaveHeandler.Settings.vSync.ToString();
        valueAnimTrava.isOn = SaveHeandler.Settings.travsAnim;

        ApplySettings();
    }

    private void ApplySettings()
    {
        Application.targetFrameRate = SaveHeandler.Settings.FPSMode == sliderTargetFPS.maxValue ? -1 : SaveHeandler.Settings.FPSMode;
        QualitySettings.vSyncCount = SaveHeandler.Settings.vSync;
    }
}
