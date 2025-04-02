using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class LightManager : MonoBehaviour
{
    public bool HaveLight => _light;
    public bool OnLight = false;

    [SerializeField] private DataBase data;

    [SerializeField] private WorldTime worldTime;

    [SerializeField] private SpriteRenderer spRender;
    [SerializeField] private Light2D light2D;
    [SerializeField] private GameObject lightFanric;
    [SerializeField] private Volume volume;
    private VolumeProfile _baseVolume;
    private bool _isGlobalLight;

    private LightObject _light;

    public void Awake()
    {
        if (worldTime)
            _isGlobalLight = worldTime.IsGlobalLight;
        if (volume)
            _baseVolume = volume.profile;
    }

    public Sprite GetImages() => _light?.ImgLight;

    public void OnResetOutfit(Dictionary<string, IItem> _items)
    {
        if (_items.TryGetValue("LIT", out IItem _item))
            _light = data.GetLight(_item.Id);
        else
            _light = null;

        UpdateGrafic();
    }

    public void OnSetLight()
    {
        if (_light)
        {
            OnLight = !OnLight;
            volume.profile = OnLight && _light.TypeLight == TypeLight.PNV ? _light.Profile : _baseVolume;
            if (OnLight && _light.TypeLight == TypeLight.PNV)
                light2D.intensity = 0.5f;
            worldTime.IsGlobalLight = OnLight && _light.TypeLight == TypeLight.PNV ? false : _isGlobalLight;
            lightFanric.SetActive(OnLight && _light.TypeLight != TypeLight.PNV);
        }
    }

    private void UpdateGrafic()
    {
        spRender.gameObject.SetActive(HaveLight);
        spRender.sprite = _light ? _light.ImgLight : null;

        if (_light)
        {
            volume.profile = OnLight && _light.TypeLight == TypeLight.PNV ? _light.Profile : _baseVolume;
            if (OnLight && _light.TypeLight == TypeLight.PNV)
                light2D.intensity = 0.5f;
            worldTime.IsGlobalLight = OnLight && _light.TypeLight == TypeLight.PNV ? false : _isGlobalLight;
            lightFanric.SetActive(OnLight && _light.TypeLight != TypeLight.PNV);
        }
    }

    public void Save() => SaveHeandler.SessionNow.onLight = OnLight;
    public void Load() => OnLight = SaveHeandler.SessionNow.onLight;
}
