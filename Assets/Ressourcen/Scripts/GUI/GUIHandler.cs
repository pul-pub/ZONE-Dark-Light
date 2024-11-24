using UnityEngine;

public enum TypeButton { Shoot, Reload, SetWeapon };

public class GUIHandler : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private FixedJoystick fixedJoystick;

    public IInput input;

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
    //public void SetEvent(string _type) => input.ReadButton(_type);
    public void SetNumWeapon(int _num) => input.ReadNumWeapon(_num);
}
