using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum TypeButton { Shoot, Reload, SetWeapon };

public class GUIHandler : MonoBehaviour
{
    [SerializeField] public Inventory inventory;
    [SerializeField] private GameObject inventoryObject;
    [SerializeField] private GameObject otfitObject;
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
    [SerializeField] private Slider sliderHealth;
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

    public IInput input;

    private Dialog _dialogNow;

    private void OnEnable()
    {
        if (input != null)
        {
            inventory.OnChangeOutfit += OnSetItemOutfit;
            input.OnInteractionPack += OpenNPCPack;

            bolt.OnEndDown += input.ReadOnCastBolt;
        }
    }

    private void OnDisable()
    {
        if (input != null)
        {
            inventory.OnChangeOutfit -= OnSetItemOutfit;
            input.OnInteractionPack -= OpenNPCPack;

            bolt.OnEndDown -= input.ReadOnCastBolt;
        }
    }

    public void Initialization()
    {
        input = new MobileInput(fixedJoystick);

        inventory.OnChangeOutfit += OnSetItemOutfit;
        input.OnInteractionPack += OpenNPCPack;

        bolt.OnEndDown += input.ReadOnCastBolt;

        inventory.CreateList();
    }

    private void Update()
    {
        input.ReadMovement();
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
        npcObject.SetActive(false);
        inventory.SetActivOutfit(_is);
    }
    public void SetPressMultiButton() => input.ReadPressMultiButton();
    public void OpenNPCPack(NPCBackpack _pack)
    {
        inventoryObject.SetActive(true);
        otfitObject.SetActive(false);
        npcObject.SetActive(true);
    }
    public void SetDialog(DialogList _list, Dialog _dialog)
    {
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
                    else
                        imgsAnswer[i].sprite = spritsAnswer[3];
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
            else
            {
                screenDialogs.SetActive(false);
                _dialogNow = null;
            } 
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
        string _massOld = _mass.ToString();
        string _massNew;

        if (_massOld.Length > 5)
        {
            _massNew = _massOld[0].ToString() + _massOld[1].ToString() + _massOld[2].ToString() + _massOld[3].ToString() + _massOld[4].ToString();
        }
        else
            _massNew = _massOld;

        textMass[0].text = _massNew;
        textMass[1].text = "/ " + _maxMass.ToString();
    }

    public void UpdateHealth(float _health)
    {
        textHealth.text = ((int)_health).ToString();
        sliderHealth.value = ((int)_health);
    }

    public void UpdateEnergy(float _energy)
    {
        textEnergy.text = ((int)_energy).ToString();
        sliderEnergy.value = ((int)_energy);
    }
}
