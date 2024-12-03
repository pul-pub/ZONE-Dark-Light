using UnityEngine;

public class OutFitManager : MonoBehaviour
{
    [SerializeField] private SpriteRenderer backpackSpRend;
    [SerializeField] private SpriteRenderer[] armorRenders;
    [SerializeField] private ArmorObject nullArmor;

    public int MaxMass { get; private set; } = 20;
    public int AntiBullet { get; private set; } = 0;
    public int AntiRadiation { get; private set; } = 0;

    private BackpackObject backpack;
    private ArmorObject armor;

    public void Awake()
    {
        if (armor == null)
            armor = nullArmor;
    }

    public void OnResetOutfit(Item[] _items)
    {
        if (_items[2] != null)
            armor = _items[2].armorObject;
        else
            armor = nullArmor;

        AntiBullet = armor.AntiBullet;
        AntiRadiation = armor.AntiRadiation;

        if (armor.ImgHead != null)
            armorRenders[0].sprite = armor.ImgHead;
        armorRenders[1].sprite = armor.ImgBody;
        armorRenders[2].sprite = armor.ImgHand;
        armorRenders[3].sprite = armor.ImgHand;

        if (_items[3] != null)
        {
            backpack = _items[3].backpackObject;
            MaxMass = 20 + backpack.massUp;

            if (armor != null)
                MaxMass += armor.massUp;

            backpackSpRend.gameObject.SetActive(true);
            backpackSpRend.sprite = backpack.img;
        }
        else
        {
            backpackSpRend.gameObject.SetActive(false);
            backpack = null;
            MaxMass = 20;

            if (armor != null)
                MaxMass += armor.massUp;
        }
    }
}
