using System.Collections.Generic;
using UnityEngine;

public class OutFitManager : MonoBehaviour
{
    public int MaxMass { get => 20 + (backpack == null ? 0 : backpack.MassUp + armor.MassUp) + _buffForce; }
    public int FaceID { set => face.sprite = data.GetFace(value); }

    [SerializeField] private DataBase data;
    [Space]
    [SerializeField] private SpriteRenderer[] armorRenders;
    [SerializeField] private SpriteRenderer backpackSpRend;
    [SerializeField] private SpriteRenderer face;
    [SerializeField] private Animator animatorLeg;

    private BackpackObject backpack;
    private ArmorObject armor;
    private int _buffForce = 0;

    public Dictionary<string, Sprite> GetImages()
    {
        Dictionary<string, Sprite> _dict = new Dictionary<string, Sprite>()
        {
            { "FAC", face.sprite },
            { "BOD", armor.ImgBody },
            { "MAS", armor.ImgHead },
            { "HND", armor.ImgHand },
            { "PAK", backpack ? backpack.ImgBackpack : null },
            { "LEG", armor.ImgLeg }
        };

        return _dict;
    }

    public void OnResetOutfit(Dictionary<string, IItem> _items)
    {
        IItem _item;
        if (_items.TryGetValue("ARM", out _item))
            armor = data.GetArmor(_item.Id);
        else
            armor = data.GetArmor("ARM000");

        if (_items.TryGetValue("PAK", out _item))
            backpack = data.GetBackpack(_item.Id);
        else
            backpack = null;

        UpdateGraphics();
    }

    private void UpdateGraphics()
    {
        if (armor.ImgHead != null)
            armorRenders[0].sprite = armor.ImgHead;
        armorRenders[1].sprite = armor.ImgBody;
        armorRenders[2].sprite = armor.ImgHand;
        armorRenders[3].sprite = armor.ImgHand;

        animatorLeg.runtimeAnimatorController = armor.animLeg;

        backpackSpRend.gameObject.SetActive(backpack != null);
        backpackSpRend.sprite = backpack != null ? backpack.Img : null;
    }

    public void Load()
    {
        face.sprite = data.GetFace(SaveHeandler.SessionNow.idFace);
        _buffForce = SaveHeandler.SessionNow.characteristics["Сила"] * 2;
    }
}
