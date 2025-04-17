using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUIBodyParth : MonoBehaviour
{
    public event Action<BodyParthColider> OnUse;

    public GUIButton button;
    public BodyParthColider col;

    [SerializeField] private Image img;
    [SerializeField] private Image sliderImg;
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI text;

    private void OnEnable()
    {
        if (button)
            button.Click += OnSetMedicals;
    }

    private void OnDisable()
    {
        if (button)
            button.Click -= OnSetMedicals;
    }

    public void OnChengeHP(BodyParthMeta _parth, IMetaEssence _mata)
    {
        if (_parth.Hp <= 0)
        {
            if (img)
                img.color = new Color32(0, 0, 0, 200);
            if (sliderImg)
                sliderImg.color = new Color(1, 0, 0, 0.8f);
        }
        else
        {
            float _reason = (_parth.baseHp - _parth.Hp) / _parth.baseHp;

            if (Math.Round(_reason, 3, MidpointRounding.AwayFromZero) <= 0.5f)
            {
                float r = 2 * (float)Math.Round(_reason, 3, MidpointRounding.AwayFromZero);
                if (img)
                    img.color = new Color(r, 1, 0, 0.8f);
                if (sliderImg)
                    sliderImg.color = new Color(r, 1, 0, 0.8f);
            }
            else
            {
                float g = 2 * (1 - (float)Math.Round(_reason, 3, MidpointRounding.AwayFromZero));
                if (img)
                    img.color = new Color(1, g, 0, 0.8f);
                if (sliderImg)
                    sliderImg.color = new Color(1, g, 0, 0.8f);
            }
        }

        if (slider)
            slider.value = _parth.Hp;

        if (text)
            text.text = _parth.Hp + "/" + _parth.baseHp;
    }

    private void OnSetMedicals(string str) => OnUse?.Invoke(col);
}
