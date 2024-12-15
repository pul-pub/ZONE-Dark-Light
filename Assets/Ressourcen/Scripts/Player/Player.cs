using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Button F")]
    [SerializeField] private int sizeCheck;
    [SerializeField] private LayerMask layer;
    [Header("Inital")]
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
        /*
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

            handlerGUI.input.OnPressMultiButton += OnTouchMultiButton;

            handlerGUI.input.OnCastBolt += weaponManager.OnCastBolt;
        } */ 
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

            handlerGUI.input.OnPressMultiButton += OnTouchMultiButton;

            handlerGUI.input.OnCastBolt += weaponManager.OnCastBolt;
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

            handlerGUI.input.OnPressMultiButton -= OnTouchMultiButton;

            handlerGUI.input.OnCastBolt -= weaponManager.OnCastBolt;
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

    private void OnTouchMultiButton()
    {
        Collider2D[] _list = Physics2D.OverlapCircleAll(transform.position, sizeCheck, layer);

        if (_list.Length > 0)
        {
            foreach (Collider2D col in _list)
            {
                NPC _npc = col.gameObject.GetComponentInParent<NPC>();
                DialogList _dialogList = col.gameObject.GetComponentInParent<DialogList>();

                if (_npc != null)
                {
                    if (_npc.backpack != null)
                    {
                        handlerGUI.OpenNPCPack(null);
                        handlerGUI.inventory.OnBackpackNPC(_npc);
                    }
                    else if (_dialogList)
                    {
                        handlerGUI.SetDialog(_dialogList, _dialogList.startDialog);
                    }
                        
                        //handlerGUI.input.ReadStartInteraction(TypeInteraction.TackeBackpack, _npc.backpack);
                }
            }
        }
    }
}
