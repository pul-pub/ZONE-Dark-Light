using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUIDialogs : MonoBehaviour
{
    public event Action<List<StaticItem>> AddItems;
    public event Action<int> GiveMoney;
    public event Action<Quest> AddNewQuest;
    public event Action<IMetaEssence> DeathPlayer;
    public event Action<ShopObject> OpenStor;
    public event Action<Entry> Entrying;

    [SerializeField] private DataBase data;
    [Header("---------  Screen  ---------")]
    [SerializeField] private GameObject screen;
    [Space]
    [SerializeField] private TextMeshProUGUI textGroup;
    [SerializeField] private TextMeshProUGUI textName;
    [SerializeField] private TextMeshProUGUI textNPC;
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
    [Space]
    [SerializeField] private Sprite[] spritsAnswer;
    [SerializeField] private GameObject[] answers;

    private List<Image> imgsAnswer = new();
    private List<TextMeshProUGUI> textAnswer = new();
    private List<GUIButton> buttonAnswer = new();
    
    private Dialog _now;
    private IMetaEssence _metaEs;

    private void Awake()
    {
        for (int i = 0; i < answers.Length; i++)
        {
            buttonAnswer.Add(answers[i].GetComponent<GUIButton>());
            imgsAnswer.Add(answers[i].GetComponentInChildren<Image>());
            textAnswer.Add(answers[i].GetComponentInChildren<TextMeshProUGUI>());
        }
    }

    private void OnEnable()
    {
        foreach (GUIButton gButton in buttonAnswer)
            gButton.Click += OnPressAnswer;
    }

    private void OnDisable()
    {
        foreach (GUIButton gButton in buttonAnswer)
            gButton.Click -= OnPressAnswer;
    }

    public void SetDialog(IMetaEssence _meta, Dialog _dialog = null)
    {
        if (!screen.activeSelf)
        {
            screen.SetActive(true);

            textName.text = _meta.Name;
            switch (_meta.Group)
            {
                case TypeGroup.millitary:
                    textGroup.text = "Военные";
                    break;
                case TypeGroup.scientist:
                    textGroup.text = "Учёные";
                    break;
                case TypeGroup.Mutant:
                    textGroup.text = "Мутанты";
                    break;
                case TypeGroup.bandut:
                    textGroup.text = "Бандиты";
                    break;
                case TypeGroup.clearSky:
                    textGroup.text = "Чистое Небо";
                    break;
                case TypeGroup.svoboda:
                    textGroup.text = "Свобода";
                    break;
                case TypeGroup.dolg:
                    textGroup.text = "Долг";
                    break;
                case TypeGroup.stalker:
                    textGroup.text = "Сталкеры";
                    break;
            }

            ViewEssence viv = _meta.Visual;

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

        if (_dialog.questReward)
        {
            if (_dialog.GiveItems.Count > 0)
                AddItems?.Invoke(_dialog.GiveItems);
            if (_dialog.GiveMoney > 0)
                GiveMoney?.Invoke(_dialog.GiveMoney);
        }

        _now = _dialog;
        _metaEs = _meta;
        string addString = "";
        if (_dialog.GiveItems.Count > 0)
        {
            addString += "\n<color=#FFD700>[ ПОЛУЧЕНО: ";
            for (int j = 0; j < _dialog.GiveItems.Count; j++)
            {
                addString += _dialog.GiveItems[j].count + "x " + data.GetItem(_dialog.GiveItems[j].id).Name;
                addString += j + 1 == _dialog.GiveItems.Count ? "" : ", ";
            }

            addString += " ]</color>";
        }
        textNPC.text = _dialog.text + addString;

        for (int i = 0; i < 3; i++)
            textAnswer[i].transform.parent.gameObject.SetActive(false);

        for (int i = 0; i < _dialog.answers.Count; i++)
        {
            textAnswer[i].transform.parent.gameObject.SetActive(true);
            textAnswer[i].text = _dialog.answers[i].text;

            imgsAnswer[i].gameObject.SetActive(_dialog.answers[i].typeDescriptions != TypeDescription.NextDialog);

            switch (_dialog.answers[i].typeDescriptions)
            {
                case TypeDescription.Quest:
                    imgsAnswer[i].sprite = spritsAnswer[0];
                    break;
                case TypeDescription.Buy:
                    imgsAnswer[i].sprite = spritsAnswer[1];
                    break;
                case TypeDescription.Sale:
                    imgsAnswer[i].sprite = spritsAnswer[2];
                    break;
                case TypeDescription.Exit:
                    imgsAnswer[i].sprite = spritsAnswer[4];
                    break;
                case TypeDescription.WalkTo:
                    imgsAnswer[i].sprite = spritsAnswer[3];
                    break;
            }
        }
    }

    public void OnPressAnswer(string _in)
    {
        int _num = int.Parse(_in);
        
        switch (_now.answers[_num].typeDescriptions)
        {
            case TypeDescription.NextDialog:
                SetDialog(_metaEs, _now.answers[_num].nextDialog);
                break;

            case TypeDescription.Quest:
                if (_now.NameNPC == "Лебедев" && _now.Id == 3)
                    SaveHeandler.SessionNow.SetSwitchObject("InScene", false);
                if (_now.NameNPC == "Нарцисс" && _now.Id == 3)
                {
                    SaveHeandler.SessionNow.SetSwitchObject("BaseNarciss", false);
                    SaveHeandler.SessionNow.SetSwitchObject("FightNarciss", true);
                }

                AddNewQuest?.Invoke(_now.answers[_num].quest);
                screen.SetActive(false);
                _now = null;
                break;

            case TypeDescription.WalkTo:
                if (_now.NameNPC == "Каратель")
                    SaveHeandler.SessionNow.SetSwitchObject("Karatel", false);
                Entry _ent = new Entry();
                _ent.meta = _now.answers[_num].metaEntry;
                Entrying?.Invoke(_ent);
                break;

            case TypeDescription.Dide:
                DeathPlayer?.Invoke(_metaEs);
                break;

            case TypeDescription.Buy:
                OpenStor?.Invoke(_now.answers[_num].Shop);
                screen.SetActive(false);
                _now = null;
                break;

            case TypeDescription.Exit:
                if (_now.NameNPC == "Сварог" && _now.Id == 0)
                {
                    SaveHeandler.SessionNow.SetSwitchObject("SvarogS", false);
                    SaveHeandler.SessionNow.SetSwitchObject("InScene", true);
                }

                screen.SetActive(false);
                _now = null;
                _metaEs = null;
                break;
        }
    }
}

[Serializable]

public class ViewEssence
{
    public Sprite Light;
    public Sprite Mask;
    public Sprite Face;

    public Sprite Body;
    public Sprite Body2;
    public Sprite Backpack;
    public Sprite Gun;
    public Sprite Pistol;

    public Sprite Hand;

    public Sprite Leg;
}
