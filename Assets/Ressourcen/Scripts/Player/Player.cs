using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GUIHandler handlerGUI;
    [SerializeField] private Movement movement;
    [SerializeField] private WeaponManager weaponManager;

    public void Initialization()
    {
        if (handlerGUI.input != null)
        {
            handlerGUI.input.OnMove += movement.Move;
            handlerGUI.input.OnShoot += weaponManager.SetIsShoot;
            handlerGUI.input.OnSetNumWeapon += weaponManager.SetNumberWeapon;
            handlerGUI.input.OnResetOutfit += weaponManager.SetGunList;
        }  
    }

    private void OnEnable()
    {
        if (handlerGUI.input != null)
        {
            handlerGUI.input.OnMove += movement.Move;
            handlerGUI.input.OnShoot += weaponManager.SetIsShoot;
            handlerGUI.input.OnSetNumWeapon += weaponManager.SetNumberWeapon;
            handlerGUI.input.OnResetOutfit += weaponManager.SetGunList;
        }    
    }

    private void OnDisable()
    {
        if (handlerGUI.input != null)
        {
            handlerGUI.input.OnMove -= movement.Move;
            handlerGUI.input.OnShoot -= weaponManager.SetIsShoot;
            handlerGUI.input.OnSetNumWeapon -= weaponManager.SetNumberWeapon;
            handlerGUI.input.OnResetOutfit -= weaponManager.SetGunList;
        }    
    }
}
