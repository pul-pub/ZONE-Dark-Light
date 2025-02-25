using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class LightManager : MonoBehaviour
{
    public bool HaveLight => _lightObject != null;
    public bool OnLight = false;

    [SerializeField] private WorldTime worldTime;

    [SerializeField] private SpriteRenderer spRender;
    [SerializeField] private Light2D light2D;
    [SerializeField] private GameObject lightFanric;
    [SerializeField] private Volume volume;
    [SerializeField] private VolumeProfile baseProfile;
    [SerializeField] private bool baseIsGrafic;

    private LightObject _lightObject;
    private bool isLoaged = false;

    private void OnEnable()
    {
        SaveHeandler.OnSaveSession += Save;
    }

    private void OnDisable()
    {
        SaveHeandler.OnSaveSession -= Save;
    }

    public void OnResetOutfit(Item[] _items)
    {
        if (_items[4] != null)
        {
            if (_lightObject != _items[4].lightObject && (lightFanric.activeSelf || volume.profile != baseProfile))
            {
                lightFanric.SetActive(false);
                volume.profile = baseProfile;
            }

            _lightObject = _items[4].lightObject;

            spRender.gameObject.SetActive(true);
            spRender.sprite = _lightObject.img;
        }
        else
        {
            if (lightFanric.activeSelf || volume.profile != baseProfile)
            {
                lightFanric.SetActive(false);
                volume.profile = baseProfile;
            }

            _lightObject = null;

            spRender.gameObject.SetActive(false);
        }

        Debug.Log(SaveHeandler.SessionSave.onLight);
        if (SaveHeandler.SessionSave.onLight && !isLoaged)
        {
            isLoaged = true;
            OnSetLight();
        }   
    }

    public void OnSetLight()
    {
        if (_lightObject != null)
        {
            if (lightFanric.activeSelf || volume.profile != baseProfile)
            {
                lightFanric.SetActive(false);
                volume.profile = baseProfile;

                if (_lightObject.typeLight == TypeLight.PNV)
                {
                    worldTime.IsSetGragics = baseIsGrafic;
                    light2D.intensity = StaticValue.lightLevel;
                }

                OnLight = false;
            }
            else
            {
                if (_lightObject.typeLight == TypeLight.PNV)
                {
                    volume.profile = _lightObject.profile;
                    worldTime.IsSetGragics = false;
                    light2D.intensity = 0.5f;
                }
                else
                    lightFanric.SetActive(true);

                OnLight = true;
            }
        } 
    }

    private void Save() => SaveHeandler.SessionSave.onLight = OnLight;
}
