using System.Collections;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEditor.Progress;

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

    private bool _isInterecrion = false;
    private Coroutine _coroutine;

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

            handlerGUI.input.OnCastBolt += weaponManager.OnCastBolt;
        } */ 
    }

    private void Awake()
    {
        transform.position = new Vector2(SaveHeandler.SessionSave.pos.x,
                                         SaveHeandler.SessionSave.pos.y);

        if (SaveHeandler.SessionSave.pos.flipX < 0)
        {
            movement.toRight = false;
            Vector3 scaler = transform.localScale;
            scaler.x = scaler.x * -1;
            transform.localScale = scaler;
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

            handlerGUI.input.OnCastBolt += weaponManager.OnCastBolt;
        }

        SaveHeandler.OnSaveSession += SaveSessinon;
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

            handlerGUI.input.OnCastBolt -= weaponManager.OnCastBolt;
        }

        SaveHeandler.OnSaveSession -= SaveSessinon;
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

        if (_coroutine == null)
            _coroutine = StartCoroutine(Check());
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
                    if (_npc.backpack)
                    {
                        handlerGUI.OpenNPCPack();
                        handlerGUI.inventory.OnBackpackNPC(_npc);
                    }
                    else if (_dialogList)
                    {
                        handlerGUI.SetDialog(_dialogList, _dialogList.startDialog);
                    }
                }
            }
        }
    }

    IEnumerator Check()
    {
        while (true)
        {
            RaycastHit2D _hit = Physics2D.BoxCast(transform.position, new Vector2(sizeCheck, 3f), 0f, Vector2.zero, 0f, layer);

            if (_hit)
            {
                if (!_isInterecrion)
                {
                    Collider2D _col = _hit.collider;

                    NPC _npc = _col.gameObject.GetComponentInParent<NPC>();
                    DialogList _dialogList = _col.gameObject.GetComponentInParent<DialogList>();
                    Entry _entry = _col.gameObject.GetComponent<Entry>();

                    if (_entry)
                        handlerGUI.ButtonInterection(null, null, null, _entry);
                    else if (_npc)
                    {
                        if (_npc.backpack)
                            handlerGUI.ButtonInterection(null, null, _npc);
                        else if (_dialogList)
                            handlerGUI.ButtonInterection(_dialogList, _dialogList.startDialog);
                    }

                    _isInterecrion = true;
                }
            }
            else
            {
                handlerGUI.ButtonInterection();
                _isInterecrion = false;
            }
                

            yield return new WaitForEndOfFrame();
        }
    }

    private void SaveSessinon()
    {
        SaveHeandler.SessionSave.pos.y = 0.8f;

        SaveHeandler.SessionSave.pos.flipX = (int)transform.localScale.x;
    }
}
