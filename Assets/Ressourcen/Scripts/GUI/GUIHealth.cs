using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUIHealth : MonoBehaviour
{
    [Header("Overly")]
    [SerializeField] private Image[] imgOverlyBody;
    [Header("Body Parth Menu")]
    [SerializeField] private GameObject screenMenu;
    [SerializeField] private Image[] imgMenuBody;
    [SerializeField] private Image[] imgMenuSliderHP;
    [SerializeField] private List<Slider> sliderHP;
    [SerializeField] private List<TextMeshProUGUI> textHp;
    [Header("Medic Menu")]
    [SerializeField] private GameObject medicMenu;
    [SerializeField] private GameObject duscriptMenu;
    [Space]
    [SerializeField] private Image[] imgMedicBody;
    [SerializeField] private Image[] imgMedicSliderHP;
    [SerializeField] private List<Slider> sliderHPMedic;
    [SerializeField] private List<TextMeshProUGUI> textHpMedic;

    private ObjectItem _medic;
    private Dictionary<string, BodyParthColider> _parths = new();
    private List<BodyParthColider> _bodyList;

    public void Use(ObjectItem _item)
    {
        if (_item.item.medicObject != null)
        {
            medicMenu.SetActive(true);
            _medic = _item;
        }
    }

    public void DoMedic(string _key) 
    { 
        foreach (string  k in _parths.Keys)
        {
            if (k == _key)
            {
                _parths[k].ApplyMedic(_medic.item.medicObject.RecoveryHP);
                if (_medic.count > 1)
                    _medic.count--;
                else
                {
                    _medic.Delete();
                    medicMenu.SetActive(false);
                    duscriptMenu.SetActive(false);
                }

                SetOverlyBody(_bodyList);
                SetBodyMenu(_bodyList);
            }
        }
    }

    public void SetMedicMenu(List<BodyParthColider> _body)
    {
        SetBodyColor(_body, imgMedicBody);
        SetBodyColor(_body, imgMedicSliderHP);

        for (int i = 0; i < 5; i++)
        {
            sliderHPMedic[i].value = _body[i].BodyParth.Hp;
            textHpMedic[i].text = _body[i].BodyParth.baseHp + "/" + _body[i].BodyParth.Hp;

            if (_body[i].BodyParth.Hp < _body[i].BodyParth.baseHp)
                textHpMedic[i].transform.parent.parent.gameObject.SetActive(true);
            else
                textHpMedic[i].transform.parent.parent.gameObject.SetActive(false);
        }

        if (_parths.Keys.Count == 0)
        {
            _parths.Add("Head", _body[0]);
            _parths.Add("Body", _body[1]);
            _parths.Add("HandL", _body[2]);
            _parths.Add("HandR", _body[3]);
            _parths.Add("Leg", _body[4]);
        }
        
        _bodyList ??= _body;
    }

    public void SetOverlyBody(List<BodyParthColider> _body)
    {
        SetBodyColor(_body, imgOverlyBody);
    }

    public void SetBodyMenu(List<BodyParthColider> _body)
    {
        SetBodyColor(_body, imgMenuBody);
        SetBodyColor(_body, imgMenuSliderHP);

        for (int i = 0; i < 5; i++)
        {
            sliderHP[i].value = _body[i].BodyParth.Hp;
            textHp[i].text = _body[i].BodyParth.baseHp + "/" + _body[i].BodyParth.Hp;
        }
    }

    private void SetBodyColor(List<BodyParthColider> _body, Image[] _list)
    {
        for (int i = 0; i < 5; i++)
        {
            if (_body[i].BodyParth.Hp == 0)
            {
                _list[i].color = new Color32(0, 0, 0, 200);
            }
            else
            {
                float _reason = (_body[i].BodyParth.baseHp - _body[i].BodyParth.Hp) / _body[i].BodyParth.baseHp;

                if (Math.Round(_reason, 3, MidpointRounding.AwayFromZero) <= 0.5f)
                {
                    float r = 2 * (float)Math.Round(_reason, 3, MidpointRounding.AwayFromZero);

                    _list[i].color = new Color(r, 1, 0, 0.8f);
                }
                else
                {
                    float g = 2 * (1 - (float)Math.Round(_reason, 3, MidpointRounding.AwayFromZero));

                    _list[i].color = new Color(1, g, 0, 0.8f);
                }
            }
        }
    }
}
