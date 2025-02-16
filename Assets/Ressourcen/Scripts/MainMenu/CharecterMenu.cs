using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharecterMenu : MonoBehaviour
{
    public List<Button> listCarecters;
    public DataBase Data;

    [SerializeField] private RectTransform parent;
    [SerializeField] private UnityEngine.Object charecterObj;
    [Space]
    [SerializeField] private GameObject buttonPlay;
    [SerializeField] private TextMeshProUGUI textName;
    [SerializeField] private Image[] imgsPeopl;
    [Header("Create")]
    [SerializeField] private GameObject screenNew;
    [Space]
    [SerializeField] private Image face;
    [SerializeField] private TMP_InputField inputFieldName;
    [SerializeField] private TextMeshProUGUI[] charecteristicText;
    [SerializeField] private GameObject buttonPlayNew;
    [Space]
    [SerializeField] private ArmorObject nullArmor;

    private int _numCharecter = 0;

    private int[] _characteristic = new int[3] { 0, 0, 0 };
    private int _faceID = 0;
    private string _name;

    private int _score = 7;

    private void Start()
    {
        if (SaveHeandler.charecters.keysCharecters.Count > 0)
        {
            int _i = 0;

            foreach (string _token in SaveHeandler.charecters.keysCharecters.Keys)
            {
                if (_i == 0)
                    StaticValue.SessionToken = _token;

                GameObject _gObj = Instantiate(charecterObj, parent) as GameObject;

                _gObj.GetComponent<Button>().onClick.AddListener(delegate { OpenCharecter(_token); });
                _gObj.name = _token;
                _gObj.GetComponentsInChildren<TextMeshProUGUI>()[0].text = SaveHeandler.charecters.keysCharecters[_token].name;
                _gObj.GetComponentsInChildren<TextMeshProUGUI>()[1].text = " райн€€ сесси€:  " + SaveHeandler.charecters.keysCharecters[_token].endTimeSession;

                parent.sizeDelta = new Vector3(0, 200 + 175 * _i, 0);
                _gObj.GetComponent<RectTransform>().localPosition -= new Vector3(0, 175 * _i, 0);

                _i++;
            }

            SetVisualPlayer(StaticValue.SessionToken);

            buttonPlay.SetActive(true);
        }
        else
        {
            SetOutfitCharecter(-1);
            textName.text = "";
            buttonPlay.SetActive(false);
        }
    }

    private void Update()
    {
        for (int i = 0; i < 3; i++)
            charecteristicText[i].text = _characteristic[i].ToString();
    }

    public void OpenCharecter(string _token)
    {
        StaticValue.SessionToken = _token;
        SetVisualPlayer(StaticValue.SessionToken);
    }

    public void NewCharecter()
    {
        screenNew.SetActive(true);
        buttonPlayNew.SetActive(false);

        Item _i = Data.GetItem(140).Clone();
        SetOutfitCharecter(0, _i.armorObject);
        Destroy(_i);

        charecteristicText[0].transform.parent.GetComponent<RectTransform>().localPosition = new Vector3(-800, 250, 0);
        charecteristicText[1].transform.parent.GetComponent<RectTransform>().localPosition = new Vector3(-950, 50, 0);
        charecteristicText[2].transform.parent.GetComponent<RectTransform>().localPosition = new Vector3(-1100, -150, 0);

        face.transform.parent.GetComponent<RectTransform>().localPosition = new Vector3(500, 250, 0);

        StartCoroutine(AnimationMove(charecteristicText[0].transform.parent.GetComponent<RectTransform>(), new Vector3(150, 250, 0), 15));
        StartCoroutine(AnimationMove(charecteristicText[1].transform.parent.GetComponent<RectTransform>(), new Vector3(0, 50, 0), 15));
        StartCoroutine(AnimationMove(charecteristicText[2].transform.parent.GetComponent<RectTransform>(), new Vector3(-150, -150, 0), 15));

        StartCoroutine(AnimationMove(face.transform.parent.GetComponent<RectTransform>(), new Vector3(-160, 250, 0), 15));
    }

    public void ExitCreatMenuCharecter()
    {
        _characteristic = new int[3] { 0, 0, 0 };
        _faceID = 0;
        _name = null;

        inputFieldName.text = "";
        face.sprite = Data.faces[0];
        for (int i = 1; i < 3; i++)
            charecteristicText[i].text = "0";

        if (SaveHeandler.charecters.keysCharecters.Count > 0)
        {
            
        }
        else
        {
            SetOutfitCharecter(-1);
            textName.text = "";
            buttonPlay.SetActive(false);
        }
    }

    public void SetOutfitCharecter(int _face, ArmorObject _armor = null, Item _gun = null, LightObject _light = null, BackpackObject _backpack = null)
    {
        if (_face == -1)
        {
            for (int i = 0; i < imgsPeopl.Length; i++)
                if (imgsPeopl[i] != null)
                    imgsPeopl[i].color = Color.black;
            return;
        }
        else
        {
            for (int i = 0; i < imgsPeopl.Length; i++)
                if (imgsPeopl[i] != null)
                    imgsPeopl[i].color = Color.white;
        }

        imgsPeopl[0].sprite = Data.faces[_face];
        if (_light != null)
           imgsPeopl[1].sprite = _light.img;
        //imgsPeopl[2].sprite = маска на лицо;
        imgsPeopl[3].sprite = _armor.ImgBody;
        //imgsPeopl[4].sprite = _list.armor.ImgBody;
        if (_backpack != null)
            imgsPeopl[5].sprite = _backpack.img;
        imgsPeopl[6].sprite = _armor.ImgLeg;
        if (_gun != null)
            imgsPeopl[7].sprite = _gun.img;
        imgsPeopl[8].sprite = _armor.ImgHand;
        imgsPeopl[9].sprite = _armor.ImgHand;
    }

    public void InputCharecteristic(string _char)
    {
        if (_char[1] == '+' && _score > 0 && _characteristic[int.Parse(_char[0].ToString())] < 5)
        {
            _characteristic[int.Parse(_char[0].ToString())] += 1;
            _score -= 1;
        }
        else if (_char[1] == '-' && _characteristic[int.Parse(_char[0].ToString())] > 0)
        {
            _characteristic[int.Parse(_char[0].ToString())] -= 1;
            _score += 1;
        }
    }

    public void InputFace(string _char)
    {
        if (_char[0] == '+' && _faceID + 1 < Data.faces.Length)
            _faceID += 1;
        else if (_char[0] == '-' && _faceID > 0)
            _faceID -= 1;

        Item _i = Data.GetItem(140).Clone();
        SetOutfitCharecter(_faceID, _i.armorObject);
        Destroy(_i);
        face.sprite = Data.faces[_faceID];
    }

    public void InputName()
    {
        _name = inputFieldName.text;
        if (_name != "" && _name != null)
            buttonPlayNew.SetActive(true);
        else
            buttonPlayNew.SetActive(false);
    }

    public void Create()
    {
        SaveHeandler.NewCharecter(_name, _faceID, _characteristic);
        Play();
    }

    public void Play()
    {
        SaveHeandler.StartSession();
        SceneManager.LoadScene(SaveHeandler.SessionSave.idScene, LoadSceneMode.Single);
    }

    private int FindItem(int _cell, string _token)
    {
        foreach (SavesItem _item in SaveHeandler.charecters.keysCharecters[_token].items)
            if (_item.cellsId[0] == _cell)
                return _item.idItem;

        return -1;
    }

    private void SetVisualPlayer(string _token)
    {
        int[] _ids = new int[5]
        {
                SaveHeandler.charecters.keysCharecters[_token].idFace,
                FindItem(104, _token) != -1 ? FindItem(104, _token) : -1,
                FindItem(101, _token) != -1 ? FindItem(101, _token) : -1,
                FindItem(106, _token) != -1 ? FindItem(106, _token) : -1,
                FindItem(105, _token) != -1 ? FindItem(105, _token) : -1
        };

        ArmorObject _armor = _ids[1] != -1 ? Data.GetItem(_ids[1]).Clone().armorObject : null;
        Item _gun = _ids[2] != -1 ? Data.GetItem(_ids[2]).Clone() : null;
        LightObject _light = _ids[3] != -1 ? Data.GetItem(_ids[3]).Clone().lightObject : null;
        BackpackObject _backpack = _ids[4] != -1 ? Data.GetItem(_ids[4]).Clone().backpackObject : null;

        SetOutfitCharecter(_ids[0], _armor, _gun, _light, _backpack);
        textName.text = SaveHeandler.charecters.keysCharecters[_token].name;
    }

    IEnumerator AnimationMove(RectTransform _transform, Vector3 _target, float _speed)
    {
        while (IsTargetValue(_transform.localPosition, _target, true))
        {
            _transform.localPosition = Vector3.MoveTowards(_transform.localPosition, _target, _speed * Time.deltaTime * 100);

            yield return new WaitForEndOfFrame();
        }
    }

    private bool IsTargetValue(Vector3 current, Vector3 target, bool invers = false, float range = 0.1f)
    {
        Vector3 _v = current - target;

        if (Math.Abs(_v.x) <= range &&
            Math.Abs(_v.y) <= range &&
            Math.Abs(_v.z) <= range)
        {
            if (!invers)
                return true;

            return false;
        }

        if (!invers)
            return false;

        return true;
    }
}
