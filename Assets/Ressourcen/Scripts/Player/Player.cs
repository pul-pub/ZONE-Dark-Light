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
            handlerGUI.input.OnRload += weaponManager.StartReload;
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
            handlerGUI.input.OnRload += weaponManager.StartReload;
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
            handlerGUI.input.OnRload -= weaponManager.StartReload;
        }    
    }

    private void Update()
    {
        if (weaponManager._guns[weaponManager._numWeapon] != null && weaponManager._flagWeapon)
            handlerGUI.UpdateAmmos(100, weaponManager._guns[weaponManager._numWeapon].currentAmmos);
        else
            handlerGUI.UpdateAmmos(-1, -1);
    }
}
