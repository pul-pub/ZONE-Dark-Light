using UnityEngine;
using UnityEngine.UI;

public enum TypeButton { Shoot, Reload, SetWeapon };

public class GUIHandler : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private FixedJoystick fixedJoystick;
    [SerializeField] private Image[] imgSetNumWeapon;

    public IInput input;

    private void OnEnable()
    {
        inventory.OnChangeOutfit += OnSetItemOutfit;
    }

    private void OnDisable()
    {
        inventory.OnChangeOutfit -= OnSetItemOutfit;
    }

    public void Initialization()
    {
        input = new MobileInput(fixedJoystick);

        inventory.CreateList();
    }

    private void Update()
    {
        input.ReadMovement();
    }

    public void SetIsShoot(bool _isActiv) => input.ReadButtonShoot(_isActiv);

    public void SetNumWeapon(int _num) => input.ReadNumWeapon(_num);

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
    }
}
