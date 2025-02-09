using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GUIHandler : MonoBehaviour
{
    public Inventory inventory;
    public GUIDetector Detector;
    public GUIHealth Health;
    public GUICall Call;
    [SerializeField] private GameObject inventoryObject;
    [SerializeField] private GameObject inputUI;
    [SerializeField] private GameObject diskriptObject;
    [SerializeField] private GameObject otfitObject;
    [SerializeField] private GameObject otfitObj;
    [SerializeField] private GameObject npcObject;
    [SerializeField] private FixedJoystick fixedJoystick;
    [SerializeField] private FixedJoystick bolt;
    [Header("Weapon")]
    [SerializeField] private Image[] imgSetNumWeapon;
    [SerializeField] private TextMeshProUGUI[] textAmmo;
    [Header("Mass")]
    [SerializeField] private TextMeshProUGUI[] textMass;
    [Header("Health")]
    [SerializeField] private TextMeshProUGUI textHealth;
    [SerializeField] private GUIDideScreen dideScreen;
    [Header("Energy")]
    [SerializeField] private TextMeshProUGUI textEnergy;
    [SerializeField] private Slider sliderEnergy;
    [Header("Dialogs")]
    [SerializeField] private GameObject screenDialogs;
    [Space]
    [SerializeField] private TextMeshProUGUI textGroup;
    [SerializeField] private TextMeshProUGUI textName;
    [SerializeField] private Image[] imgsPeopl;
    [Space]
    [SerializeField] private TextMeshProUGUI textDescription;
    [Space]
    [SerializeField] private Sprite[] spritsAnswer;
    [SerializeField] private Image[] imgsAnswer;
    [SerializeField] private TextMeshProUGUI[] textAnswer;
    [Header("Quest")]
    [SerializeField] QuestManager questManager;
    [Space]
    [SerializeField] TextMeshProUGUI titleQuest;
    [SerializeField] TextMeshProUGUI descriptionQuest;
    [Space]
    [SerializeField] RectTransform marker;
    [SerializeField] RectTransform markerZone;
    [SerializeField] MapObject map;
    [Header("Button for Interection")]
    [SerializeField] private RectTransform rectTrInterection;
    [SerializeField] private TextMeshProUGUI textInterection;
    [Header("Input Buttons")]
    [SerializeField] private RectTransform buttonShoot;
    [SerializeField] private RectTransform buttonReload;
    [SerializeField] private RectTransform buttonLight;
    [Header("Entry Screen")]
    [SerializeField] private Image entryScreen;

    public IInput input;

    private Vector3 _posQuest;
    private Camera _cam;

    private Dialog _dialogNow;

    private NPC _npcPack;
    private DialogList _dialogList;
    private Dialog _dialog;
    private Entry _entry;

    private Coroutine _animationUp;

    private Coroutine[] _animButton = new Coroutine[3];

    private bool[] _activButton = new bool[3] { false, false, false };

    private void OnEnable()
    {
        if (input != null)
        {
            inventory.OnChangeOutfit += OnSetItemOutfit;
            inventory.Disct.OnUse += Use;
            inventory.Disct.OnUse += Health.Use;

            bolt.OnEndDown += input.ReadOnCastBolt;
        }


    }
    
    private void OnDisable()
    {
        if (input != null)
        {
            inventory.OnChangeOutfit -= OnSetItemOutfit;
            inventory.Disct.OnUse -= Use;
            inventory.Disct.OnUse -= Health.Use;

            bolt.OnEndDown -= input.ReadOnCastBolt;
        }
    }

    public void Awake()
    {
        input = new MobileInput(fixedJoystick);
        //input = new DesctopInput();

        inventory.OnChangeOutfit += OnSetItemOutfit;

        bolt.OnEndDown += input.ReadOnCastBolt;
        _cam = Camera.main;
        //inventory.CreateList();
    }

    private void Update()
    {
        input.ReadMovement();
        
        if (marker.gameObject.activeSelf)
        {
            Vector2 _o = _cam.WorldToScreenPoint(_posQuest);

            if (_o.x < Screen.width && _o.x > 0)
                marker.position = new Vector3(_o.x, _o.y, 0);
            else
            {
                if (_o.x > 0)
                    marker.position = new Vector3(Screen.width, _o.y, 0);
                else
                    marker.position = new Vector3(0, _o.y, 0);
            }
        }
    }

    private void Use(ObjectItem _item)
    {
        if (_item.item.detectorObject)
        {
            Detector.SetActiv(_item.item.detectorObject);

            diskriptObject.SetActive(false);
            inventoryObject.SetActive(false);
            inputUI.SetActive(true);
        }    
    }

    public void SetIsShoot(bool _isActiv) => input.ReadButtonShoot(_isActiv);
    public void SetReload() => input.ReadButtonReload();
    public void SetLight() => input.ReadButtonLight();
    public void GetAllBackpack() => inventory.GiveAllItem();
    public void SetNumWeapon(int _num) => input.ReadNumWeapon(_num);
    public void SetActivInv(bool _is)
    {
        inventoryObject.SetActive(_is);
        otfitObject.SetActive(_is);
        otfitObj.SetActive(_is);
        npcObject.SetActive(false);
        inventory.SetActivOutfit(_is);
    }

    public void ButtonInterection(DialogList _list = null, Dialog _d = null, NPC _pack = null, Entry _entr = null)
    {
        if (_list || _dialog || _pack || _entr)
        {
            if (_animationUp != null)
            {
                StopCoroutine(_animationUp);
                _animationUp = null;
            }

            //rectTrInterection.gameObject.SetActive(true);
            _animationUp = StartCoroutine(AnimationMove(rectTrInterection, new Vector3(0, -200, 0), 15f));

            if (_list)
            {
                _dialogList = _list;
                _dialog = _d;
                _npcPack = null;
                _entry = null;

                textInterection.text = "Говорить";
            }
            else if (_pack)
            {
                _dialogList = null;
                _dialog = null;
                _npcPack = _pack;
                _entry = null;

                textInterection.text = "Обыскать";
            }
            else if (_entr)
            {
                textInterection.text = "Перейти";

                _dialogList = null;
                _dialog = null;
                _npcPack = null;
                _entry = _entr;
            }
            else
            {
                textInterection.text = "";

                _dialogList = null;
                _dialog = null;
                _npcPack = null;
                _entry = null;
            }
        }
        else
        {
            if (_animationUp != null)
            {
                StopCoroutine(_animationUp);
                _animationUp = null;
            }

            _dialogList = null;
            _dialog = null;
            _npcPack = null;
            _entry = null;

            //rectTrInterection.gameObject.SetActive(false);
            _animationUp = StartCoroutine(AnimationMove(rectTrInterection, new Vector3(0, -800, 0), 10f));
        }
    }
    public void OnPointDownInterection()
    {
        if (_dialogList)
        {
            Quest _q = questManager.FindActivQuest(_dialogList.Name);

            if (_q != null)
            {
                SetDialog(_dialogList, _q.startDialog);
                questManager.SetEndQuest(_q);
            }
            else
                SetDialog(_dialogList, _dialogList.startDialog);
        }
        else if (_npcPack)
        {
            OpenNPCPack();
            inventory.OnBackpackNPC(_npcPack);
        }
        else
        {
            SaveHeandler.SaveSession();
            SaveHeandler.SessionSave.pos.x = _entry.meta.posTo.x;
            SceneManager.LoadScene(_entry.meta.locationToID, LoadSceneMode.Single);
        }
    }

    public void Dide(IMetaEnemy _meta) => dideScreen.OnDide(_meta);
    public void OpenNPCPack()
    {
        inventoryObject.SetActive(true);
        otfitObject.SetActive(false);
        npcObject.SetActive(true);
    }
    public void SetDialog(DialogList _list, Dialog _dialog)
    {
        if (_dialog.GiveMoney > 0)
            inventory.money += _dialog.GiveMoney;
        if (_dialog.GiveItem)
            inventory.SetItem(_dialog.GiveItem, _dialog.GiveCountItem);

        if (!screenDialogs.activeSelf)
        {
            screenDialogs.SetActive(true);

            textName.text = _list.Name;
            textGroup.text = _list.Group;

            imgsPeopl[0].sprite = _list.face;
            if (_list.lightObj != null) 
                imgsPeopl[1].sprite = _list.lightObj.img;
            //imgsPeopl[2].sprite = маска на лицо;
            imgsPeopl[3].sprite = _list.armor.ImgBody;
            //imgsPeopl[4].sprite = _list.armor.ImgBody;
            if (_list.backpack != null)
                imgsPeopl[5].sprite = _list.backpack.img;
            imgsPeopl[6].sprite = _list.armor.ImgLeg;
            //imgsPeopl[7].sprite = плащ/накидка;
            imgsPeopl[8].sprite = _list.armor.ImgHand;
            imgsPeopl[9].sprite = _list.armor.ImgHand;
        }

        textDescription.text = _dialog.text;
        _dialogNow = _dialog;
        _dialogList = _list;

        for (int i = 0; i < 3; i++) 
        {
            if (i < _dialog.answers.Count)
            {
                textAnswer[i].gameObject.SetActive(true);
                textAnswer[i].text = _dialog.answers[i].text;

                if (_dialog.answers[i].typeDescriptions != TypeDescription.NextDialog)
                {
                    imgsAnswer[i].gameObject.SetActive(true);

                    if (_dialog.answers[i].typeDescriptions == TypeDescription.Quest)
                        imgsAnswer[i].sprite = spritsAnswer[0];
                    else if (_dialog.answers[i].typeDescriptions == TypeDescription.Buy)
                        imgsAnswer[i].sprite = spritsAnswer[1];
                    else if (_dialog.answers[i].typeDescriptions == TypeDescription.Sale)
                        imgsAnswer[i].sprite = spritsAnswer[2];
                    else if (_dialog.answers[i].typeDescriptions == TypeDescription.WalkTo)
                        imgsAnswer[i].sprite = spritsAnswer[3];
                    else
                        imgsAnswer[i].sprite = spritsAnswer[4];
                }
                else
                {
                    imgsAnswer[i].gameObject.SetActive(false);
                }
            }
            else
            {
                textAnswer[i].gameObject.SetActive(false);
                imgsAnswer[i].gameObject.SetActive(false);
            }
        }
    }
    public void OnPressAnswer(int _num)
    {
        if (_dialogNow != null)
        {
            if (_dialogNow.answers[_num].typeDescriptions == TypeDescription.NextDialog)
                SetDialog(null, _dialogNow.answers[_num].nextDialog);
            else if (_dialogNow.answers[_num].typeDescriptions == TypeDescription.Quest)
            {
                if (_dialogNow.NameNPC == "Лебедев" && _dialogNow.Id == 3)
                    SaveHeandler.SessionSave.SetSwitchObject("InScene", false);

                questManager.SetNewQuest(_dialogNow.answers[_num].quest);
                screenDialogs.SetActive(false);
                _dialogNow = null;
            }
            else if (_dialogNow.answers[_num].typeDescriptions == TypeDescription.WalkTo)
            {
                SaveHeandler.SessionSave.pos.x = _dialogNow.answers[_num].metaEntry.posTo.x;
                SaveHeandler.SessionSave.idScene = _dialogNow.answers[_num].metaEntry.locationToID;
               
                if (_dialogNow.NameNPC == "Каратель")
                    SaveHeandler.SessionSave.SetSwitchObject("Karatel", false);

                SaveHeandler.SaveSession();

                StartCoroutine(AnimationEntryed(_dialogNow.answers[_num].metaEntry.locationToID));
            }
            else if (_dialogNow.answers[_num].typeDescriptions == TypeDescription.Dide)
            {
                Debug.Log(_dialogList);
                Dide(_dialogList.npc);
            }
            else
            {
                if (_dialogNow.NameNPC == "Сварог" && _dialogNow.Id == 0)
                {
                    SaveHeandler.SessionSave.SetSwitchObject("SvarogS", false);
                    SaveHeandler.SessionSave.SetSwitchObject("InScene", true);
                }

                screenDialogs.SetActive(false);
                _dialogNow = null;
            } 
        }
    }

    public void UpdateQuest(Quest _quest)
    {
        if (_quest != null)
        {
            if (!descriptionQuest.gameObject.activeSelf || titleQuest.text != _quest.textTitell)
            {
                descriptionQuest.gameObject.SetActive(true);
                titleQuest.gameObject.SetActive(true);

                descriptionQuest.text = _quest.textDiscription;
                titleQuest.text = _quest.textTitell;
            }

            if (SceneManager.GetActiveScene().buildIndex == _quest.idScene)
                _posQuest = new Vector3(_quest.position.x, (_quest.position.y == 0 ? 2.5f : _quest.position.y), 0);
            else
            {
                EntryMeta _ent;

                if (_ent = map.FindPath(SceneManager.GetActiveScene().buildIndex, _quest.idScene))
                    _posQuest = new Vector3(_ent.posFrom.x, 2.5f, 0);
            }
        }
        else
        {
            descriptionQuest.gameObject.SetActive(false);
            titleQuest.gameObject.SetActive(false);
            marker.gameObject.SetActive(false);
        }
    }

    public void OnSetItemOutfit()
    {
        Item _item;

        if ((_item = inventory.FindItemCell(101)) != null)
        {
            imgSetNumWeapon[0].sprite = _item.img;
            imgSetNumWeapon[0].color = new Color(1, 1, 1, 1);
        }
        else
            imgSetNumWeapon[0].color = new Color(1, 1, 1, 0);

        if ((_item = inventory.FindItemCell(103)) != null)
        {
            imgSetNumWeapon[1].sprite = _item.img;
            imgSetNumWeapon[1].color = new Color(1, 1, 1, 1);
        }
        else
            imgSetNumWeapon[1].color = new Color(1, 1, 1, 0);

        Item[] _items = new Item[6];

        _items[0] = inventory.FindItemCell(101);
        _items[1] = inventory.FindItemCell(103);
        _items[2] = inventory.FindItemCell(104);
        _items[3] = inventory.FindItemCell(105);
        _items[4] = inventory.FindItemCell(106);
        _items[5] = inventory.FindItemCell(107);

        input.ReadResetOutfit(_items);
    }

    public void UpdateButtons(bool _shoot, bool _reload, bool _light)
    {
        if (_shoot != _activButton[0])
        {
            if (_animButton[0] != null)
            {
                StopCoroutine(_animButton[0]);
                _animButton[0] = null;
            }

            _animButton[0] = StartCoroutine(AnimationMove(buttonShoot, new Vector3(_shoot ? 75 : 550, _shoot ? -100: -550, 0), 15f));
            _activButton[0] = _shoot;
        }

        if (_reload != _activButton[1])
        {
            if (_animButton[1] != null)
            {
                StopCoroutine(_animButton[1]);
                _animButton[1] = null;
            }

            _animButton[1] = StartCoroutine(AnimationMove(buttonReload, new Vector3(_reload ? 225 : 500, 125, 0), 15f));
            _activButton[1] = _reload;
        }

        if(_light != _activButton[2])
        {
            if (_animButton[2] != null)
            {
                StopCoroutine(_animButton[2]);
                _animButton[2] = null;
            }

            _animButton[2] = StartCoroutine(AnimationMove(buttonLight, new Vector3(-120, _light ? 0 : 200, 0), 15f));
            _activButton[2] = _light;
        }
    }

    public void UpdateAmmos(int _allAmmos, int _ammo)
    {
        if (_ammo != -1)
        {
            textAmmo[0].text = _ammo.ToString();
            textAmmo[1].text = _allAmmos.ToString();
        }
        else
        {
            textAmmo[0].text = "--";
            textAmmo[1].text = "--";
        }
    }

    public void UpdateMass(int _maxMass, float _mass)
    {
        textMass[0].text = Math.Round(_mass, 4, MidpointRounding.AwayFromZero).ToString();
        textMass[1].text = "/ " + _maxMass.ToString();
    }

    public void UpdateHealth(float _health) => textHealth.text = ((int)_health).ToString();

    public void UpdateEnergy(float _energy)
    {
        textEnergy.text = ((int)_energy).ToString();
        sliderEnergy.value = ((int)_energy);
    }

    IEnumerator AnimationEntryed(int _locationToID)
    {
        entryScreen.gameObject.SetActive(true);
        byte _alfa = 0;
        while (entryScreen.color.a < 0.94f)
        {
            entryScreen.color = new Color32(0, 0, 0, _alfa);
            _alfa += 4;
            yield return new WaitForEndOfFrame();
            Debug.Log(entryScreen.color.a);
        }
        Debug.Log(_locationToID);
        SceneManager.LoadScene(_locationToID, LoadSceneMode.Single);
    }

    IEnumerator AnimationMove(RectTransform _transform, Vector3 _target, float _speed)
    {
        while (IsTargetValue(_transform.localPosition, _target, true))
        {
            _transform.localPosition = Vector3.MoveTowards(_transform.localPosition, _target, _speed * Time.deltaTime * 100);

            yield return new WaitForEndOfFrame();
        }

        _animationUp = null;
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
