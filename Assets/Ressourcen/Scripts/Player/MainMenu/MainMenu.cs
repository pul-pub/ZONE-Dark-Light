using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

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
    [SerializeField] private Toggle valueVSync;
    [Space]
    [SerializeField] private Toggle valueAnimTrava;
    [Space]
    [SerializeField] private GUIButton exit;
    [SerializeField] private GameObject settingsScreen;

    private void Awake()
    {
        sliderAlfaGUI.onValueChanged.AddListener(OnSetValueAlfaGUI);
        sliderMusic.onValueChanged.AddListener(OnSetValueMusic);
        sliderSound.onValueChanged.AddListener(OnSetValueSound);
        sliderTargetFPS.onValueChanged.AddListener(OnSetValueFrameRate);
        valueVSync.onValueChanged.AddListener(OnSetVSyncMode);
        valueAnimTrava.onValueChanged.AddListener(OnSetAnimTravMode);

        sliderAlfaGUI.value = SaveHeandler.Settings.alfaUi;
        valueAlfaGUI.text = (System.Math.Round(SaveHeandler.Settings.alfaUi, 2, System.MidpointRounding.ToEven)).ToString();
        foreach (Image img in imagesGUI)
            img.color = new Color(img.color.r, img.color.g, img.color.b, SaveHeandler.Settings.alfaUi);
        sliderMusic.value = SaveHeandler.Settings.volMusic;
        valueMusic.text = SaveHeandler.Settings.volMusic.ToString();
        sliderSound.value = SaveHeandler.Settings.volSound;
        valueSound.text = SaveHeandler.Settings.volSound.ToString();
        sliderTargetFPS.value = SaveHeandler.Settings.FPSMode;
        valueTargetFPS.text = ((int)SaveHeandler.Settings.FPSMode).ToString();
        valueVSync.isOn = SaveHeandler.Settings.vSync;
        valueAnimTrava.isOn = SaveHeandler.Settings.travsAnim;
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

    private void OnSetValueAlfaGUI(float _value)
    {
        SaveHeandler.Settings.alfaUi = _value;
        valueAlfaGUI.text = (System.Math.Round(_value, 2, System.MidpointRounding.ToEven)).ToString();
        foreach (Image img in imagesGUI)
            img.color = new Color(img.color.r, img.color.g, img.color.b, _value);
    }
    private void OnSetValueMusic(float _value)
    {
        SaveHeandler.Settings.volMusic = _value;
        valueMusic.text = _value.ToString();
    }
    private void OnSetValueSound(float _value)
    {
        SaveHeandler.Settings.volSound = _value;
        valueSound.text = _value.ToString();
    }
    private void OnSetValueFrameRate(float _value)
    {
        SaveHeandler.Settings.FPSMode = (int)_value;
        valueTargetFPS.text = ((int)_value).ToString();
    }
    private void OnSetVSyncMode(bool _value) => SaveHeandler.Settings.vSync = _value;
    private void OnSetAnimTravMode(bool _value) => SaveHeandler.Settings.travsAnim = _value;
}
