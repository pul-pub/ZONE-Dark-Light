using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    #region IMGS ESSENCE
    [SerializeField] private Image imgLight;
    [SerializeField] private Image imgMask;
    [SerializeField] private Image imgFace;
    [SerializeField] private Image imgBody;
    [SerializeField] private Image imgBody2;
    [SerializeField] private Image imgBackpack;
    [SerializeField] private Image imgGun;
    [SerializeField] private Image imgPistol;
    [SerializeField] private Image imgHandR;
    [SerializeField] private Image imgHandL;
    [SerializeField] private Image imgLeg;
    #endregion
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
        if (SaveHeandler.ListCharecters.keysCharecters.Count > 0)
        {
            List<Character> list = new List<Character>();

            foreach (string _token in SaveHeandler.ListCharecters.keysCharecters.Keys)
                list.Add(SaveHeandler.ListCharecters.keysCharecters[_token]);

            List<Character> sortedSessions = list
            .Select(s => new
            {
                Session = s,
                DateParts = s.endTimeSession.Split(new[] { '.', ' ', ':' }) // Разделяем строку на части
            })
            .OrderByDescending(x => int.Parse(x.DateParts[0])) // Год (по убыванию)
            .ThenByDescending(x => int.Parse(x.DateParts[1]))   // Месяц (по убыванию)
            .ThenByDescending(x => int.Parse(x.DateParts[2]))   // День (по убыванию)
            .ThenByDescending(x => int.Parse(x.DateParts[3]))   // Час (по убыванию)
            .ThenByDescending(x => int.Parse(x.DateParts[4]))   // Минута (по убыванию)
            .ThenByDescending(x => int.Parse(x.DateParts[5]))   // Секунда (по убыванию)
            .Select(x => x.Session) // Возвращаем исходные объекты Session
            .ToList();

            int _i = 0;

            foreach (Character c in sortedSessions)
            {
                string _token = "";
                foreach (string _t in SaveHeandler.ListCharecters.keysCharecters.Keys)
                    if (SaveHeandler.ListCharecters.keysCharecters[_t] == c)
                        _token = _t;

                if (_i == 0)
                    StaticValue.SessionToken = _token;

                GameObject _gObj = Instantiate(charecterObj, parent) as GameObject;

                _gObj.GetComponent<Button>().onClick.AddListener(delegate { OpenCharecter(_token); });
                _gObj.name = _token;
                _gObj.GetComponentsInChildren<TextMeshProUGUI>()[0].text = SaveHeandler.ListCharecters.keysCharecters[_token].name;
                _gObj.GetComponentsInChildren<TextMeshProUGUI>()[1].text = "Крайняя сессия:  " + SaveHeandler.ListCharecters.keysCharecters[_token].endTimeSession;

                parent.sizeDelta = new Vector3(0, 200 + 175 * _i, 0);
                _gObj.GetComponent<RectTransform>().localPosition -= new Vector3(0, 175 * _i, 0);

                _i++;
            }

            SetVisualPlayer(StaticValue.SessionToken);

            buttonPlay.SetActive(true);
        }
        else
        {
            SetOutfitCharecter(null);
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

        SetOutfitCharecter(GetVisual("-"));

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
        face.sprite = Data.GetFace(_faceID);
        for (int i = 1; i < 3; i++)
            charecteristicText[i].text = "0";

        if (SaveHeandler.ListCharecters.keysCharecters.Count <= 0)
        {
            SetOutfitCharecter(null);
            textName.text = "";
            buttonPlay.SetActive(false);
        }
    }

    public void SetOutfitCharecter(ViewEssence viv)
    {
        imgLight.color = viv == null ? Color.black : Color.white;
        imgMask.color = viv == null ? Color.black : Color.white;
        imgFace.color = viv == null ? Color.black : Color.white;
        imgBody.color = viv == null ? Color.black : Color.white;
        imgBody2.color = viv == null ? Color.black : Color.white;
        imgBackpack.color = viv == null ? Color.black : Color.white;
        imgGun.color = viv == null ? Color.black : Color.white;
        imgPistol.color = viv == null ? Color.black : Color.white;
        imgHandL.color = viv == null ? Color.black : Color.white;
        imgHandR.color = viv == null ? Color.black : Color.white;
        imgLeg.color = viv == null ? Color.black : Color.white;

        if (viv != null)
        {
            imgLight.gameObject.SetActive(viv.Light);
            imgMask.gameObject.SetActive(viv.Mask);
            imgBody2.gameObject.SetActive(viv.Body2);
            imgBackpack.gameObject.SetActive(viv.Backpack);
            imgGun.gameObject.SetActive(viv.Gun);
            imgPistol.gameObject.SetActive(viv.Pistol);

            imgLight.sprite = viv.Light;
            imgMask.sprite = viv.Mask;
            imgFace.sprite = viv.Face;

            imgBody.sprite = viv.Body;
            imgBody2.sprite = viv.Body2;
            imgBackpack.sprite = viv.Backpack;
            imgGun.sprite = viv.Gun;
            imgPistol.sprite = viv.Pistol;

            imgHandL.sprite = viv.Hand;
            imgHandR.sprite = viv.Hand;

            imgLeg.sprite = viv.Leg;
        }
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
        if (_char[0] == '+' && _faceID + 1 < Data.GetFaceLen())
            _faceID += 1;
        else if (_char[0] == '-' && _faceID > 0)
            _faceID -= 1;

        SetOutfitCharecter(GetVisual("-", _faceID));
        face.sprite = Data.GetFace(_faceID);
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
        SceneManager.LoadScene(SaveHeandler.SessionNow.idScene, LoadSceneMode.Single);
    }

    private ViewEssence GetVisual(string _token, int _face = 0)
    {
        ViewEssence viw = new ViewEssence();

        viw.Face = Data.GetFace(_face);

        viw.Mask = Data.GetArmor("ARM000").ImgHead;
        viw.Body = Data.GetArmor("ARM000").ImgBody;
        viw.Hand = Data.GetArmor("ARM000").ImgHand;
        viw.Leg = Data.GetArmor("ARM000").ImgLeg;

        if (_token != "-")
        {
            foreach(SavesItem _item in SaveHeandler.ListCharecters.keysCharecters[_token].items)
            {
                switch (_item.cellsId[0])
                {
                    case 101:
                        viw.Gun = Data.GetGun(_item.idItem).Img;
                        break;

                    case 103:
                        viw.Pistol = Data.GetGun(_item.idItem).Img;
                        break;

                    case 104:
                        viw.Mask = Data.GetArmor(_item.idItem).ImgHead;
                        viw.Body = Data.GetArmor(_item.idItem).ImgBody;
                        viw.Hand = Data.GetArmor(_item.idItem).ImgHand;
                        viw.Leg = Data.GetArmor(_item.idItem).ImgLeg;
                        break;

                    case 105:
                        viw.Backpack = Data.GetBackpack(_item.idItem).ImgBackpack;
                        break;

                    case 106:
                        viw.Light = Data.GetLight(_item.idItem).ImgLight;
                        break;
                }
            }
        }

        return viw;
    }

    private void SetVisualPlayer(string _token)
    {
        SetOutfitCharecter(GetVisual(_token, SaveHeandler.ListCharecters.keysCharecters[_token].idFace));
        textName.text = SaveHeandler.ListCharecters.keysCharecters[_token].name;
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
