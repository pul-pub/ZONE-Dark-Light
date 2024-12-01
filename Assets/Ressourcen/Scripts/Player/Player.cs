using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GUIHandler handlerGUI;
    [SerializeField] private Movement movement;
    [SerializeField] private WeaponManager weaponManager;
    [SerializeField] private LightManager lightManager;

    public void Initialization()
    {
        weaponManager.inv = handlerGUI.inventory;

        if (handlerGUI.input != null)
        {
            handlerGUI.input.OnMove += movement.Move;

            handlerGUI.input.OnShoot += weaponManager.SetIsShoot;
            handlerGUI.input.OnSetNumWeapon += weaponManager.SetNumberWeapon;
            handlerGUI.input.OnResetOutfit += weaponManager.SetGunList;
            handlerGUI.input.OnRload += weaponManager.StartReload;

            handlerGUI.input.OnResetOutfit += lightManager.OnResetOutfit;
            handlerGUI.input.OnLight += lightManager.OnSetLight;
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

            handlerGUI.input.OnResetOutfit += lightManager.OnResetOutfit;
            handlerGUI.input.OnLight += lightManager.OnSetLight;
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

            handlerGUI.input.OnResetOutfit -= lightManager.OnResetOutfit;
            handlerGUI.input.OnLight -= lightManager.OnSetLight;
        }    
    }

    private void Update()
    {
        if (weaponManager._numWeapon <= 1 && weaponManager._guns[weaponManager._numWeapon] != null && weaponManager._flagWeapon)
            handlerGUI.UpdateAmmos(handlerGUI.inventory.GetCountAmmos(weaponManager._guns[weaponManager._numWeapon]),
                                                                      weaponManager._guns[weaponManager._numWeapon].currentAmmos);
        else
            handlerGUI.UpdateAmmos(-1, -1);
    }
}
