using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] public GUIHandler handlerGUI;
    [SerializeField] public Movement movement;
    [SerializeField] public WeaponManager weaponManager;
    [SerializeField] public LightManager lightManager;
    [SerializeField] public OutFitManager outfit;
    [SerializeField] public Health health;
    [SerializeField] public Energy energy;

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

            handlerGUI.input.OnResetOutfit += outfit.OnResetOutfit;
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

            handlerGUI.input.OnResetOutfit += outfit.OnResetOutfit;
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

            handlerGUI.input.OnResetOutfit -= outfit.OnResetOutfit;
        }    
    }

    private void Update()
    {
        if (weaponManager._numWeapon <= 1 && weaponManager._guns[weaponManager._numWeapon] != null && weaponManager._flagWeapon)
            handlerGUI.UpdateAmmos(handlerGUI.inventory.GetCountAmmos(weaponManager._guns[weaponManager._numWeapon]),
                                                                      weaponManager._guns[weaponManager._numWeapon].currentAmmos);
        else
            handlerGUI.UpdateAmmos(-1, -1);

        float reason;

        handlerGUI.UpdateMass(outfit.MaxMass, handlerGUI.inventory.allMass);
        if ((reason = handlerGUI.inventory.allMass - outfit.MaxMass) > 0)
            movement.debufSpeed = reason / 10;
        else
            movement.debufSpeed = 0;

        handlerGUI.UpdateHealth(health.health);
        handlerGUI.UpdateEnergy(energy.energy);
    }
}
