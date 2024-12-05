using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum TypeButton { Shoot, Reload, SetWeapon };

public class GUIHandler : MonoBehaviour
{
    [SerializeField] public Inventory inventory;
    [SerializeField] private GameObject inventoryObject;
    [SerializeField] private GameObject otfitObject;
    [SerializeField] private GameObject npcObject;
    [SerializeField] private FixedJoystick fixedJoystick;
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

    public IInput input;

    private void OnEnable()
    {
        if (input != null)
        {
            inventory.OnChangeOutfit += OnSetItemOutfit;
            input.OnInteractionPack += inventory.OnBackpackNPC;
            input.OnInteractionPack += OpenNPCPack;
        }
    }

    private void OnDisable()
    {
        if (input != null)
        {
            inventory.OnChangeOutfit -= OnSetItemOutfit;
            input.OnInteractionPack -= inventory.OnBackpackNPC;
            input.OnInteractionPack -= OpenNPCPack;
        }
    }

    public void Initialization()
    {
        input = new MobileInput(fixedJoystick);

        inventory.OnChangeOutfit += OnSetItemOutfit;
        input.OnInteractionPack += inventory.OnBackpackNPC;
        input.OnInteractionPack += OpenNPCPack;

        inventory.CreateList();
    }

    private void Update()
    {
        input.ReadMovement();
    }

    public void SetIsShoot(bool _isActiv) => input.ReadButtonShoot(_isActiv);
    public void SetReload() => input.ReadButtonReload();
    public void SetLight() => input.ReadButtonLight();
    public void SetNumWeapon(int _num) => input.ReadNumWeapon(_num);
    public void SetActivInv(bool _is)
    {
        inventoryObject.SetActive(_is);
        otfitObject.SetActive(_is);
        npcObject.SetActive(false);
    }
    public void SetPressMultiButton() => input.ReadPressMultiButton();
    public void OpenNPCPack(NPCBackpack _pack)
    {
        inventoryObject.SetActive(true);
        otfitObject.SetActive(false);
        npcObject.SetActive(true);
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
